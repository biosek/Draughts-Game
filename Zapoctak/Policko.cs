using System;
using System.Collections.Generic;
using System.Text;
using MTV3D65;

namespace Zapoctak
{
    public class Policko
    {
        public int hodnota;
        public int x;
        public int y;
        public TVMesh mesh;
        public string barva;
        public bool skoceno;
        public bool oznaceno;
        public bool najeto;
        public Figurka fig;

        public Policko(int x, int y)
        {
            this.x = x;
            this.y = y;
            oznaceno = false;
            najeto = false;
            skoceno = false;
            hodnota = 0;
            fig = null;
        }

        
    }
}
