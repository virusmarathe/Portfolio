using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Microsoft.Xna.Framework;
using PsychicNinja.Data.Object;
using PsychicNinja.Data.Util;

namespace PsychicNinjaUnitTest
{
    public class CommandTests
    {

        [Fact]
        public void CommandConnectionTest()
        {
            Command c = new Command(new Point(35, 75), PsychicNinja.Data.Util.CommandType.LedgeClimb);
            Platform p = new Platform(new Rectangle(35, 75, 40, 80));

            c.ConnectToPlatform(p);

            Assert.Equal(true, c.ConnectedPlatforms.Count > 0);
        }

        [Fact]
        public void CommandCollisionTest()
        {
            Command c = new Command(new Point(35, 75), PsychicNinja.Data.Util.CommandType.LedgeClimb);
            Platform p = new Platform(new Rectangle(35, 75, 40, 80));

            c.ConnectToPlatform(p);

            Assert.Equal(true, c.CollidesWithConnectedPlatforms(new Platform(new Rectangle(70, 65, 40, 80))) != null);
        }

        [Fact]
        public void CommandValidityTest()
        {
            Command c = new Command(new Point(35, 75), PsychicNinja.Data.Util.CommandType.Jump);
            Platform p = new Platform(new Rectangle(35, 75, 40, 80));

            c.ConnectToPlatform(p);

            Assert.Equal(false, c.ConnectedPlatforms.Count > 0);
        }
    }
}
