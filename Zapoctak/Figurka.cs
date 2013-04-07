using System;
using System.Collections.Generic;
using System.Text;
using MTV3D65;

namespace Zapoctak
{
    public class Figurka
    {
        public int hodnota;
        public int x;
        public int y;
        public TVMesh mesh;

        public Figurka(int x, int y, int hodnota)
        {
            this.x = x;
            this.y = y;
            this.hodnota = hodnota;
        }

    }
}
