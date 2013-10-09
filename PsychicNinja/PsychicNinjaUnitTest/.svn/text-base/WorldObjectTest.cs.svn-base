using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Microsoft.Xna.Framework;

using PsychicNinja.Data.WorldObject;

namespace PsychicNinjaUnitTest
{
    public class WorldObjectTest
    {
        [Fact]
        public void WorldObject_DefaultInitTest()
        {
            WorldObject obj = new WorldObject();

            Assert.Equal(obj.velocity, Vector2.Zero);

        }

        [Fact]
        public void WorldObject_VelocityTest()
        {
            WorldObject obj = new WorldObject();

            obj.setCenter(Point.Zero);
            obj.setVelocity(5.0f, 5.0f);

            obj.Update();

            Assert.NotEqual(obj.Position(), Vector2.Zero);

        }
    }
}
