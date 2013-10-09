using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.Object
{

    public class Rope : WorldObject
    {
        private bool isHazardous;
        private bool hasNinja;
        private bool isReleasing;
        private int direction = 1;
        const float MAX_ANGLE = 1.0f;
        const float ROPE_SPEED = 0.05f;

        private static Texture2D ropeTexture;

        public static Rope droppableDevRope(Rectangle rect, Vector2 vel, bool gravity)
        {
            return new Rope(rect, ropeTexture, ObjectShape.Rectangle, false, gravity);
        }

        public Rope(Rectangle rect, Texture2D tex, ObjectShape Shape, bool IsHazardous, bool gravity) :
            base(rect, tex, Shape)
        {
            isHazardous = IsHazardous;
            hasGravity = gravity;
            hasNinja = false;
            isReleasing = false;
        }

        public override void Update(int timeElapsed)
        {
            if (hasNinja)
            {
                swingRope(timeElapsed);
            }
            else
            {
                rotationAngle = 0;
            }
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(drawTex, drawRect, null, Color.White, rotationAngle, new Vector2(0, 0), SpriteEffects.None, 0.5f);
        }

        public static new void LoadContent(ContentManager Content)
        {
            ropeTexture = Content.Load<Texture2D>("Interactive/hanging rope");
        }

        public void setHasNinja(Boolean b)
        {
            hasNinja = b;
        }

        public float getRotationAngle()
        {
            return rotationAngle;
        }

        public void setRotationAngle(float f)
        {
            rotationAngle = f;
        }

        private void swingRope(int timeElapsed)
        {
            rotationAngle = rotationAngle + direction * ROPE_SPEED;
            if (rotationAngle >= MAX_ANGLE || rotationAngle <= MAX_ANGLE * -1)
            {
                direction = direction * -1;
            }
        }

        public void ropeRelease(Boolean r)
        {
            isReleasing = true;
            // animation stuff in here
        }

        public void resolveCollisionWithObject(int type, Player p)
        {
            if (type != 0 && !isReleasing)
            {
                hasNinja = true;
                p.Action_RopeSwing(this);
            }
            else
            {
                hasNinja = false;
            }
        }


    }
}
