using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.Object
{

    public class SuspendRope : WorldObject
    {
        private bool isHazardous;

        //Platform that the rope holds up
        //private Platform parent;

        private static Texture2D ropeTexture;

        public static SuspendRope droppableDevRope(Rectangle rect, Vector2 vel, bool gravity)
        {
            return new SuspendRope(rect, ropeTexture, ObjectShape.Rectangle, false, gravity);
        }

        public SuspendRope(Rectangle rect, Texture2D tex, ObjectShape Shape, bool IsHazardous, bool gravity) :
            base(rect, tex, Shape)
        {
            isHazardous = IsHazardous;
            hasGravity = gravity;
        }

        public override void Update(int timeElapsed)
        {

            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
  //          spriteBatch.Draw(drawTex, drawRect, drawRect, Color.White, rotationAngle, new Vector2(0, 0), SpriteEffects.None, 0.5f);
            base.Draw(spriteBatch);
        }

        public static new void LoadContent(ContentManager Content)
        {
            ropeTexture = Content.Load<Texture2D>("Interactive/hanging rope");
        }
    }
}
