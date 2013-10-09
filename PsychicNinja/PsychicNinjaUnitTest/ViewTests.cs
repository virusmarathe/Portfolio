using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using Microsoft.Xna.Framework;
using PsychicNinja.Interface.Engine;
using PsychicNinja.Data.Util;

namespace PsychicNinjaUnitTest
{
    public class ViewTests
    {
        [Fact]
        public void ViewConsistencyTest()
        {
            View a = new View(new Rectangle(30, 50, 50, 50));

            Assert.Equal(true, a.Center() == new Point(55, 75));
        }

        [Fact]
        public void ViewHitTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));
            Assert.Equal(true, a.Contains(new Point(55, 90)));
        }

        [Fact]
        public void ViewMissTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));
            Assert.Equal(false, a.Contains(new Point(85, 90)));
        }

        [Fact]
        public void ViewIntersectTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));
            View b = new View(new Rectangle(60, 90, 50, 50));

            Assert.Equal(true, a.Contains(b.drawRect));
        }

        [Fact]
        public void ViewNonIntersectTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));
            View b = new View(new Rectangle(90, 90, 50, 50));

            Assert.Equal(false, a.Contains(b.drawRect));

        }

        [Fact]
        public void ViewInflateTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));

            a.ResizeFrame(30, 40);

            Assert.Equal(true, a.drawRect.Width == 80);
        }

        [Fact]
        public void ViewMoveTest()
        {
            View a = new View(new Rectangle(30, 60, 50, 50));

            a.MoveFrame(30, 40);

            Assert.Equal(true, a.drawRect.Left == 60);
        }
    }
}
