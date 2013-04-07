using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using MTV3D65;


/*
 * Dáma, stolní hra.
 * Jakub Suchý, I.ročník, skupina 40
 * Zimní semestr 2010/2011
 */

namespace Zapoctak
{
    public partial class Dama : Form
    {
        public Dama()
        {
            InitializeComponent();
        }

        #region Deklarace

        // Promenne zvuku
        SoundPlayer sTah, sChyba, sKonec, sVarovani, sVitezBily, sVitezCerny;

        // Promenne Nastaveni
        public int Polomer = Settings1.Default.Radius;
        public int LastRadius; // Slouží pro kontrolu jestli došlo ke změně Polomeru
        public bool Oznacovani = Settings1.Default.Oznaceni;
        public float Rychlost = Settings1.Default.Rychlost;
        public bool Zvuky = Settings1.Default.Zvuky;

        // Promenne tahu
        public bool TAH;
        public bool Tahnuto;
        public bool VTahu;
        public bool PoTahu;
        public bool Konec;
        public int Vitez;
        public bool Smazano;
        public bool NecoOznaceno;
        public bool LzeOdoznacit;
        public Figurka SloSkocit;

        // Promenne sceny
        public TVEngine TV;
        public TVScene Scene;
        public TVInputEngine Input;
        public TVMaterialFactory MatFactory;
        public TVTextureFactory TexFactory;
        public TVGlobals Globals;
        public TVLightEngine light;
        public bool bDoLoop;
        public bool Pause;

        // Promenne sachovnice
        public float xc, yc, zc;
        public Policko[,] Pole;
        public Policko lastNajete, lastPoleOZN, lastSmazane;
        public List<Policko> LPTah; //List Tahnutelnych policek aneb kam muze dana figurka tahnout/skakat
        public List<Policko> LPSkok; //List Skocitelnych policek aneb co muze dana figurka skocit
        public List<Policko> LPSmaz; //List policek urcenych ke smazani
        public List<Policko> LPOznac; //List policek urcenych ke smazani

        // Promenne figurek
        public List<Figurka> LFB; //List Bilych figurek
        public List<Figurka> LFC; //List Cernych figurek

        // Promenne kamery
        float sngPositionX;
        float sngPositionY;
        float sngPositionZ;
        float snglookatX;
        float snglookatY;
        float snglookatZ;
        float sngAngleX;
        float sngAngleY;

        // Promenne mysi
        int tmpMouseX, tmpMouseY;
        bool tmpMouseB1, tmpMouseB2, tmpMouseB3;
        bool tmpMouseScrollNew;
        bool MouseDOWN;

        // Mesh mistnosti
        TVMesh Room, Obj1, Obj2;

        // Pomocne promenne
        OknoNastaveni f2;
        int vyska, sirka;
        public TVScreen2DText text;
        public TVScreen2DText text2;
        public bool otacej, doleva;
        public bool oznaceno;
        float lx, ly, t1;
        int x, y;

        float sngWalk;
        float sngStrafe;
        float t, rx, ry, t2;
        private Timer timer1;

        #endregion

        //Pro vysledny program naprosto zbytecne metody. Slouží pouze pro ladění.
        #region Pomocne metody

