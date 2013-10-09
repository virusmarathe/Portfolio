using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace PsychicNinja.Interface.Engine
{
    class Cutscene : View 
    {
        Vector2 offset = Vector2.Zero;
        Vector2 motionTarget;
        Button continueButton;

        

        public Cutscene(string texname) : base(new Rectangle(0, 0, 800, 480), Content.Load<Texture2D>(texname))
        {
            continueButton = new Button(new Rectangle(370, 400, 60, 60), Content.Load<Texture2D>("eyeopen"));
            motionTarget = Vector2.Zero;
        }

        public override void Update()
        {
            if (motionTarget != Vector2.Zero)
            {
                offset = Vector2.Lerp(offset, motionTarget, 0.3f);
                if (Math.Abs(offset.X - motionTarget.X) < 5 && Math.Abs(offset.Y - motionTarget.Y) < 5)
                    motionTarget = Vector2.Zero;
            }
            EnforceBoundaries();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(drawTex, Vector2.Zero, new Rectangle(drawRect.Left + (int)offset.X, drawRect.Top + (int)offset.Y, drawTex.Width, drawTex.Height), Color.White);

            continueButton.Draw(spriteBatch);
        }

        /// <summary>
        /// Process a touch gesture and return an indicator of whether or not the user has advanced past this cutscene.
        /// This is abberant behavior from our usual application paradigm because a cutscene it treated as a special case. 
        /// </summary>
        /// <param name="gesture"></param>
        /// <returns>true if the user has touched the Continue button. False otherwise. </returns>
        public override bool ProcessTouch(GestureSample gesture)
        {
            if (continueButton.ProcessTouch(gesture))
            {
                return true;
            }

            switch (gesture.GestureType)
            {
                case GestureType.FreeDrag:
                    offset -= gesture.Delta;
                    motionTarget = Vector2.Zero;
                    EnforceBoundaries();
                    return false;
                case GestureType.Flick:
                    Vector2 normal = gesture.Delta;
                    normal.Normalize();
                    motionTarget = offset - (normal * 200);
                    return false;
                case GestureType.Tap:
                    motionTarget = Vector2.Zero;
                    return false;
                default:
                    return false; 

            }
        }

        private void EnforceBoundaries()
        {
            if (offset.X < 0) offset.X = 0;
            if (offset.Y < 0) offset.Y = 0;

            int horizBound = drawTex.Width - 800;
            int vertBound = drawTex.Height - 480;

            if (offset.X > horizBound) offset.X = horizBound;
            if (offset.Y > vertBound) offset.Y = vertBound;
        }
    }
}
