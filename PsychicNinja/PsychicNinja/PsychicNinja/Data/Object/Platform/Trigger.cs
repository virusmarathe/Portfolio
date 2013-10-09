using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PsychicNinja.Data.Util;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PsychicNinja.Data.Object
{
    public class Trigger : WorldObject
    {
        public Platform attachedTo;
        public Trigger(Rectangle rect, Platform p) :
            base(rect)
        {
            attachedTo = p;
        }

        public override void Update(int timeElapsed)
        {
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public static new void LoadContent(ContentManager Content) 
        {

        }
    }
}