        private void Check_Input()
        {

            sngWalk = 0.0f;
            sngStrafe = 0.0f;

            // Check if we pressed the PAGEUP key, if so, then scene moves in +y direction
            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_PAGEUP))
            {
                Scene.SetCamera(sngPositionX, sngPositionY += 0.1f, sngPositionZ, snglookatX, snglookatY += 0.01f, snglookatZ);
            }
            // Check if we pressed the PAGEDOWN key, if so, then scene moves in -y direction

            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_PAGEDOWN))
            {
                Scene.SetCamera(sngPositionX, sngPositionY -= 0.1f, sngPositionZ, snglookatX, snglookatY -= 0.1f, snglookatZ);
            }

            // Check if we pressed the UP arrow key, if so, then we are
            // walking forward.
            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_UP))
            {
                sngWalk = 0.1f;
            }


            // If we are not walking forward, maybe we are walking backward
            // by using the DOWN arrow? If so, set walk speed to negative.
            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_DOWN))
            {
                sngWalk = -0.1f;
            }


            // Check if we pressed the LEFT arrow key, if so, then strafe
            // on the left.
            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_LEFT))
            {
                sngStrafe =0.1f;
            }


            // If we are not strafing left, maybe we want to strafe to the
            // right, using the RIGHT arrow? If so, set strafe to negative.
            if (Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_RIGHT))
            {
                sngStrafe = -0.1f;
            }


            // Get the movement of the mouse.
            Input.GetMouseState(ref tmpMouseX, ref tmpMouseY, ref tmpMouseB1, ref tmpMouseB2, ref tmpMouseB3, ref tmpMouseScrollNew);
            if (tmpMouseB2)
            {

                sngAngleX = sngAngleX - ((float)tmpMouseY / 1000);
                sngAngleY = sngAngleY - ((float)tmpMouseX / 1000);
            }
        }
        private void Check_Movement()
        {
            /*if (otacej)
                if (doleva)
                    otocKamerou(true);
                else
                    otocKamerou(false);*/

            float TimeElapsed;
            TimeElapsed = TV.TimeElapsed();

            // Simple check of the mouse.
            if (sngAngleX > 1.3f)
            {
                sngAngleX = 1.3f;
            }

            if (sngAngleX < -1.3)
            {
                sngAngleX = -1.3f;
            }

            // Update the vectors using the angles and positions.
            sngPositionX = sngPositionX + (float)(System.Math.Cos((double)sngAngleY) * sngWalk / 5.0f * TimeElapsed) + (float)(System.Math.Cos((double)sngAngleY + 3.141596 / 2) * sngStrafe / 5.0f * TimeElapsed);
            sngPositionZ = sngPositionZ + (float)(System.Math.Sin((double)sngAngleY) * sngWalk / 5.0f * TimeElapsed) + (float)(System.Math.Sin((double)sngAngleY + 3.141596 / 2) * sngStrafe / 5.0f * TimeElapsed);


            // We update the look at position.
            snglookatX = sngPositionX + (float)System.Math.Cos((double)sngAngleY);
            snglookatY = sngPositionY + (float)System.Math.Tan((double)sngAngleX);
            snglookatZ = sngPositionZ + (float)System.Math.Sin((double)sngAngleY);

            // With the new values of the camera vectors (position and
            // look at), we update the scene's camera.
            Scene.SetCamera(sngPositionX, sngPositionY, sngPositionZ, snglookatX, snglookatY, snglookatZ);

        }
        private void VypisText()
        {
            for (int i = 0; i < LPTah.Count; i++)
            {
                text.NormalFont_DrawText(LPTah[i].x + "T" + LPTah[i].y, 50.0f, 80.0f + 10.0f * i, -16710933);
            }
            for (int i = 0; i < LPSkok.Count; i++)
            {
                text2.NormalFont_DrawText(LPSkok[i].x + "S" + LPSkok[i].y, 100.0f, 80.0f + 10.0f * i, -16710933);
            }
            if (lastNajete != null)
                text2.NormalFont_DrawText(lastNajete.x.ToString() + " " + lastNajete.y.ToString() + " " + lastNajete.hodnota.ToString(), 50.0f, 60.0f, -16710933);
        }

        #endregion

        #region Hlavni metody

        //Pokud bylo najeto na policko, vrati Policko jinak null
        private Policko GetPolicko()
        {
            
            int xi, yi;
            TVCollisionResult xq = new TVCollisionResult();
            Input.GetMousePosition(ref x, ref y);
            xq = Scene.MousePick(x, y);
            xi = xq.GetCollisionMesh().GetMeshName()[0] - 48;
            yi = xq.GetCollisionMesh().GetMeshName()[2] - 48;
            if ((xi < 8 && yi < 8))
                return Pole[xi, yi];
            else
                return null;
        }
        /*Pokud vstupní souřadnice leží v poli, pak vrátí Policko, jinak null
        slouží pro výpočet Tahu/Skoku dámy*/
        private Policko GetPolicko(int x, int y)
        {
            if ((x < 8 && y < 8 && x >= 0 && y >= 0))
                return Pole[x, y];
            return null;
        }
        //Naplnění listu LPTah všemi možnými možnosti táhnutí pro dané Policko
        private void Naplneni_LPTah(Policko p)
        {
            if (LPTah.Count != 0) LPTah.Clear();
            int Hod = p.hodnota;
            int x = p.x, y = p.y, lHod = 10, pHod = 10, llHod = 10, ppHod = 10;
            switch (Hod)
            {
                case 1:     // Bila obicejna figurka

                    if (y <= 6)    //Horni okraj o jedno policko
                    {
                        if (x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[x - 1, y + 1].hodnota;
                            if (lHod == 0) LPTah.Add(Pole[x - 1, y + 1]);
                        }
                        if (x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[x + 1, y + 1].hodnota;
                            if (pHod == 0) LPTah.Add(Pole[x + 1, y + 1]);
                        }
                        if (y <= 5)  //Horni okraj o dve policka
                        {
                            if (x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[x - 2, y + 2].hodnota;
                                if ((lHod == 2 || lHod == 4) && llHod == 0) LPTah.Add(Pole[x - 2, y + 2]);
                            }
                            if (x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[x + 2, y + 2].hodnota;
                                if ((pHod == 2 || pHod == 4) && ppHod == 0) LPTah.Add(Pole[x + 2, y + 2]);
                            }
                        }
                    }
                    break;
                case 2:     // Cerna obicejna figurka
                    if (y >= 1)    //Dolni okraj o jedno policko
                    {
                        if (x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[x - 1, y - 1].hodnota;
                            if (lHod == 0) LPTah.Add(Pole[x - 1, y - 1]);
                        }
                        if (x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[x + 1, y - 1].hodnota;
                            if (pHod == 0) LPTah.Add(Pole[x + 1, y - 1]);
                        }
                        if (y >= 2)  //Dolni okraj o dve policka
                        {
                            if (x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[x - 2, y - 2].hodnota;
                                if ((lHod == 1 || lHod == 3) && llHod == 0) LPTah.Add(Pole[x - 2, y - 2]);
                            }
                            if (x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[x + 2, y - 2].hodnota;
                                if ((pHod == 1 || pHod == 3) && ppHod == 0) LPTah.Add(Pole[x + 2, y - 2]);
                            }
                        }
                    }
                    break;
                //TODO: figurky DAM
                case 3: // Bila dama
                    Policko pom;
                    Policko LastPom;
                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(p.x, p.y);
                        x = p.x; y = p.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null)
                        {
                            if (pom.hodnota == 1 || pom.hodnota == 3) break;
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom == null) LPTah.Add(pom);
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) LPTah.Add(pom);
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom == null) LastPom = pom;

                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;
                case 4: // Cerna dama
                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(p.x, p.y);
                        x = p.x; y = p.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null)
                        {
                            if (pom.hodnota == 2 || pom.hodnota == 4) break;
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom == null) LPTah.Add(pom);
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) LPTah.Add(pom);
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom == null) LastPom = pom;
                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;
                default:
                    MessageBox.Show("Error pri naplneni LPT policka bez figurky!!");
                    break;
            }
        }
        //Naplnění listu LPSkok Policky takovymi na ktere muzem z daneho Policka skakat
        private void Naplneni_LPSkok(Policko pp)
        {
            if (LPSkok.Count != 0) LPSkok.Clear();
            int Hod = pp.hodnota;
            int x = pp.x, y = pp.y, lHod = 10, pHod = 10, llHod = 10, ppHod = 10;
            switch (Hod)
            {
                case 1: // Bila normalni figurka
                    if (y <= 5)  //Horni okraj o dve policka
                    {
                        if (x >= 2)    //Levy oraj o dve policka
                        {
                            lHod = Pole[x - 1, y + 1].hodnota;
                            llHod = Pole[x - 2, y + 2].hodnota;
                            if ((lHod == 2 || lHod == 4) && llHod == 0) LPSkok.Add(Pole[x - 2, y + 2]);
                        }
                        if (x <= 5)    //Pravy okraj o dve policka
                        {
                            pHod = Pole[x + 1, y + 1].hodnota;
                            ppHod = Pole[x + 2, y + 2].hodnota;
                            if ((pHod == 2 || pHod == 4) && ppHod == 0) LPSkok.Add(Pole[x + 2, y + 2]);
                        }
                    }
                    break;
                case 2: // Cerna normalni figurka
                    if (y >= 2)    //Dolni okraj o dve policka
                    {
                        if (x >= 2)    //Levy oraj o dve policka
                        {
                            lHod = Pole[x - 1, y - 1].hodnota;
                            llHod = Pole[x - 2, y - 2].hodnota;
                            if ((lHod == 1 || lHod == 3) && llHod == 0) LPSkok.Add(Pole[x - 2, y - 2]);
                        }
                        if (x <= 5)    //Pravy okraj o dve policka
                        {
                            pHod = Pole[x + 1, y - 1].hodnota;
                            ppHod = Pole[x + 2, y - 2].hodnota;
                            if ((pHod == 1 || pHod == 3) && ppHod == 0) LPSkok.Add(Pole[x + 2, y - 2]);
                        }
                    }
                    break;
                //TODO: Lze skocit s damou?
                case 3: // Bila dama
                    Policko pom;
                    Policko LastPom;
                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(pp.x, pp.y);
                        x = pp.x; y = pp.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null) // Doprava nahoru
                        {
                            if (pom.hodnota == 1 || pom.hodnota == 3) break;
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) LPSkok.Add(pom);
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom == null) LastPom = pom;

                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;
                case 4: // Cerna dama
                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(pp.x, pp.y);
                        x = pp.x; y = pp.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null) // Doprava nahoru
                        {
                            if (pom.hodnota == 2 || pom.hodnota == 4) break;
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) LPSkok.Add(pom);
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom == null) LastPom = pom;

                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;
            }
        }
        /*Přesune figurku z bodu A do bodu B
        Jestli mezi A a B je figurka protivnika, prida se ke smazani*/
        private void Presun(ref Policko a, Policko b)
        {
            //Nastaveni i podle smeru posuvu
            int x = b.x - a.x, y = b.y - a.y, i = 10;
            if (x > 0 && y > 0) i = 0;
            if (x > 0 && y < 0) i = 1;
            if (x < 0 && y < 0) i = 2;
            if (x < 0 && y > 0) i = 3;

            Policko pom;
            pom = GetPolicko(a.x, a.y);
            x = a.x; y = a.y;

            if (i == 0) { x++; y++; }
            else if (i == 1) { x++; y--; }
            else if (i == 2) { x--; y--; }
            else if (i == 3) { x--; y++; }
            pom = GetPolicko(x, y);

            while (pom.x != b.x && pom.y != b.y)
            {
                if (pom.hodnota != 0)
                {
                    PridatKeSmazani(pom);
                    break;
                }
                if (i == 0) { x++; y++; }
                else if (i == 1) { x++; y--; }
                else if (i == 2) { x--; y--; }
                else if (i == 3) { x--; y++; }
                pom = GetPolicko(x, y);
            }

            //Prenastaveni hodnot -> presun
            b.hodnota = a.hodnota;
            b.fig = a.fig;
            b.fig.mesh.SetPosition((float)5 * b.x + xc, 1.0f + yc, (float)5 * b.y + zc);
            b.fig.x = b.x;
            b.fig.y = b.y;
            b.fig.mesh.SetMeshName(b.x.ToString() + "F" + b.y.ToString());

            a.hodnota = 0;
            a.fig = null;


            a.mesh.SetTexture(Globals.GetTex("seda"), -1);
            NecoOznaceno = false;
            a.oznaceno = false;
            a = null;
        }
        //Ulozi vstupni Policko do listu ke smazani
        private void PridatKeSmazani(Policko p)
        {
            OznacKeSmazani(p);
            p.skoceno = true;
            LPSmaz.Add(p);
        }
        //Vymaze Policka v listu ke smazani
        private void SmazList()
        {
            //Posle vsechny polozky Listu ke smazani
            foreach (Policko item in LPSmaz)
            {
                Smaz(item.x, item.y);
            }
            //Vycisti list
            LPSmaz.Clear();
        }
        //Smaze policko na souradnicich x,y pole
        private void Smaz(int x, int y)
        {
            //Kontrola co se maze
            if (Pole[x, y].fig.hodnota == 1 || Pole[x, y].fig.hodnota == 3)
                foreach (Figurka item in LFB)
                {
                    if (Pole[x, y].fig.x == item.x && Pole[x, y].fig.y == item.y)
                    {
                        LFB.Remove(item);
                        break;
                    }
                }
            if (Pole[x, y].fig.hodnota == 2 || Pole[x, y].fig.hodnota == 4)
                foreach (Figurka item in LFC)
                {
                    if (Pole[x, y].fig.x == item.x && Pole[x, y].fig.y == item.y)
                    {
                        LFC.Remove(item);
                        break;
                    }
                }
            //Mazani
            Pole[x, y].skoceno = false;
            Pole[x, y].fig.mesh.Destroy();
            Pole[x, y].hodnota = 0;
            Pole[x, y].fig = null;
        }
        //Jeli zapnuto oznacovani tahu, pak se oznaci tahnutelna,skocitelna policka
        private void ProvedOznacovani(List<Policko> list, bool bozn)
        {
            if (Oznacovani && bozn)
                LPOznac.AddRange(list);
            if (Oznacovani && LPOznac.Count != 0)
                foreach (Policko item in LPOznac)
                    OznacTahnutelne(item);
        }
        //Odoznaceni vsech tahnutelnych, skocitelnych oznacenych policek
        public void ProvedOdOznacovani()
        {
            if (LPOznac.Count != 0)
            {
                foreach (Policko item in LPOznac)
                    OdOznacTahnutelne(item);
                LPOznac.Clear();
            }
        }

        #endregion

        #region Kontrolujici metody

        //Tahle metoda kontroluje jestli uz se kamera po tahu otocila
        private void JeKameraOtocena()
        {
            if (TAH)
            {
                //Konec otaceni kamery na cernou stranu
                if (t1 >= Math.PI + (float)Math.PI / 2)
                {
                    MStrip.Enabled = true;
                    label2.Text = "BÍLÝ";
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    otacej = false;
                    doleva = false;
                    PoTahu = false;
                    MouseDOWN = false;
                    if (KontrolaKonce1())
                    {
                        Konec = true;
                    }

                    t1 = -(float)(Math.PI / 2);
                    sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
                    sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
                    Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);

                    //Konec
                    JeKonecHry();
                }
            }
            else
            {
                //Konec otaceni kamery na bilou stranu
                if (t1 <= -Math.PI - (float)Math.PI / 2)
                {
                    MStrip.Enabled = true;
                    label2.Text = "ČERNÝ";
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    if (KontrolaKonce1())
                    {
                        Konec = true;
                    }

                    otacej = false;
                    doleva = true;
                    PoTahu = false;
                    MouseDOWN = false;
                    t1 = (float)Math.PI / 2;
                    sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
                    sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
                    Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);

                    //Konec
                    JeKonecHry();
                }
            }
        }
        //Kontroluje zda maji oba hraci nejake figurky
        private bool KontrolaKonce1()
        {
            //Kontrola jestli ma bily nejake figurky
            if (LFB.Count == 0)
            {
                Vitez = 2;
                return true;
            }
            //Kontrola jestli ma cerny nejake figurky
            else if (LFC.Count == 0)
            {
                Vitez = 1;
                return true;
            }
            return false;
        }
        //Kontroluje jestli hrac na vstupu muze tahnout
        private bool KontrolaKonce2(bool kdo)
        {
            //Kontrola jestli ten kdo je na tahu muza tahnout
            if (!Lze_Tahnout(kdo))
            {
                if (kdo)
                    Vitez = 2;
                else
                    Vitez = 1;
                return true;
            }
            return false;
        }
        //Jeli konec, zablokuje hru a vyhlasi viteze.
        private void JeKonecHry()
        {
            if (Konec)
            {
                Konec = false;
                label2.Text = "----";
                label3.Text = "KONEC";
                label4.Text = "VÍTĚZEM SE";
                if (Vitez == 1)
                {
                    if (Zvuky)
                        sVitezBily.Play();
                    label5.Text = "STÁVÁ BÍLÝ";
                }
                else if (Vitez == 2)
                {
                    if (Zvuky)
                        sVitezCerny.Play();
                    label5.Text = "STÁVÁ ČERNÝ";
                }
                Pause = true;

            }
        }
        //Provedeni zmeny tahu a otoceni kamery
        private void JeTahnuto()
        {
            if (Tahnuto)
            {
                //Prepnuti tahu
                if (TAH) TAH = false;
                else TAH = true;

                if (Zvuky && sVarovani.Tag == null)
                    sTah.Play();

                //Nastaveni ridicich promennych
                PoTahu = true;
                Tahnuto = false;
                VTahu = false;
                SloSkocit = null;
                Smazano = false;
                LzeOdoznacit = true;
                MouseDOWN = true;
                sVarovani.Tag = null;
                MStrip.Enabled = false;

                //Smazani skocenych figurek
                if (LPSmaz.Count != 0)
                    SmazList();

                if (LPOznac.Count != 0)
                {
                    foreach (Policko item in LPOznac)
                    {

                        OdOznacTahnutelne(item);
                    }
                    LPOznac.Clear();
                }
            }
        }
        //Aktualizuje uzivatelska nastaveni
        private void ZjistiNastaveni()
        {
            //Precte uzivatelsky nastaveni a zmeni vzdalenost kamery od stredu sachovnice
            Polomer = Settings1.Default.Radius;
            if (LastRadius != Polomer)
            {
                LastRadius = Polomer;
                sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
                sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
                Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);
            }
            //Precte a zmeni rychlost otaceni
            Rychlost = Settings1.Default.Rychlost;
            Oznacovani = Settings1.Default.Oznaceni;
            Zvuky = Settings1.Default.Zvuky;
        }
        //Vraci figurku se kterou slo v danou chvili skakat
        private void SloNecoSkocit()
        {
            //V nasledujicich dvou ifech se zjistuje prvni figurka kterou jde skocit a ulozi se do SloSkocit
            if ((lastPoleOZN.hodnota == 1 || lastPoleOZN.hodnota == 3) && TAH)
            {
                foreach (Figurka item2 in LFB)
                {
                    if (Lze_Skocit(item2) && !Smazano)
                    {
                        SloSkocit = item2;
                        break;
                    }
                }
            }
            if ((lastPoleOZN.hodnota == 2 || lastPoleOZN.hodnota == 4) && !TAH)
            {
                foreach (Figurka item2 in LFC)
                {
                    if (Lze_Skocit(item2) && !Smazano)
                    {
                        SloSkocit = item2;
                        break;
                    }
                }
            }
        }
        //Kontrola jestli zmenit figurku na damu
        private void JeFigurkaNaKonci(Policko p)
        {
            if (p.hodnota == 1 && p.y == 7)
            {
                p.fig.mesh.Destroy();
                LFB.Remove(p.fig);
                Figurka f = new Figurka(p.x, p.y, 3);
                LFB.Add(f);

                f.mesh = new TVMesh();
                f.mesh = Scene.CreateMeshBuilder();
                f.mesh = Cylinder(2, 1, 100, 0, 0, 0, 1, 1);
                f.mesh.SetPosition((float)5 * f.x + xc, yc + 1.5f, (float)5 * f.y + zc);
                f.mesh.SetMeshName(f.x.ToString() + "F" + f.y.ToString());
                f.mesh.SetTexture(Globals.GetTex("bilad"), -1);
                p.hodnota = f.hodnota;
                p.fig = f;
                Upozorneni("Bílý dojel na konec šachovnice, měním figurku na dámu.");
            }
            if (p.hodnota == 2 && p.y == 0)
            {
                p.fig.mesh.Destroy();
                LFC.Remove(p.fig);
                Figurka f = new Figurka(p.x, p.y, 4);
                LFC.Add(f);

                f.mesh = new TVMesh();
                f.mesh = Scene.CreateMeshBuilder();
                f.mesh = Cylinder(2, 1, 100, 0, 0, 0, 1, 1);
                f.mesh.SetPosition((float)5 * f.x + xc, yc + 1.5f, (float)5 * f.y + zc);
                f.mesh.SetMeshName(f.x.ToString() + "F" + f.y.ToString());
                f.mesh.SetTexture(Globals.GetTex("cernad"), -1);
                Pole[f.x, f.y].hodnota = f.hodnota;
                Pole[f.x, f.y].fig = f;
                Upozorneni("Černý dojel na konec šachovnice, měním figurku na dámu.");
            }
        }
        //Kontrola jestli dany hrac na vstupu muze vubec hrat
        private bool Lze_Tahnout(bool bily)
        {
            if (bily)
            {
                //Muze bily tahnout?
                foreach (Figurka item in LFB)
                {
                    int lHod = 10, pHod = 10, llHod = 10, ppHod = 10;
                    //Pro normalni bile figurky a bile damy smerem dolu
                    if (item.y <= 6)    //Horni okraj o jedno policko
                    {
                        if (item.x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[item.x - 1, item.y + 1].hodnota;
                            if (lHod == 0) return true;
                        }
                        if (item.x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[item.x + 1, item.y + 1].hodnota;
                            if (pHod == 0) return true;
                        }
                        if (item.y <= 5)  //Horni okraj o dve policka
                        {
                            if (item.x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[item.x - 2, item.y + 2].hodnota;
                                if ((lHod == 2 || lHod == 4) && llHod == 0) return true;
                            }
                            if (item.x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[item.x + 2, item.y + 2].hodnota;
                                if ((pHod == 2 || pHod == 4) && ppHod == 0) return true;
                            }
                        }
                    }
                    //Pro bile damy smerem dolu
                    if ((Pole[item.x, item.y].hodnota == 3) && (item.y >= 1))   //Dolni okraj o jedno policko
                    {
                        lHod = 10; pHod = 10; llHod = 10; ppHod = 10;
                        if (item.x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[item.x - 1, item.y - 1].hodnota;
                            if (lHod == 0) return true;
                        }
                        if (item.x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[item.x + 1, item.y - 1].hodnota;
                            if (pHod == 0) return true;
                        }
                        if (item.y >= 2)    //Dolni okraj o dve policka
                        {
                            if (item.x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[item.x - 2, item.y - 2].hodnota;
                                if ((lHod == 2 || lHod == 4) && llHod == 0) return true;
                            }
                            if (item.x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[item.x + 2, item.y - 2].hodnota;
                                if ((pHod == 2 || pHod == 4) && ppHod == 0) return true;
                            }
                        }
                    }
                }
            }
            else
            {
                //Muze cerny tahnout?
                foreach (Figurka item in LFC)
                {
                    int lHod = 10, pHod = 10, llHod = 10, ppHod = 10;
                    //Pro normalni cerne figurky a cerne damy smerem dolu
                    if (item.y >= 1)    //Dolni okraj o jedno policko
                    {
                        if (item.x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[item.x - 1, item.y - 1].hodnota;
                            if (lHod == 0) return true;
                        }
                        if (item.x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[item.x + 1, item.y - 1].hodnota;
                            if (pHod == 0) return true;
                        }
                        if (item.y >= 2)    //Dolni okraj o dve policka
                        {
                            if (item.x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[item.x - 2, item.y - 2].hodnota;
                                if ((lHod == 1 || lHod == 3) && llHod == 0) return true;
                            }
                            if (item.x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[item.x + 2, item.y - 2].hodnota;
                                if ((pHod == 1 || pHod == 3) && ppHod == 0) return true;
                            }
                        }
                    }
                    //Pro cerne damy smerem nahoru
                    if ((Pole[item.x, item.y].hodnota == 4) && (item.y <= 6))   //Horni okraj o jedno policko
                    {
                        lHod = 10; pHod = 10; llHod = 10; ppHod = 10;
                        if (item.x >= 1)    //Levy okraj o jedno policko
                        {
                            lHod = Pole[item.x - 1, item.y + 1].hodnota;
                            if (lHod == 0) return true;
                        }
                        if (item.x <= 6)    //Pravy okraj o jedno policko
                        {
                            pHod = Pole[item.x + 1, item.y + 1].hodnota;
                            if (pHod == 0) return true;
                        }
                        if (item.y <= 5)  //Horni okraj o dve policka
                        {
                            if (item.x >= 2)    //Levy oraj o dve policka
                            {
                                llHod = Pole[item.x - 2, item.y + 2].hodnota;
                                if ((lHod == 1 || lHod == 3) && llHod == 0) return true;
                            }
                            if (item.x <= 5)    //Pravy okraj o dve policka
                            {
                                ppHod = Pole[item.x + 2, item.y + 2].hodnota;
                                if ((pHod == 1 || pHod == 3) && ppHod == 0) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        //Kontrola jestli s danou figurkou muzeme skakat
        private bool Lze_Skocit(Figurka f)
        {
            int Hod = f.hodnota;
            int lHod = 10, pHod = 10, llHod = 10, ppHod = 10;
            bool skoc = true;
            switch (Hod)
            {
                case 1:  // Bila normalni figurka
                    if (f.y <= 5)  //Horni okraj o dve policka
                    {
                        if (f.x >= 2)    //Levy oraj o dve policka
                        {
                            skoc = Pole[f.x - 1, f.y + 1].skoceno;
                            lHod = Pole[f.x - 1, f.y + 1].hodnota;
                            llHod = Pole[f.x - 2, f.y + 2].hodnota;
                            if ((lHod == 2 || lHod == 4) && llHod == 0 && !skoc) return true;
                        }
                        if (f.x <= 5)    //Pravy okraj o dve policka
                        {
                            skoc = Pole[f.x + 1, f.y + 1].skoceno;
                            pHod = Pole[f.x + 1, f.y + 1].hodnota;
                            ppHod = Pole[f.x + 2, f.y + 2].hodnota;
                            if ((pHod == 2 || pHod == 4) && ppHod == 0 && !skoc) return true;
                        }
                    }
                    break;
                case 2: // Cerna normalni figurka
                    if (f.y >= 2)    //Dolni okraj o dve policka
                    {
                        if (f.x >= 2)    //Levy oraj o dve policka
                        {
                            skoc = Pole[f.x - 1, f.y - 1].skoceno;
                            lHod = Pole[f.x - 1, f.y - 1].hodnota;
                            llHod = Pole[f.x - 2, f.y - 2].hodnota;
                            if ((lHod == 1 || lHod == 3) && llHod == 0 && !skoc) return true;
                        }
                        if (f.x <= 5)    //Pravy okraj o dve policka
                        {
                            skoc = Pole[f.x + 1, f.y - 1].skoceno;
                            pHod = Pole[f.x + 1, f.y - 1].hodnota;
                            ppHod = Pole[f.x + 2, f.y - 2].hodnota;
                            if ((pHod == 1 || pHod == 3) && ppHod == 0 && !skoc) return true;
                        }
                    }
                    break;
                case 3: // Bila dama
                    Policko pom;
                    int x, y;
                    Policko LastPom;


                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(f.x, f.y);
                        x = f.x; y = f.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null) // Doprava nahoru
                        {
                            if (pom.hodnota == 1 || pom.hodnota == 3) break;
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) return true;
                            if ((pom.hodnota == 2 || pom.hodnota == 4) && LastPom == null) LastPom = pom;

                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;

                case 4: // Cerna dama
                    for (int i = 0; i < 4; i++)
                    {
                        pom = GetPolicko(f.x, f.y);
                        x = f.x; y = f.y;
                        LastPom = null;

                        if (i == 0) { x++; y++; }
                        else if (i == 1) { x++; y--; }
                        else if (i == 2) { x--; y--; }
                        else if (i == 3) { x--; y++; }
                        pom = GetPolicko(x, y);

                        while (pom != null) // Doprava nahoru
                        {
                            if (pom.hodnota == 2 || pom.hodnota == 4) break;
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom != null) break;
                            if (pom.hodnota == 0 && LastPom != null)
                                if (!LastPom.skoceno) return true;
                            if ((pom.hodnota == 1 || pom.hodnota == 3) && LastPom == null) LastPom = pom;

                            if (i == 0) { x++; y++; }
                            else if (i == 1) { x++; y--; }
                            else if (i == 2) { x--; y--; }
                            else if (i == 3) { x--; y++; }
                            pom = GetPolicko(x, y);
                        }
                    }
                    break;
            }
            return false;
        }

        #endregion

        #region Vytvarejici metody

        //Inicializace enginu
        private void VytvorScenu()
        {
            TV = new TVEngine();

            //Povoleni debug zprav
            TV.SetDebugMode(true, true);
            //Nastaveni debug souboru pro ladeni chyb
            TV.SetDebugFile(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\debugfile.txt");

            //Incizializace enginu
            TV.Init3DWindowed(this.panel2.Handle, true);

            //Nastaveni defaultniho adresare
            TV.SetSearchDirectory(Environment.CurrentDirectory);

            //Nastaveni samoobnovy velikosti obrazu dle okna
            TV.GetViewport().SetAutoResize(true);

            //Zobrazeni FPS
            TV.DisplayFPS(false);

            //Nastaveni preferovaneho uhloveho systemu
            TV.SetAngleSystem(MTV3D65.CONST_TV_ANGLE.TV_ANGLE_DEGREE);

            //Inicializace vsechno enginovych doplnku
            Scene = new TVScene();
            Globals = new TVGlobals();
            Input = new TVInputEngine();
            MatFactory = new TVMaterialFactory();
            TexFactory = new TVTextureFactory();

            //Povoleni mysi a klavesnice na vsup
            Input.Initialize(true, true);
        }
        //Inicializace a nacteni textur
        private void VytvorTextury()
        {
            //Nacteni vsech textur
            TexFactory.LoadTexture("Textury\\tex03.jpg", "Floor", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\Wall.bmp", "Wall", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\Wood.bmp", "Wood2", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\0016_wood.jpg", "Wood", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\bila.bmp", "bila", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\cerna.bmp", "cerna", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\bilaf.bmp", "bilaf", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\cernaf.bmp", "cernaf", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\bilad.bmp", "bilad", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\cernad.bmp", "cernad", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\bildel.bmp", "bdelet", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\cerndel.bmp", "cdelet", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\seda.bmp", "seda", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\SedaOznaceno.bmp", "sedaozn", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\SedaOznacenoPolicko.bmp", "sedaoznp", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\SedaOznaceni.bmp", "sedaoznaceni", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
            TexFactory.LoadTexture("Textury\\SedaSmazane.bmp", "SedaSmazane", -1, -1, CONST_TV_COLORKEY.TV_COLORKEY_NO, true);
        }
        //Inicializace a vytvoreni mistnosti
        private void VyvorMistnost()
        {
            //Inicializace mistnosti
            Room = Scene.CreateMeshBuilder("RoomMesh");

            //Nacteni textur
            VytvorTextury();

            //Pridani sten a nastaveni pozice mistnosti
            Room.AddWall3D(Globals.GetTex("Wall"), 150.0f, -350.0f, -350.0f, -350.0f, 350.0f, 5.0f, false, false, -50.0f, 5.0f, 5.0f);
            Room.AddWall3D(Globals.GetTex("Wall"), -350.0f, -350.0f, -350.0f, 150.0f, 350.0f, 5.0f, false, false, -50.0f, 5.0f, 5.0f);
            Room.AddWall3D(Globals.GetTex("Wall"), -350.0f, 150.0f, 150.0f, 150.0f, 350.0f, 5.0f, false, false, -50.0f, 5.0f, 5.0f);
            Room.AddWall3D(Globals.GetTex("Wall"), 150.0f, 150.0f, 150.0f, -350.0f, 350.0f, 5.0f, false, false, -50.0f, 5.0f, 5.0f);
            Room.AddFloor(Globals.GetTex("Floor"), -350.0f, -350f, 150.0f, 150.0f, -50.0f, 20.0f, 20.0f, true);
            Room.AddFloor(Globals.GetTex("Floor"), -350.0f, -350.0f, 150.0f, 150.0f, 300.0f, 10.0f, 10.0f, true);
            Room.SetPosition((float)30.0f, (float)-30.0f, (float)150.0f);

            //Pridani stolku
            Obj1 = new TVMesh();
            Obj1 = Scene.CreateMeshBuilder("Obj1");
            Obj1.LoadXFile("Meshe\\stul.x", true, true);
            Obj1.SetPosition(-57.0f, -80.0f, 38.0f);
            Obj1.SetTexture(Globals.GetTex("Wood"), -1);
            Obj1.SetMeshName("other");

            Obj2 = new TVMesh();
            Obj2 = Scene.CreateMeshBuilder("Obj2");
            Obj2.LoadXFile("Meshe\\okraj.x", true, true);
            Obj2.SetPosition(xc + 17.5f, yc - 1.0f, zc + 17.5f);
            Obj2.SetTexture(Globals.GetTex("Wood2"), -1);
            Obj2.SetMeshName("other");
        }
        //Incializace a vytvoreni sachovnice i s figurkama
        private void VytvorSachovnici(float x, float y, float z)
        {
            //Inicializace sachovnice
            Pole = new Policko[8, 8];

            //Vytvoreni jednotlivych policek a jejich nastaveni
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pole[i, j] = new Policko(i, j);
                    Pole[i, j].mesh = new TVMesh();
                    Pole[i, j].mesh = Scene.CreateMeshBuilder();
                    Pole[i, j].mesh.CreateBox(5, 2, 5, true);
                    Pole[i, j].mesh.SetPosition((float)5 * i + x, y, (float)5 * j + z);
                    Pole[i, j].mesh.SetMeshName(i.ToString() + "P" + j.ToString());
                    if ((((j % 2) == 0) && ((i % 2) == 1)) || (((j % 2) == 1) && ((i % 2) == 0)))
                    {
                        Pole[i, j].barva = "bila";
                        Pole[i, j].hodnota = 10;
                        Pole[i, j].mesh.SetTexture(Globals.GetTex("bila"), -1);
                    }
                    else
                    {
                        Pole[i, j].barva = "seda";
                        Pole[i, j].mesh.SetTexture(Globals.GetTex("seda"), -1);
                    }
                }
            }
        }
        private void NoveFigurky()
        {
            //Pridani bilych figurek do listu
            LFB.Add(new Figurka(0, 0, 1));
            LFB.Add(new Figurka(2, 0, 1));
            LFB.Add(new Figurka(4, 0, 1));
            LFB.Add(new Figurka(6, 0, 1));
            LFB.Add(new Figurka(1, 1, 1));
            LFB.Add(new Figurka(3, 1, 1));
            LFB.Add(new Figurka(5, 1, 1));
            LFB.Add(new Figurka(7, 1, 1));
            LFB.Add(new Figurka(0, 2, 1));
            LFB.Add(new Figurka(2, 2, 1));
            LFB.Add(new Figurka(4, 2, 1));
            LFB.Add(new Figurka(6, 2, 1));

            //Pridani cernych figurek do listu
            LFC.Add(new Figurka(1, 7, 2));
            LFC.Add(new Figurka(3, 7, 2));
            LFC.Add(new Figurka(5, 7, 2));
            LFC.Add(new Figurka(7, 7, 2));
            LFC.Add(new Figurka(0, 6, 2));
            LFC.Add(new Figurka(2, 6, 2));
            LFC.Add(new Figurka(4, 6, 2));
            LFC.Add(new Figurka(6, 6, 2));
            LFC.Add(new Figurka(1, 5, 2));
            LFC.Add(new Figurka(3, 5, 2));
            LFC.Add(new Figurka(5, 5, 2));
            LFC.Add(new Figurka(7, 5, 2));
            VytvorFigurky(xc, yc, zc);
        }
        private void VytvorFigurky(float x, float y, float z)
        {
            //Prirazeni atributu bilym figurkam a vytvoreni meshe
            foreach (Figurka item in LFB)
            {
                item.mesh = new TVMesh();
                item.mesh = Scene.CreateMeshBuilder();
                item.mesh = Cylinder(2, 1, 100, 0, 0, 0, 1, 1);
                item.mesh.SetPosition((float)5 * item.x + x, y + 1.0f, (float)5 * item.y + z);
                item.mesh.SetMeshName(item.x.ToString() + "F" + item.y.ToString());
                if (item.hodnota == 3)
                    item.mesh.SetTexture(Globals.GetTex("bilad"), -1);
                else
                    item.mesh.SetTexture(Globals.GetTex("bilaf"), -1);
                Pole[item.x, item.y].hodnota = item.hodnota;
                Pole[item.x, item.y].fig = item;
            }

            //Prirazeni atributu cernym figurkam a vytvoreni meshe
            foreach (Figurka item in LFC)
            {
                item.mesh = new TVMesh();
                item.mesh = Scene.CreateMeshBuilder();
                item.mesh = Cylinder(2, 1, 100, 0, 0, 0, 1, 1);
                item.mesh.SetPosition((float)5 * item.x + x, y + 1.0f, (float)5 * item.y + z);
                item.mesh.SetMeshName(item.x.ToString() + "F" + item.y.ToString());
                if (item.hodnota == 4)
                    item.mesh.SetTexture(Globals.GetTex("cernad"), -1);
                else
                    item.mesh.SetTexture(Globals.GetTex("cernaf"), -1);
                Pole[item.x, item.y].hodnota = item.hodnota;
                Pole[item.x, item.y].fig = item;
            }
        }
        //Nastaveni kamery do vychozi polohy
        private void NastavKameru()
        {
            //Pocatecni hodnoty kamery
            sngPositionX = -57.0f;
            sngPositionY = 0.0f;
            sngPositionZ = 0.0f;
            snglookatX = -56.5f;
            snglookatY = -25.1f;
            snglookatZ = 38.48f;

            //Pocatecni nastaveni kamery
            t1 = -(float)Math.PI / 2;
            sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
            sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
            Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);
        }
        //Pocatecni nastaveni vsech potrebnych promennych
        private void NastavPromenne()
        {
            //Zvuky
            sTah = new SoundPlayer("Zvuky\\tah.wav");
            sTah.Load();
            sChyba = new SoundPlayer("Zvuky\\Chyba.wav");
            sChyba.Load();
            sKonec = new SoundPlayer("Zvuky\\Konec.wav");
            sKonec.Load();
            sVarovani = new SoundPlayer("Zvuky\\Varovani.wav");
            sVarovani.Load();
            sVitezBily = new SoundPlayer("Zvuky\\VitezBily.wav");
            sVitezBily.Load();
            sVitezCerny = new SoundPlayer("Zvuky\\VitezCerny.wav");
            sVitezCerny.Load();


            //Naplneni pomocnych promennych pro oznaceni
            lastNajete = null;
            lastPoleOZN = null;
            NecoOznaceno = false;
            LzeOdoznacit = true;

            LFB = null;
            LFC = null;

            //Naplneni promennych tahu
            TAH = true;
            Tahnuto = false;
            VTahu = false;
            PoTahu = false;
            Vitez = 0;

            //Inicializace listu pro predvypocitani tahu/skoku/smazani
            LPTah = new List<Policko>();
            LPSkok = new List<Policko>();
            LPSmaz = new List<Policko>();
            LPOznac = new List<Policko>();

            //Nastaveni timeru pro otaceni
            timer2.Enabled = false;
            timer3.Enabled = false;

            LastRadius = Polomer;

            //Inicializace listu figurek
            LFB = new List<Figurka>();
            LFC = new List<Figurka>();
            Pause = true;
            Konec = false;

            label2.Text = "BÍLÝ";
        }

        #endregion

        #region Vykreslovaciji metody

        //Oznacovani a odoznacovani policek ze vstupu
        private void OznacNajete(Policko pom)
        {
            //Pokud je nejake policko uz oznacene najetim, tak ho odoznaci
            if (lastNajete != null && lastNajete != lastPoleOZN && lastNajete.barva == "seda")
            {
                if (lastNajete.oznaceno)
                    lastNajete.mesh.SetTexture(Globals.GetTex("sedaoznaceni"), -1);
                else
                    lastNajete.mesh.SetTexture(Globals.GetTex("seda"), -1);

                if (lastSmazane != null && lastSmazane == lastNajete)
                    lastNajete.mesh.SetTexture(Globals.GetTex("SedaSmazane"), -1);

                lastNajete.najeto = false;
                lastNajete = null;
            }

            //Pokud neni nic najeto, oznaci najete
            if (pom != null && lastPoleOZN != pom && pom.barva == "seda")
            {
                if (lastPoleOZN != null && pom.hodnota == 0)
                {
                    pom.najeto = true;
                    pom.mesh.SetTexture(Globals.GetTex("sedaozn"), -1);
                    lastNajete = pom;
                }
                else if (lastPoleOZN == null)
                {
                    pom.najeto = true;
                    pom.mesh.SetTexture(Globals.GetTex("sedaozn"), -1);
                    lastNajete = pom;
                }
            }
        }
        private void OznacPolicko(Policko pom)
        {
            //Odoznaci kliknute policko
            if (pom != null && pom.oznaceno && pom.barva == "seda")
            {
                pom.mesh.SetTexture(Globals.GetTex("seda"), -1);
                NecoOznaceno = false;
                pom.oznaceno = false;
                lastPoleOZN = null;

                if (LPOznac.Count != 0)
                {
                    foreach (Policko item in LPOznac)
                    {

                        OdOznacTahnutelne(item);
                    }
                    LPOznac.Clear();
                }
            }
            //Oznaci kliknute policko
            else if (!pom.oznaceno && lastPoleOZN == null && pom.barva == "seda")
            {
                pom.oznaceno = true;
                pom.mesh.SetTexture(Globals.GetTex("sedaoznp"), -1);
                NecoOznaceno = true;
                lastPoleOZN = pom;
            }
        }
        private void OznacTahnutelne(Policko p)
        {
            p.oznaceno = true;
            p.mesh.SetTexture(Globals.GetTex("sedaoznaceni"), -1);
        }
        private void OdOznacTahnutelne(Policko p)
        {
            p.oznaceno = false;
            p.mesh.SetTexture(Globals.GetTex("seda"), -1);
        }
        private void OznacSmazane(Policko p)
        {
            p.mesh.SetTexture(Globals.GetTex("SedaSmazane"), -1);
        }
        private void OdOznacSmazane(Policko p)
        {
            p.mesh.SetTexture(Globals.GetTex("seda"), -1);
        }
        private void OznacKeSmazani(Policko p)
        {
            if (p.hodnota == 1 || p.hodnota == 3)
                p.fig.mesh.SetTexture(Globals.GetTex("bdelet"), -1);
            else if (p.hodnota == 2 || p.hodnota == 4)
                p.fig.mesh.SetTexture(Globals.GetTex("cdelet"), -1);
        }
        //Metoda vracejici vytvoreny cylinder
        private TVMesh Cylinder(float Radius, float Height, int Sides, float xm, float ym, float zm, int textureVyskaNasobek, int textureSirkaNasobek)
        {
            TVMesh p = new TVMesh();
            p = Scene.CreateMeshBuilder();
            //Scene.SetRenderMode(CONST_TV_RENDERMODE.TV_LINE);

            double Theta;
            float Inc;
            float x, y, z;
            TV_3DVECTOR n;
            float tu, tv;
            float UStep;
            int sign;

            p.SetPrimitiveType(CONST_TV_PRIMITIVETYPE.TV_TRIANGLESTRIP);
            Inc = (float)((2 * Math.PI) / Sides);
            Theta = 0;
            UStep = (float)(1 / (float)Sides);
            tu = 0;
            sign = -1;

            //Making bottom cap
            for (int i = 1; i <= Sides; i++)
            {
                x = Radius * (float)Math.Cos(sign * Theta);
                y = Height;
                z = Radius * (float)Math.Sin(sign * Theta);

                tu = 0.5f * (float)Math.Cos(sign * Theta) + 0.5f;
                tv = 0.5f * (float)Math.Sin(sign * Theta) + 0.5f;

                p.AddVertex(x - xm, 0 - ym, z - zm, 0, -1, 0, tu, tv);

                if (sign == 1)
                    sign = -1;
                else
                {
                    sign = 1;
                    Theta = Theta + Inc;
                }

            }

            // Making cylinder
            for (int i = 0; i <= Sides; i++)
            {
                x = Radius * (float)Math.Cos(Theta);
                y = Height;
                z = Radius * (float)Math.Sin(Theta);

                n = new TV_3DVECTOR(x, 0, y);
                p.AddVertex(x - xm, 0 - ym, z - zm, n.x, n.y, n.z, 0, 0);
                p.AddVertex(x - xm, y - ym, z - zm, n.x, n.y, n.z, 0, 0);
                Theta = Theta + Inc;

                tu = tu + UStep * textureSirkaNasobek;

            }


            Theta = Math.PI;
            sign = 1;
            //Making top cap
            for (int i = 1; i <= Sides; i++)
            {
                x = Radius * (float)Math.Cos(sign * Theta);
                y = Height;
                z = Radius * (float)Math.Sin(sign * Theta);

                tu = 0.5f * (float)Math.Cos(sign * Theta) + 0.5f;
                tv = 0.5f * (float)Math.Sin(sign * Theta) + 0.5f;

                p.AddVertex(x - xm, y - ym, z - zm, 0, 1, 0, tu, tv);

                if (sign == 1)
                {
                    sign = -1;
                    Theta = Theta + Inc;
                }
                else
                    sign = 1;


            }

            return p;
        }
        //Vypisujici metody chyb a upozorneni a nasledni smazani
        private void Chyba(string s)
        {
            if (Zvuky)
                sChyba.Play();

            label6.Text = "CHYBA:";
            label7.Text = s;
        }
        private void Upozorneni(string s)
        {
            if (Zvuky)
            {
                sVarovani.Play();
                sVarovani.Tag = new object();
            }
            label6.Text = "UPOZORNĚNÍ:";
            label7.Text = s;
        }
        private void SmazChyby()
        {
            label6.Text = "";
            label7.Text = "";
        }
        //Otaci kamerou po tahu po kruznici na stranu soupere
        private void OtocKamerou(bool k)
        {
            //Otaceni kamerou na stranu bileho
            if (k)
            {
                if (!timer2.Enabled)
                    timer2.Enabled = true;
                sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
                sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
                Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);
            }
            //Otaceni kamerou na stranu cerneho
            else
            {
                if (!timer3.Enabled)
                    timer3.Enabled = true;
                sngPositionX = (float)Math.Cos(t1) * Polomer + snglookatX;
                sngPositionZ = (float)Math.Sin(t1) * Polomer + snglookatZ;
                Scene.SetCamera(sngPositionX, sngPositionY + Polomer, sngPositionZ, snglookatX, snglookatY, snglookatZ);
            }
        }

        #endregion

        #region Metody akci

        //Citace pocitajici uhel otoceni pro otaceni kamerou
        private void timer2_Tick(object sender, EventArgs e)
        {
            t1 = t1 + Rychlost / 1000f;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            t1 = t1 - Rychlost / 1000f;
        }
        //Zabiti procesu
        private void TSMIKonec_Click(object sender, EventArgs e)
        {
            bDoLoop = false;
        }
        private void Dama_FormClosing(object sender, FormClosingEventArgs e)
        {
            bDoLoop = false;
        }
        //Inicializace a otevreni okna s nastavenim
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            f2 = new OknoNastaveni();
            f2.Location = new Point(this.Location.X + (this.Size.Width - f2.Size.Height) / 2, this.Location.Y + (this.Size.Height - f2.Size.Width) / 2);
            f2.Show();
        }
        //Metody pro nastavovani velikosti okna na pomer 4:3
        private void Dama_ResizeEnd(object sender, EventArgs e)
        {
            double newvyska = (4 * this.Height) / 3;
            double newsirka = (3 * this.Width) / 4;
            if (sirka != this.Size.Width)
                this.Size = new Size(this.Size.Width, (int)newsirka);
            else if (vyska != this.Size.Height)
                this.Size = new Size((int)newvyska, this.Size.Height);
        }
        private void Dama_ResizeBegin(object sender, EventArgs e)
        {
            sirka = this.Size.Width;
            vyska = this.Size.Height;
        }
        //Akce tlacitka nove hry
        private void TSMINova_Click(object sender, EventArgs e)
        {
            MatFactory.DeleteAllMaterials();
            TexFactory.DeleteAllTextures();
            Room.Destroy();
            Obj1.Destroy();
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pole[i, j].mesh.Destroy();
                }
            }

            Room = null;
            Obj1 = null;
            Pole = null;
            if(LFC != null)
                foreach (Figurka item in LFC)
                {
                    item.mesh.Destroy();
                }
            if (LFB != null)
                foreach (Figurka item in LFB)
                {
                    item.mesh.Destroy();
                }


            LFC.Clear();
            LFB.Clear();
            LPOznac.Clear();
            LPSkok.Clear();
            LPSmaz.Clear();
            LPTah.Clear();
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";

            NastavPromenne();
            VyvorMistnost();
            xc = -75.0f;
            yc = -27.0f;
            zc = 21.0f;
            VytvorSachovnici(xc, yc, zc);
            NoveFigurky();

            NastavKameru();
            
            Pause = false;
        }
        //Pomocne metody pro focusovani okenka nastaveni pokud existuje
        private void Dama_Click(object sender, EventArgs e)
        {
            if (f2 != null)
                if (f2.IsOpened)
                    f2.Focus();
        }
        private void Dama_Click(object sender, MouseEventArgs e)
        {
            if (f2 != null)
                if (f2.IsOpened)
                    f2.Focus();
        }
        private void Dama_Activated(object sender, EventArgs e)
        {
            if (f2 != null)
                if (f2.IsOpened)
                    f2.Focus();
        }

        #endregion

        #region Testovaci sady

        private void Test_Main()
        {
            MatFactory.DeleteAllMaterials();
            TexFactory.DeleteAllTextures();
            Room.Destroy();
            Obj1.Destroy();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pole[i, j].mesh.Destroy();
                }
            }

            Room = null;
            Obj1 = null;
            Pole = null;
            if (LFC != null)
                foreach (Figurka item in LFC)
                {
                    item.mesh.Destroy();
                }
            if (LFB != null)
                foreach (Figurka item in LFB)
                {
                    item.mesh.Destroy();
                }


            LFC.Clear();
            LFB.Clear();
            LPOznac.Clear();
            LPSkok.Clear();
            LPSmaz.Clear();
            LPTah.Clear();
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";

            NastavPromenne();
            VyvorMistnost();
            xc = -75.0f;
            yc = -27.0f;
            zc = 21.0f;
            VytvorSachovnici(xc, yc, zc);

            NastavKameru();

        }
        //Testovani zacykleni pri skakani
        private void Test1_Click(object sender, EventArgs e)
        {
            Test_Main();
            LFB.Add(new Figurka(7, 7, 3));
            LFC.Add(new Figurka(1, 1, 2));
            LFC.Add(new Figurka(3, 1, 2));
            LFC.Add(new Figurka(5, 1, 2));
            LFC.Add(new Figurka(1, 3, 2));
            LFC.Add(new Figurka(3, 3, 2));
            LFC.Add(new Figurka(5, 3, 2));
            LFC.Add(new Figurka(7, 3, 2));
            LFC.Add(new Figurka(1, 5, 2));
            LFC.Add(new Figurka(3, 5, 2));
            LFC.Add(new Figurka(5, 5, 2));
            LFC.Add(new Figurka(7, 5, 2));
            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        //Testovani zmeny figurky na damu
        private void Test2_Click(object sender, EventArgs e)
        {
            Test_Main();
            LFB.Add(new Figurka(6, 6, 1));
            LFC.Add(new Figurka(1, 1, 2));
            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        //Testovani multiple skoku
        private void Test3_Click(object sender, EventArgs e)
        {
            Test_Main();
            LFB.Add(new Figurka(4, 0, 1));
            LFB.Add(new Figurka(0, 0, 1));
            LFB.Add(new Figurka(2, 0, 1));
            LFB.Add(new Figurka(6, 0, 1));
            LFC.Add(new Figurka(1, 1, 2));
            LFC.Add(new Figurka(3, 1, 2));
            LFC.Add(new Figurka(5, 1, 2));
            LFC.Add(new Figurka(1, 3, 2));
            LFC.Add(new Figurka(3, 3, 2));
            LFC.Add(new Figurka(5, 3, 2));
            LFC.Add(new Figurka(7, 3, 2));
            LFC.Add(new Figurka(1, 5, 2));
            LFC.Add(new Figurka(3, 5, 2));
            LFC.Add(new Figurka(5, 5, 2));
            LFC.Add(new Figurka(7, 5, 2));
            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test4_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test5_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test6_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test7_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test8_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test9_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }
        private void Test10_Click(object sender, EventArgs e)
        {
            Test_Main();

            VytvorFigurky(xc, yc, zc);
            Pause = false;
        }

        #endregion

        //Tak a zde se deje vsechno v hlavni smycce
        private void Dama_Load(object sender, EventArgs e)
        {
            //---POMOCNE-----------------------
            text = new TVScreen2DText();
            text2 = new TVScreen2DText();
            //---------------------------------


            VytvorScenu();
            xc = -75.0f;
            yc = -27.0f;
            zc = 21.0f;
            VyvorMistnost();
            VytvorSachovnici(xc, yc, zc);

            NastavKameru();
            NastavPromenne();
            

            bDoLoop = true;
            this.Show();
            this.Focus();

            Scene.SetBackgroundColor(0.5f, 0.5f, 0.7f);

            // Lets setup the Loop:
            while (bDoLoop)
            {
                // Check if the application has focus, if yes thats when we process the loop.

                // Pomocne metody na posun ve scene
                /*heck_Input();
                Check_Movement();*/

                // The actual render loop:
                TV.Clear(false);
                // Render everything
                Scene.RenderAll(true);

                //Zmena tahu, nastaveni promennych
                JeTahnuto();

                if (PoTahu)
                {
                    if (KontrolaKonce2(TAH))
                    {
                        Konec = true;
                    }
                    //Otaci kamerou na stranu protihrace
                    OtocKamerou(TAH);
                    //Zjisti, jestli uz je kamera spravne natocena
                    JeKameraOtocena();
                }
                else if (this.Focused && !Pause && !Konec)
                {
                    Policko pom;

                    //Nastavi uzivatelske promenne
                    ZjistiNastaveni();

                    // Oznaci najete policko
                    pom = GetPolicko();
                    OznacNajete(pom);

                    //Reseni kliku do sachovnice, kdyz je policko oznaceno a kliknuto jinam
                    //Provedeni tahu a pokracovani v nem, jeli to mozne
                    //Vypsani chybovych,upozornujicich hlasek
                    //Zamezeno vzorkovani, kvuli dlouhemu kliknuti tlacitka mysi
                    Input.GetMouseState(ref tmpMouseX, ref tmpMouseY, ref tmpMouseB1, ref tmpMouseB2, ref tmpMouseB3, ref tmpMouseScrollNew);
                    if (tmpMouseB1 && !MouseDOWN && lastPoleOZN != null && lastPoleOZN != pom)
                    {
                        //Zamezeni vzorkovani
                        MouseDOWN = true;

                        //Promazani chyb vypsane mezi tahem a kliknutim na spravne policko tahnuti
                        SmazChyby();
                        pom = GetPolicko();

                        //Rozdeleni tahu na dve faze
                        //Prvni faze je prvni klik po oznaceni na tahnutelne policko
                        // -zde se provede presun
                        // -jeli pri presunu neco skoceno, provede se kontrola dalsiho skoku
                        // -jeli dalsi skok mozny nastavi se tah na druhou fazi
                        //Druha faze je klik po skoceni na dalsi skocitelne policko
                        // -po skoku se provede kontrola dalsiho skoku
                        // -jeli dalsi skok mozny provede se znovu druha faze
                        if (!VTahu)
                        {
                            //Prvni tah
                            if (LPTah.Contains(pom))
                            {
                                //Sloli neco skocit, naplni se SloSkocit tim Polickem
                                SloNecoSkocit();
                                //Odkresleni napovedy tahu
                                ProvedOdOznacovani();
                                
                                //Pokud se skocilo nastavime priznak Smazano
                                if (SloSkocit != null)
                                {
                                    Naplneni_LPSkok(lastPoleOZN);
                                    if (LPSkok.Contains(pom))
                                        Smazano = true;
                                }
                                Presun(ref lastPoleOZN, pom);
                                
                                //Kontrola jestli lze dale skakat jinak konec tahu
                                if (Lze_Skocit(pom.fig) && Smazano)
                                {
                                    VTahu = true;
                                    Smazano = false;
                                    LzeOdoznacit = false;
                                    OznacPolicko(pom);
                                    Naplneni_LPSkok(pom);

                                    //Pokud je zapnuto oznacovani tak se oznaci
                                    ProvedOznacovani(LPSkok, true);
                                }
                                else
                                {
                                    //Pokud necim SloSkocit ale neskocilo se, tak se dana figurka smaze dle pravidel
                                    if (SloSkocit != null && !Smazano)
                                    {
                                        lastSmazane = Pole[SloSkocit.x, SloSkocit.y];
                                        OznacSmazane(lastSmazane);
                                        Smaz(SloSkocit.x, SloSkocit.y);
                                        Upozorneni("Zapoměl jsi skočit, smazal jsem ti figurku!");
                                    }
                                    Tahnuto = true;
                                    JeFigurkaNaKonci(pom);
                                }
                            }
                            else
                                Chyba("Sem nemůžeš táhnout!");
                        }
                        else
                        {
                            //Pokracovani ve skakani
                            if (LPSkok.Contains(pom))
                            {
                                //Odkresleni oznacenych policek ke skoceni
                                ProvedOdOznacovani();

                                Presun(ref lastPoleOZN, pom);

                                if (Lze_Skocit(pom.fig))
                                {
                                    VTahu = true;
                                    Smazano = false;
                                    LzeOdoznacit = false;
                                    OznacPolicko(pom);
                                    Naplneni_LPSkok(pom);

                                    //Pokud je zapnuto oznacovani tak se oznaci
                                    ProvedOznacovani(LPSkok, true);
                                }
                                else
                                {
                                    Tahnuto = true;
                                    JeFigurkaNaKonci(pom);
                                }
                            }
                            else
                                Chyba("Sem nemůžeš táhnout!");
                        }
                    }
                    else if (!tmpMouseB1)
                        MouseDOWN = false;

                    //Reseni kliku do sachovnice, kdyz neni nic oznaceno
                    //Oznaceni figurky k tahnuti, jeli to mozne
                    //Vypsani chybovych,upozornujicich hlasek
                    //Zamezeno vzorkovani, kvuli dlouhemu kliknuti tlacitka mysi
                    pom = GetPolicko();
                    if (tmpMouseB1 && !MouseDOWN && pom != null && pom.hodnota != 0 && LzeOdoznacit)
                    {
                        //Bool pro zamezeni vzorkovani
                        MouseDOWN = true;
                        //Smazani chybovych,upozornujicich hlasek z minuleho tahu, jsouli
                        SmazChyby();
                        //Odoznaceni minuleho automaticky smazaneho policka
                        if (lastSmazane != null)
                        {
                            OdOznacSmazane(lastSmazane);
                            lastSmazane = null;
                        }
                        //Osetreni ze kdyz tahne bily tak pouze bilyma figurkama
                        if ((pom.hodnota == 1 || pom.hodnota == 3) && TAH)
                        {
                            Naplneni_LPTah(pom);
                            if (LPTah.Count != 0)
                                OznacPolicko(pom);
                            else
                                Chyba("S touto figurkou nemůžeš nikam táhnout!");

                            //Pokud je zapnuto oznacovani tak se oznaci vsechna tahnutelna policka
                            ProvedOznacovani(LPTah,pom.oznaceno);
                        }
                        //Osetreni ze kdyz tahne bily tak pouze bilyma figurkama
                        else if ((pom.hodnota == 2 || pom.hodnota == 4) && !TAH)
                        {
                            Naplneni_LPTah(pom);
                            //Oznaceni pripraveni k tahnuti pokud je kam tahnout
                            if (LPTah.Count != 0)
                                OznacPolicko(pom);
                            else
                                Chyba("S touto figurkou nemůžeš nikam táhnout!");

                            //Pokud je zapnuto oznacovani tak se oznaci
                            ProvedOznacovani(LPTah,pom.oznaceno);
                        }
                        //Ostatni kliknute figurky hodi chybu
                        else if (pom.hodnota != 10)
                            Chyba("Toto není tvoje figurka!");
                        else
                            Upozorneni("Kliknutí sem nemá žádný smysl.");

                    }
                    else if (!tmpMouseB1)
                        MouseDOWN = false;
                    
                    //Osetreni upozorneni na policka na kterych nic neni
                    pom = GetPolicko();
                    if (tmpMouseB1 && !MouseDOWN && pom != null)
                    {
                        if (pom.hodnota == 0)
                            Upozorneni("Kliknutí sem nemá žádný smysl.");
                        MouseDOWN = true;
                    }
                    else if (!tmpMouseB1)
                        MouseDOWN = false;




                   // text.NormalFont_DrawText(this.Size.ToString() + "Tah: " + TAH.ToString() + " LFB Count: " + LFB.Count.ToString() + " LFC Count: " + LFC.Count.ToString(), 50.0f, 40.0f, -16710933);


                    //Pomocny vypis
                    //VypisText();

                    //POMOCNY TEXT NA NASTAVOVANI KAMERY
                    //text2.NormalFont_DrawText("SLx " + snglookatX + " " + "SLy " + snglookatY + " " + "SLz " + snglookatZ + " " + "SPx " + sngPositionX + " " + "SPy " + sngPositionY + " " + "SPz " + sngPositionZ + " " + "SAx " + sngAngleX + " " + "SAy " + sngAngleY, 50.0f, 100.0f, -16710933);

                    //------------------------------------
                }
                
                TV.RenderToScreen();

                //Zpomaleni vykreslovani, pokud je okno neaktivni
                if (!this.Focused && f2 == null)
                    System.Threading.Thread.Sleep(200);
                else if( f2 != null)
                    if (!this.Focused && !f2.IsOpened)
                        System.Threading.Thread.Sleep(200);


                Application.DoEvents();
            }
                this.Close();
        }

    }
}