using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PsychicNinja.Data.Util
{
    public class Util
    {

        public static int Min(int a, int b)
        {
            return (a < b) ? a : b;
        }

        public static int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public static Rectangle ResizeRectangleKeepingCenter(Rectangle rect, int width, int height)
        {
            Point c = rect.Center;
            return new Rectangle(c.X - width / 2, c.Y - height / 2, width, height);
        }
    }
}
