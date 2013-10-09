using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PsychicNinja.Data.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PsychicNinja.Data.Object;

namespace PsychicNinja.Interface.Engine
{
    class Layer : View
    {
        int spotlightX, spotlightY;
        float spotlightScale;
        /// <summary>
        /// top left corner
        /// </summary>
        double bgX, bgY;
        LayerBehaviorType behaviorType;
        Vector4[] movingElements;
        public Layer(Rectangle r, Texture2D t, LayerBehaviorType lbt)
            : base(r, t)
        {
            behaviorType = lbt;
            if (behaviorType == LayerBehaviorType.Cloud)
            {
                //WorldObject version (less precision)
                /*movingElements = new List<WorldObject>();
                WorldObject w1 = new WorldObject(new Rectangle(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - t.Width) / 3, Game1.RapperRandomDiggityDawg.Next(0, 100), t.Width, t.Height), t);
                w1.velocity = new Vector2((float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);

                WorldObject w2 = new WorldObject(new Rectangle(Game1.RapperRandomDiggityDawg.Next((int)Spotlight.stageCorner.X / 3, (int)(((2*Spotlight.stageCorner.X)-t.Width)) / 3), Game1.RapperRandomDiggityDawg.Next(0, 100), t.Width, t.Height), t);
                w2.velocity = new Vector2((float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);

                WorldObject w3 = new WorldObject(new Rectangle(Game1.RapperRandomDiggityDawg.Next((int)((2 * Spotlight.stageCorner.X) / 3), (int)(Spotlight.stageCorner.X - t.Width)), Game1.RapperRandomDiggityDawg.Next(0, 100), t.Width, t.Height), t);
                w3.velocity = new Vector2((float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);*/

                //Vector4 version (less flexibility)
                movingElements = new Vector4[3];
                movingElements[0] = new Vector4(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - t.Width), Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);
                movingElements[1] = new Vector4(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - t.Width), Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);
                movingElements[2] = new Vector4(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - t.Width), Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);

                //old code with variety based on thirds of the screen; will crash levels with third increments smaller than the cloud texture width!
                //movingElements[0] = new Vector4(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - t.Width) / 3, Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);
                //movingElements[1] = new Vector4(Game1.RapperRandomDiggityDawg.Next((int)Spotlight.stageCorner.X / 3, (int)(((2 * Spotlight.stageCorner.X) - t.Width)) / 3), Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);
                //movingElements[2] = new Vector4(Game1.RapperRandomDiggityDawg.Next((int)((2 * Spotlight.stageCorner.X) / 3), (int)(Spotlight.stageCorner.X - t.Width)), Game1.RapperRandomDiggityDawg.Next(0, 100), (float)(Game1.RapperRandomDiggityDawg.Next(50, 200) / 100.0), 0);



                //make clouds
                //movingElements.Add(new WorldObject(

            }
            else if (behaviorType == LayerBehaviorType.Fog)
            {
                movingElements = new Vector4[1];
                movingElements[0] = new Vector4(Game1.RapperRandomDiggityDawg.Next(0, (int)Spotlight.stageCorner.X - GraphicsDeviceManager.DefaultBackBufferWidth), 0, 1f, 0);
            }


        }

        public void Update(int gametime)
        {
            //get the three variables needed to determine proper scrolling for this layer
            spotlightX = (int)Spotlight.offset.X;
            spotlightY = (int)Spotlight.offset.Y;
            spotlightScale = 1.0f;

            switch (behaviorType)
            {
                case LayerBehaviorType.Cloud:
                    for (int i = 0; i < movingElements.Length; i++)
                    {
                        movingElements[i].X += movingElements[i].Z;
                        //movingElements[i].Y += movingElements[i].W; // clouds that move up? 
                        if (movingElements[i].X > Spotlight.stageCorner.X)
                            movingElements[i].X = -drawTex.Width - 10;
                    }
                    break;

                case LayerBehaviorType.Fog:
                    movingElements[0].X += movingElements[0].Z;
                    if (movingElements[0].X > (int)Spotlight.stageCorner.X)
                        movingElements[0].X = 0;
                    break;

                case LayerBehaviorType.NearBG:
                    bgX = ((0 - spotlightX/2.0)); // adjusts for the offset 
                    bgY = ((0 - spotlightY/2.0)); // and zoom rate for layer2
                    //SetPosition(new Point(
                    //    (int)(((0 - spotlightX / 2.0 - 400) * spotlightScale) + 400), //adjusts for the
                    //    (int)(((0 - spotlightY/2.0 - 240) * spotlightScale) + 240) //offset and zoom rate
                    //    ));
                    break;

                case LayerBehaviorType.FarBG:
                    bgX = ((0 - spotlightX/3.0)); // adjusts for the offset 
                    bgY = ((0 - spotlightY/3.0)); // and zoom rate for layer2
                    //SetPosition(new Point(
                    //    (int)(((0 - spotlightX / 3.0 - 400) * spotlightScale) + 400), //adjusts for the
                    //    (int)(((0 - spotlightY / 3.0 - 240) * spotlightScale) + 240) //offset and zoom rate
                    //    ));
                    break;


                case LayerBehaviorType.GlowingLine:
                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            switch (behaviorType)
            {
                case LayerBehaviorType.Cloud:
                    for (int i = 0; i < movingElements.Length; i++)
                        sb.Draw(drawTex, new Rectangle((int)movingElements[i].X, (int)movingElements[i].Y, drawTex.Width, drawTex.Height), Color.White);
                    break;

                case LayerBehaviorType.Fog:
                    sb.Draw(drawTex, new Rectangle((int)movingElements[0].X - (int)Spotlight.stageCorner.X, (int)movingElements[0].Y, (int)Spotlight.stageCorner.X, (int)Spotlight.stageCorner.Y), Color.White);
                    sb.Draw(drawTex, new Rectangle((int)movingElements[0].X, (int)movingElements[0].Y, (int)Spotlight.stageCorner.X, (int)Spotlight.stageCorner.Y), Color.White);
                    break;

                case LayerBehaviorType.NearBG:
                    
                    sb.End();

                    sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                                    DepthStencilState.Default, RasterizerState.CullNone);


                    sb.Draw(drawTex, new Vector2((int)bgX, (int)bgY), new Rectangle(0, 0, (int)Spotlight.stageCorner.X, (int)Spotlight.stageCorner.Y), Color.White, 0, Vector2.Zero, new Vector2(1.52f, Spotlight.stageCorner.Y / drawTex.Height), SpriteEffects.None, 0);

                    sb.End();

                    sb.Begin();
                    break;

                case LayerBehaviorType.FarBG:
                    sb.End();

                    sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                                    DepthStencilState.Default, RasterizerState.CullNone);

                    sb.Draw(drawTex, new Vector2((int)bgX, (int)bgY), new Rectangle(0, 0, (int)Spotlight.stageCorner.X, (int)Spotlight.stageCorner.Y), Color.White, 0, Vector2.Zero,new Vector2(1.52f, Spotlight.stageCorner.Y / drawTex.Height), SpriteEffects.None, 0);

                    sb.End();

                    sb.Begin();
                    break;

                default:
                    base.Draw(sb);
                    break;
            }
        }
    }
}
