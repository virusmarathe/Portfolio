using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PsychicNinja.Interface.Engine;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.Object.Entity
{
    class Particle
    {
        Vector2 velocity;
        Vector2 position; //needed for more precision, considering some particles may be moving slower than 1 pixel per frame

        Vector2 Vdelta; //additional form of acceleration acting on particle
        Vector2 Videal; //threshhold for vdelta acceleration; for example, terminal velocity with gravity

        private Rectangle drawRect;

        Texture2D tex;


        private int startingLifespan;
        public int lifespanRemaining; //counts down from starting value until it hits 0, at which point this particle is considered "dead"
        double rotation = 0;
        double rotationDelta;

        private bool drawingInScreenCoordinates = false;

        /// <summary>
        /// if fades is true, the particle will fade as it approaches the end of its lifespan
        /// </summary>
        public bool fades = false;


        /// <summary>
        /// Constructs a new particle.
        /// </summary>
        /// <param name="bounds">the starting region of the particle</param>
        /// <param name="texture">texture to use for this particle</param>
        /// <param name="RotationRadiansPerUpdate">amount to rotate this particle by per frame</param>
        /// <param name="Velocity">movement speed of this particle</param>
        /// <param name="LifespanInFrames">number of frames to draw this particle.  Use -1 to draw the particle forever</param>
        /// <param name="drawInScreenCoords">whether or not this particle should draw in screen coordinates</param>
        public Particle(Rectangle bounds, Texture2D texture, double RotationRadiansPerUpdate, Vector2 Velocity, int LifespanInFrames, bool drawInScreenCoords)
        {
            velocity = Velocity;
            position = new Vector2((float)bounds.X, (float)bounds.Y);
            drawRect = bounds;

            this.Vdelta = Vector2.Zero;

            tex = texture;

            startingLifespan = lifespanRemaining = LifespanInFrames;
            rotationDelta = RotationRadiansPerUpdate;

            drawingInScreenCoordinates = drawInScreenCoords;
        }

        /// <summary>
        /// Constructs a new particle.
        /// </summary>
        /// <param name="bounds">the starting region of the particle</param>
        /// <param name="texture">texture to use for this particle</param>
        /// <param name="RotationRadiansPerUpdate">amount to rotate this particle by per frame</param>
        /// <param name="Velocity">movement speed of this particle</param>
        /// <param name="Vdelta">acceleration of movement speed; can be wind, gravity, etc depending on particle</param>
        /// <param name="Videal">cutoff for Vdelta effect; a value of 0 in this vector will cause that value to be ignored in the cutoff check</param>
        /// <param name="LifespanInFrames">number of frames to draw this particle.  Use -1 to draw the particle forever</param>
        /// <param name="drawInScreenCoords">whether or not this particle should draw in screen coordinates</param>
        public Particle(Rectangle bounds, Texture2D texture, double RotationRadiansPerUpdate, Vector2 Velocity, Vector2 Vdelta, Vector2 Videal, int LifespanInFrames, bool drawInScreenCoords)
        {
            velocity = Velocity;
            position = new Vector2((float)bounds.X, (float)bounds.Y);
            drawRect = bounds;

            this.Vdelta = Vdelta;
            this.Videal = Videal;

            tex = texture;

            startingLifespan = lifespanRemaining = LifespanInFrames;
            rotationDelta = RotationRadiansPerUpdate;

            drawingInScreenCoordinates = drawInScreenCoords;
        }

        public void Update()
        {
            //only update if this particle still has some lifespan frames
            if (lifespanRemaining > 0 || lifespanRemaining < 0)
            {
                //rotate the particle if rotation speed was specified
                if (rotationDelta != 0)
                {
                    rotation += rotationDelta;
                    rotation = rotation % (2 * Math.PI);
                }

                //move the particle
                position += velocity;

                //change velocity with vdelta, cutting off at videal
                if (Vdelta != Vector2.Zero)
                {
                    if (Videal.X != 0 && velocity.X != Videal.X)
                    {
                        velocity.X += Vdelta.X;
                        if((Videal.X < 0 && velocity.X < Videal.X) || (Videal.X > 0 && velocity.X > Videal.X))
                            velocity.X = Videal.X;
                    }

                    if (Videal.Y != 0 && velocity.Y != Videal.Y)
                    {
                        velocity.Y += Vdelta.Y;
                        if ((Videal.Y < 0 && velocity.Y < Videal.Y) || (Videal.Y > 0 && velocity.Y > Videal.Y))
                            velocity.Y = Videal.Y;
                    }
                }


                //update the draw rectangle
                drawRect.X = (int)position.X;
                drawRect.Y = (int)position.Y;

                //decrement lifespan frames at the end
                lifespanRemaining--;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

                Color c;
                if (fades)
                    c = new Color(((float)lifespanRemaining / startingLifespan), ((float)lifespanRemaining / startingLifespan), ((float)lifespanRemaining / startingLifespan), ((float)lifespanRemaining / startingLifespan));
                else
                    c = Color.White;


                if (drawingInScreenCoordinates)
                    spriteBatch.Draw(tex, drawRect, null, c, (float)rotation, new Vector2(tex.Bounds.Center.X, tex.Bounds.Center.Y), SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(tex, ModifiedDrawRect(), null, c, (float)rotation, new Vector2(tex.Bounds.Center.X, tex.Bounds.Center.Y), SpriteEffects.None, 0);
            
        }

        public Rectangle ModifiedDrawRect()
        {
            return new Rectangle(drawRect.Left - (int)Spotlight.offset.X, drawRect.Top - (int)Spotlight.offset.Y, drawRect.Width, drawRect.Height);
        }
    }
}
