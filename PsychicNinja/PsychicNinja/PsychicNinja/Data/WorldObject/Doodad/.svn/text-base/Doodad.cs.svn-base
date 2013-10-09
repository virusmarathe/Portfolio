using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.WorldObject
{
    public class Doodad : WorldObject
    {

        private static Texture2D[] textureArray;

        public Doodad(Rectangle rect, int textureIndex, ObjectShape Shape) :
            base(rect, textureArray[textureIndex], Shape)
        {         
        }
        public static Doodad droppableDevDoodad(int textureIndex, Rectangle rect, bool gravity)
        {
            return new Doodad(rect, textureIndex, ObjectShape.Rectangle);
        }
        public override void Update(int timeElapsed)
        {
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public static void LoadContent(ContentManager Content, string[] textureNames)
        {
            textureArray = new Texture2D[textureNames.Length];

            for (int i = 0; i < textureNames.Length; ++i)
            {
                textureArray[i] = Content.Load<Texture2D>("NonInteractive/" + textureNames[i]);
            }
        }
    }
}
