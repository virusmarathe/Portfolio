//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : Item.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PsychicNinja.Data.Util;
using PsychicNinja.Interface.Engine;
using System;
using PsychicNinja.Logic;

namespace PsychicNinja.Data.Object
{

    public class Item : WorldObject
    {
        public ItemType type;
        public bool isFired;
        public bool isUsed;
        public bool isHeld;
        private Vector2 directionThrown;
        const float FIRE_SPEED = 20.0f;

        private static Texture2D shurikenTexture;
        private static Texture2D katanaTexture;

        public static double radian = System.Math.PI / 180;
        //private static Texture2D hookshotTexture;

        private const int itemDimension = 40;

        #region Life Cycle

        /// <summary>
        /// Create a new Item.
        /// </summary>
        /// <param name="topLeft">Desired top left corner of item.</param>
        /// <param name="Type">Desired item type of item.</param>
        public Item(Point topLeft, ItemType Type) : base(new Rectangle(topLeft.X, topLeft.Y, itemDimension, itemDimension), (Type == ItemType.Sword) ? katanaTexture : shurikenTexture)
        {
           type = Type;
           isFired = false;
           isUsed = false;
           isHeld = false;
           directionThrown = new Vector2(1, 0);
           drawStyle = WorldObjectDrawStyle.Rotating;
        }

        #endregion

        #region Game Cycle

        public override void Update(int timeElapsed)
        {
            if (timeElapsed == lastUpdated) return;

            if (isFired)
            {
                if (type == ItemType.Shuriken)
                    rotationAngle += (float)(25*radian);
                WorldObjectMove((int)(FIRE_SPEED * directionThrown.X), (int)(FIRE_SPEED * directionThrown.Y));
                isUsed = true;
            }
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isHeld)
            {
                base.Draw(spriteBatch);
            }
        }

        public static new void LoadContent(ContentManager Content)
        {
            shurikenTexture = Content.Load<Texture2D>("shuriken");
            katanaTexture = Content.Load<Texture2D>("SWORDEYCON");

            //hookshotTexture = Content.Load<Texture2D>("grapple");
        }

        #endregion

        #region Accessors

        public void SetDirection(Vector2 v)
        {
            Vector2 norm = Vector2.Normalize(v);
            directionThrown = norm;
            if(type == ItemType.Sword)
                rotationAngle = (float)(Math.Atan2(directionThrown.Y, directionThrown.X)+(90*radian));
        }

        public void SetDirection(Point p)
        {
            SetDirection(new Vector2(p.X, p.Y));
        }

        public bool isOnScreen(Rectangle r)
        {
            // item screen check
            if (r.X > -(r.Width) && r.X < 1000 && r.Y > -(r.Height) && r.Y < 480) return true;
            return false;
        }

        public void platformCheck(Platform p)
        {
            isFired = false;
        }

        public void remove()
        {

        }

        public void enemyCheck(Enemy e)
        {
            if (e.IsAlive())
            {
                MusicManager.PlaySoundEffect(SoundEffects.deathsound2);
                e.death();
            }
            /*switch (type)
            {
                case ItemType.Sword:
                    e.death();
                    break;
                case ItemType.Shuriken:
                    e.death();
                    break;
            }*/
        }

        #endregion
    }

}
