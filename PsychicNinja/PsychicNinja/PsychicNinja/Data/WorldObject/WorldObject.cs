//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : WorldObject.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using PsychicNinja.Interface.Engine;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.WorldObject
{

    public class WorldObject : View
    {
        public const float gravity = 1;
        public const float terminalVelocity = 30;

        protected Rectangle collisionRect;

        public Vector2 velocity;
        public bool facingLeft = false;

        protected int lastUpdated;
        public bool isTiled = false;
        public float rotationAngle;
        public PositionState positionMask;
        public ObjectShape shape;

        public bool hasGravity;
        protected bool screenScrolls = true;

        public WorldObject floor;

        #region Init

        private void defaultInit()
        {
            rotationAngle = 0.0f;
            lastUpdated = 0;
            positionMask = PositionState.NotSet;
            velocity = Vector2.Zero;

            collisionRect = DrawFrame();
        }

        /// <summary>
        /// UI constructor. 
        /// </summary>
        /// <param name="rect">Rectangle in which to draw this object.</param>
        /// <param name="tex">Texture with which to draw this object.</param>
        public WorldObject(Rectangle rect, Texture2D tex) : base(rect, tex)
        {
            defaultInit();
        }

        /// <summary>
        /// Game World Constructor.
        /// </summary>
        /// <param name="rect">Rectangle in which to draw this object.</param>
        /// <param name="vel">Velocity this object initially moves with.</param>
        /// <param name="tex">Texture with which to draw this object.</param>
        /// <param name="shape">Shape of this object for collision purposes.</param>
        public WorldObject(Rectangle rect, Texture2D tex, ObjectShape newShape) : base(rect, tex)
        {
            defaultInit();
            shape = newShape;
        }

        #endregion

        #region Life Cycle

        /// <summary>
        /// Default Update for WorldObject and subclasses. Moves this object by its velocity.
        /// </summary>
        /// <param name="timeElapsed">Amount of game time in seconds. May be ignored.</param>
        public virtual void Update(int timeElapsed)
        {
            if (timeElapsed == lastUpdated) return; // Don't update if time isnt flowing
            {
                Vector2 newVel = velocity;
                if (hasGravity)
                    newVel.Y += gravity;
                if (newVel.Y > terminalVelocity)
                    newVel.Y = terminalVelocity;
                velocity = newVel;

                int xoff = (int)newVel.X;
                int yoff = (int)newVel.Y;

                MoveFrame(xoff, yoff);
                lastUpdated = timeElapsed;
            }
        }

        /// <summary>
        /// Default Draw for WorldObject. Draws the object.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the object with.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            Draw(spriteBatch, color, SpriteEffects.None);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, SpriteEffects effects)
        {
            int tileSize = (int) (20.0 * Spotlight.getScale());
            Rectangle modDrawRect = modifiedDrawRect();
            
            if (isTiled)
            {
                for (int i = 0; i < modDrawRect.Width / (tileSize); i++)
                {
                    for (int j = 0; j < modDrawRect.Height / (tileSize); j++)
                    {
                        spriteBatch.Draw(drawTex, new Rectangle(modDrawRect.X + i * tileSize, modDrawRect.Y + j * tileSize, tileSize, tileSize), null, color, 0, Vector2.Zero, effects, 0);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(drawTex, modifiedDrawRect(), null, color, 0, Vector2.Zero, effects, 0);
            }
        }

        #endregion

        #region Accessors

        #region Getters

        public Rectangle modifiedDrawRect()
        {
            if (screenScrolls == false) { return DrawFrame(); }
            Vector2 temp = Spotlight.getOffset();
            double scale = Spotlight.getScale();
            double rectX = ((GetDrawFrameX() - temp.X - 400) * scale) + 400; // adjusts for the offset 
            double rectY = ((GetDrawFrameY() - temp.Y - 240) * scale) + 240; // and zoom rate
            return new Rectangle((int)rectX, (int)rectY, (int)(GetDrawFrameWidth() * scale), (int)(GetDrawFrameHeight() * scale));
        }

        public PositionState getPositionState()
        {
            return positionMask;
        }

        public Vector2 getVelocity()
        {
            return velocity;
        }

        public bool Gravity()
        {
            return hasGravity;
        }

        public Rectangle CollisionFrame()
        {
            return collisionRect;
        }

        public Vector2 Position()
        {
            return new Vector2(GetDrawFrameX(), GetDrawFrameY());
        }

        #endregion

        #region Setters

        public void setVelocity(float x, float y)
        {
            velocity = new Vector2(x, y);
        }

        public void setPositionState(PositionState state)
        {
            positionMask = state;
        }

        public void setGravity(bool falls)
        {
            hasGravity = falls;
        }

        #endregion

        #region Modifiers

        /// <summary>
        /// Move only the collision rectangle for this object. Generally shouldn't be called.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveCollisionFrame(int x, int y)
        {
            collisionRect.Offset(x, y);
        }

        /// <summary>
        /// Move this entire object, draw and collision frames. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void WorldObjectMove(int x, int y)
        {
            MoveFrame(x, y);
            MoveCollisionFrame(x, y);
        }

        #endregion

        #endregion


    }

}