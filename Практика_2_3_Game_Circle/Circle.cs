using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика_2_3_Game_Circle
{
    public class Circle
    {
        private int x, y;
        private int radius;
        public Color color { get; set; }
        private int _dx, _dy;

        public int dx { get { return this._dx; } set { _dx = value; } }
        public int dy { get { return this._dy; } set { _dy = value; } }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        public Circle(int x, int y, int r = 20)
        {
            this.x = x;
            this.y = y;
            this.radius = r;
        }
        public void Move(int dx, int dy)
        {
            x += dx;
            y += dy;
        }

    }
}
