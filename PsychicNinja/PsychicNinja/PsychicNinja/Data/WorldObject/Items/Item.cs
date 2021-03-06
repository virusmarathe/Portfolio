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

namespace PsychicNinja.Data.WorldObject
{

    public class Item : WorldObject
    {
        private ItemType type;
        public bool isFired;
        public bool isUsed;
        private Vector2 directionThrown;
        const float FIRE_SPEED = 20.0f;

        private static Texture2D shurikenTexture;
        private static Texture2D katanaTexture;
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
            directionThrown = new Vector2(1, 0);
        }

        #endregion

        #region Game Cycle

        public override void Update(int timeElapsed)
        {
            if (timeElapsed == lastUpdated) return;

            if (isFired)
            {
                WorldObjectMove((int)(FIRE_SPEED * directionThrown.X), (int)(FIRE_SPEED * directionThrown.Y));
                isUsed = true;
            }
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public static new void LoadContent(ContentManager Content)
        {
            shurikenTexture = Content.Load<Texture2D>("ninjastar");
            katanaTexture = Content.Load<Texture2D>("S-WORD");

            //hookshotTexture = Content.Load<Texture2D>("grapple");
        }

        #endregion

        #region Accessors

        public void SetDirection(Vector2 v)
        {
            Vector2 norm = Vector2.Normalize(v);
            directionThrown = norm;
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
            if (type == ItemType.Sword)
            {
                e.death();
            }
            else if (type == ItemType.Shuriken)
            {
                e.setVelocity(0, 0);              
            }
        }

        public ItemType getItemType()
        {
            return type;
        }

        #endregion
    }

}
