using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor
{
    public class PatrolPoint
    {
        public int x, y, time;
        public PatrolPoint(int px, int py, int ptime)
        {
            x = px;
            y = py;
            time = ptime;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public int getTime()
        {
            return time;
        }

    }
}
