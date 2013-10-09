using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.Object
{
    public class CommandTarget : WorldObject
    {
        const int hitSize = 100;
        const int halfHitSize = hitSize / 2;
        const int drawSize = 50;
        const int halfDrawSize = drawSize/2;

        private static Texture2D CommandTargetTexture;

        public CommandTarget(Vector2 newCenter)
            : base(new Rectangle((int)newCenter.X - halfDrawSize, (int)newCenter.Y - halfDrawSize, drawSize, drawSize), CommandTargetTexture)
        {
            collisionRect = new Rectangle((int)(newCenter.X - halfHitSize), (int)(newCenter.Y - halfHitSize), hitSize, hitSize);
        }

        public CommandTarget(Point newCenter)
            : base(new Rectangle(newCenter.X - halfDrawSize, newCenter.Y - halfDrawSize, drawSize, drawSize), CommandTargetTexture)
        {
            collisionRect = new Rectangle((int)(newCenter.X - halfHitSize), (int)(newCenter.Y - halfHitSize), hitSize, hitSize);
        }

        public static new void LoadContent(ContentManager Content)
        {
            CommandTargetTexture = Content.Load<Texture2D>("crosshair");
        }

        public override void SetCenter(Point p)
        {
            base.SetCenter(p);
            collisionRect.X = p.X - halfHitSize;
            collisionRect.Y = p.Y - halfHitSize;
        }

        public new bool ProcessTouch(GestureSample gesture)
        {
            Point localGesturePoint = Spotlight.TranslateScreenVectorToWorldPoint(gesture.Position);

            if (!CanRespond()) return false;

            if (RespondsToWorldTouch(localGesturePoint) || IsFirstResponder())
            {

                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        SetCenter(localGesturePoint);
                        BecomeFirstResponder();
                        break;
                    case GestureType.DragComplete:
                        ResignFirstResponder();
                        break;
                    default:
                        return false;
                }
                return true;
            }

            return false;

        }
    }
}
