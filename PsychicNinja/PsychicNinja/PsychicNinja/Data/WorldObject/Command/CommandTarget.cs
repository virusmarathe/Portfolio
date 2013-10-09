using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace PsychicNinja.Data.WorldObject
{
    public class CommandTarget : WorldObject
    {
        const int size = 20;
        const int halfSize = 10;

        private static Texture2D CommandTargetTexture;

        public CommandTarget(Vector2 newCenter)
            : base(new Rectangle((int)newCenter.X - halfSize, (int)newCenter.Y - halfSize, size, size), CommandTargetTexture)
        {

        }

        public CommandTarget(Point newCenter)
            : base(new Rectangle(newCenter.X - halfSize, newCenter.Y - halfSize, size, size), CommandTargetTexture)
        {

        }

        public static new void LoadContent(ContentManager Content)
        {
            CommandTargetTexture = Content.Load<Texture2D>("ninjastar");
        }

        public new bool ProcessTouch(GestureSample gesture)
        {
            if (gesture.GestureType != GestureType.FreeDrag) return false;
            if (hidden || !Contains(gesture.Position)) return false;

            SetCenter(gesture.Position);

            return true;

        }
    }
}
