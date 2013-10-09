using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xunit;
using Microsoft.Xna.Framework;

using PsychicNinja.Data.Object;
using PsychicNinja.Data.Patrol;


using Microsoft.Xna.Framework.Graphics;
using PsychicNinja.Data.Util;

namespace PsychicNinjaUnitTest
{
    public class PsychicNinjaUnitTests
    {
        [Fact]
        public void WorldObject_DefaultInitTest()
        {
            WorldObject obj = new WorldObject(new Rectangle(0, 0, 90, 90), "texturemissing");

            Assert.Equal(obj.velocity, Vector2.Zero);

        }

        [Fact]
        public void PatrolModel_CurrentVelocityTest()
        {
            PatrolModel p = new PatrolModel();

            Vector2 a = new Vector2(2.0f, 5.0f);
            Vector2 b = new Vector2(3.0f, -7.0f);

            p.addVector(a, 5);
            p.addVector(b, 12);

            Assert.Equal(p.getCurrentVector(3).Equals(a), true); // Make sure it picks the first item
            Assert.Equal(p.getCurrentVector(9).Equals(b), true); // Make sure it picks the second item
            Assert.Equal(p.getCurrentVector(20).Equals(a), true); // Make sure it picks the first item again
        }

        [Fact]
        public void AnimationComponentUpdateTest()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(null);
            textures.Add(null);
            AnimationComponent comp = new AnimationComponent(textures, 50, 50, 50, 50, 1);

            comp.Update(1);

            Assert.Equal(comp.animationComplete(), true);

        }

        
    }
}
