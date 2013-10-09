using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using LevelEditor.WorldObjects;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    public class Menu
    {
        Texture2D menuTex, exportButtonTex, plusTex, minusTex, highlightTex, commandsButtonTex, backgroundButtonTex, pointButtonTex, xTex, yTex, tTex, openButtonTex;
        Texture2D platformIconTex, enemyIconTex, doodadIconTex, itemIconTex, hazardousPlatformIconTex, startIconTex, goalIconTex, ropeIconTex, addTriggerTex;
        Rectangle menuRect, exportButtonRect, plusRect, minusRect, highlightRect, commandsButtonRect, backgroundButtonRect, pointButtonRect, openButtonRect;
        public Rectangle platformIconRect, enemyIconRect, doodadIconRect, itemIconRect, hazardousPlatformIconRect, startIconRect, goalIconRect, ropeIconRect, addTriggerRect;
        const int NUM_COMMANDS = 7;
        const int NUM_BACKGROUNDS = 4;
        const int ICON_SPLIT = 50;
        const int MENU_X = 50;
        const int MENU_Y = 10;
        const int ICON_SIZE = 40;
        int [] NUM_TEXTURES = new int [] {41,4,28,25,0};
        const int MAX_TEXTURES = 50;
        const int MAX_PATROL = 10;
        public String[] objectStrings = new String[] {"platform","enemy","doodad", "hplatform"};
        Texture2D[] commandTex = new Texture2D[NUM_COMMANDS];
        Rectangle[] commandRect = new Rectangle[NUM_COMMANDS];

        Texture2D[] backgroundTex = new Texture2D[NUM_BACKGROUNDS];
        public Rectangle[] backgroundRect = new Rectangle[NUM_BACKGROUNDS];
        
        public int objectChosen = 0;
        
        // 0 - platforms
        // 1 - enemies
        // 2 - doodads
        // 3 - hazardous platforms

        public Texture2D[,] texturesTex = new Texture2D [MAX_TEXTURES,5];
        public Rectangle[,] texturesRect = new Rectangle [MAX_TEXTURES,5];


        Texture2D[] platformsTex = new Texture2D[1];

        WorldObject selectedObj;

        int selectedIndex = 0;
        public int selectedBackgroundIndex = 0;
        public int selectedTextureIndex = 1;
        public int selectedPatrolIndex = 0;
        public int selectedPatrolValue = 1;
        int sheight, swidth;
        int[] counter = new int[NUM_COMMANDS];
        bool clicking = false;
        public bool showCommandInfo, showBackgroundInfo, showObjectInfo;
        public Menu() { }
        public void loadContent(ContentManager c, int SCREEN_WIDTH, int SCREEN_HEIGHT)
        {
            menuTex = c.Load<Texture2D>("menu");
            menuRect = new Rectangle(SCREEN_WIDTH - 200, 0, 200, SCREEN_HEIGHT);
            exportButtonTex = c.Load<Texture2D>("exportbutton");
            exportButtonRect = new Rectangle(MENU_X + 1*ICON_SPLIT, MENU_Y , ICON_SIZE, ICON_SIZE);
            openButtonTex = c.Load<Texture2D>("openbutton");
            openButtonRect = new Rectangle(MENU_X+2*ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            platformIconTex = c.Load<Texture2D>("platformicon");
            platformIconRect = new Rectangle(MENU_X + 3 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            hazardousPlatformIconTex = c.Load<Texture2D>("hazardousplatform");
            hazardousPlatformIconRect = new Rectangle(MENU_X + 4 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            enemyIconTex = c.Load<Texture2D>("enemyicon");
            enemyIconRect = new Rectangle(MENU_X + 5 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            doodadIconTex = c.Load<Texture2D>("doodadicon");
            doodadIconRect = new Rectangle(MENU_X + 6 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            itemIconTex = c.Load<Texture2D>("itemicon");
            itemIconRect = new Rectangle(MENU_X + 7 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            startIconTex = c.Load<Texture2D>("start");
            startIconRect = new Rectangle(MENU_X + 8 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            goalIconTex = c.Load<Texture2D>("finish");
            goalIconRect = new Rectangle(MENU_X + 9 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            commandsButtonTex = c.Load<Texture2D>("commandsbutton");
            commandsButtonRect = new Rectangle(MENU_X + 10 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            backgroundButtonTex = c.Load<Texture2D>("backgroundbutton");
            backgroundButtonRect = new Rectangle(MENU_X+11*ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            ropeIconTex = c.Load<Texture2D>("ropeicon");
            ropeIconRect = new Rectangle(MENU_X + 12 * ICON_SPLIT, MENU_Y, ICON_SIZE, ICON_SIZE);
            pointButtonTex = c.Load<Texture2D>("pointbutton");
            pointButtonRect = new Rectangle(MENU_X + 1 * ICON_SPLIT, MENU_Y + ICON_SPLIT, ICON_SIZE, ICON_SIZE);
            addTriggerTex = c.Load<Texture2D>("trigger");
            addTriggerRect = new Rectangle(MENU_X + 2 * ICON_SPLIT, MENU_Y + ICON_SPLIT, ICON_SIZE, ICON_SIZE);
            plusTex = c.Load<Texture2D>("plus");
            plusRect = new Rectangle(SCREEN_WIDTH - 160, 300, ICON_SIZE, ICON_SIZE);
            minusTex = c.Load<Texture2D>("minus");
            minusRect = new Rectangle(SCREEN_WIDTH - 100, 300, ICON_SIZE, ICON_SIZE);
            xTex = c.Load<Texture2D>("x");
            yTex = c.Load<Texture2D>("y");
            tTex = c.Load<Texture2D>("t");
            highlightTex = c.Load<Texture2D>("highlight");
            highlightRect = new Rectangle(0, 0, 0, 0);
            sheight = SCREEN_HEIGHT;
            swidth = SCREEN_WIDTH;
            showCommandInfo = showBackgroundInfo = showObjectInfo = false;
            for (int i = 0; i < NUM_COMMANDS; i++)
            {
                counter[i] = -1;
                commandTex[i] = c.Load<Texture2D>("commands/" + i);
                commandRect[i] = new Rectangle(commandsButtonRect.X, ICON_SPLIT + i * 30, 20, 20);
            }
            for (int i = 0; i < NUM_BACKGROUNDS; i++)
            {
                backgroundTex[i] = c.Load<Texture2D>("background" + (i + 1));
                backgroundRect[i] = new Rectangle(backgroundButtonRect.X, ICON_SPLIT + i * 50, 40, 40);
            }
            
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < NUM_TEXTURES[j]; i++)
                {
                    switch (j)
                    {
                        case 0:
                            texturesTex[i, j] = c.Load<Texture2D>("platforms/"+objectStrings[j] + (i + 1));
                            break;
                        case 1:
                            texturesTex[i, j] = c.Load<Texture2D>("enemies/"+objectStrings[j] + (i + 1));
                            break;
                        case 2:
                            texturesTex[i, j] = c.Load<Texture2D>("noninteractive/"+objectStrings[j] + (i + 1));
                            break;
                        case 3:
                            texturesTex[i, j] = c.Load<Texture2D>("hplatforms/" + objectStrings[j] + (i + 1));
                            break;
                        default: break;
                    }
                    texturesRect[i, j] = new Rectangle(swidth - menuRect.Width + 10 + (i % 5) * 40, 380 + (i / 5) * 40, 30, 30);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, WorldObject selectedObject, SpriteFont font)
        {
            selectedObj = selectedObject;
            spriteBatch.Draw(menuTex, menuRect, Color.White);
            spriteBatch.Draw(highlightTex, highlightRect, Color.White);

            spriteBatch.Draw(exportButtonTex, exportButtonRect, Color.White);
            spriteBatch.Draw(openButtonTex, openButtonRect, Color.White);
            spriteBatch.Draw(platformIconTex, platformIconRect, Color.White);
            spriteBatch.Draw(hazardousPlatformIconTex, hazardousPlatformIconRect, Color.White);
            spriteBatch.Draw(enemyIconTex, enemyIconRect, Color.White);
            spriteBatch.Draw(doodadIconTex, doodadIconRect, Color.White);
            spriteBatch.Draw(itemIconTex, itemIconRect, Color.White);
            spriteBatch.Draw(startIconTex, startIconRect, Color.White);
            spriteBatch.Draw(goalIconTex, goalIconRect, Color.White);            
            spriteBatch.Draw(commandsButtonTex, commandsButtonRect, Color.White);
            spriteBatch.Draw(backgroundButtonTex, backgroundButtonRect, Color.White);
            spriteBatch.Draw(ropeIconTex, ropeIconRect, Color.White);
            
            if (showObjectInfo && selectedObject != null)
            {
                spriteBatch.Draw(addTriggerTex, addTriggerRect, Color.White);
                spriteBatch.DrawString(font, "Rect:" + selectedObject.drawRect.X + "," + selectedObject.drawRect.Y, new Vector2(swidth - 190, 10), Color.Black);
                spriteBatch.DrawString(font, " ," + selectedObject.drawRect.Width + "," + selectedObject.drawRect.Height, new Vector2(swidth - 100, 10), Color.Black);
                spriteBatch.DrawString(font, "(C)Copy", new Vector2(swidth - 190, 25), Color.Black);
                spriteBatch.DrawString(font, "(R)Resize", new Vector2(swidth - 190, 40), Color.Black);
                spriteBatch.DrawString(font, "(G)Gravity:" + selectedObject.hasGravity, new Vector2(swidth - 190, 60), Color.Black);
                spriteBatch.DrawString(font, "(T)Tiled:" + selectedObject.isTiled, new Vector2(swidth - 190, 80), Color.Black);
                spriteBatch.DrawString(font, "(Y)GoThrough:" + selectedObject.goThrough, new Vector2(swidth - 190, 100), Color.Black);

                int counter = 1;
                foreach (PatrolPoint p in selectedObject.points)
                {
                    counter++;
                    spriteBatch.DrawString(font, "" + p.getX() + "      " + p.getY() + "       " + p.getTime(), new Vector2(swidth - 160, 110 + counter * 25), Color.Black);
                    spriteBatch.Draw(xTex, selectedObject.xRect[counter - 1], Color.White);
                    spriteBatch.Draw(yTex, selectedObject.yRect[counter - 1], Color.White);
                    spriteBatch.Draw(tTex, selectedObject.tRect[counter - 1], Color.White);

                }
                spriteBatch.DrawString(font, "Textures:", new Vector2(swidth - 190, 350), Color.Black);                
                for (int i = 0; i < NUM_TEXTURES[objectChosen]; i++)
                {
                    spriteBatch.Draw(texturesTex[i,objectChosen], texturesRect[i,objectChosen], Color.White);
                }
                if (selectedObject.type == Util.ObjectType.Platform || selectedObject.type == Util.ObjectType.Enemy || selectedObject.type == Util.ObjectType.HazardPlatform)
                {
                    spriteBatch.DrawString(font, "Patrol Points:", new Vector2(swidth - 190, 120), Color.Black);
                    spriteBatch.Draw(pointButtonTex, pointButtonRect, Color.White);
                    spriteBatch.Draw(plusTex, plusRect, Color.White);
                    spriteBatch.Draw(minusTex, minusRect, Color.White);
                }
            }
            else if (showCommandInfo)
            {

                for (int i = 0; i < NUM_COMMANDS; i++)
                {
                    spriteBatch.DrawString(font, "  " + counter[i], new Vector2(commandsButtonRect.X+30, ICON_SPLIT + i * 30), Color.Black);
                    spriteBatch.Draw(commandTex[i], commandRect[i], Color.White);
                }
                spriteBatch.Draw(plusTex, plusRect, Color.White);
                spriteBatch.Draw(minusTex, minusRect, Color.White);
            }
            else if (showBackgroundInfo)
            {
                for (int i = 0; i < NUM_BACKGROUNDS; i++)
                {
                    spriteBatch.Draw(backgroundTex[i], backgroundRect[i], Color.White);
                }
            }
        }
        public void Update(GameTime gameTime, MouseState mouse)
        {
            int x = mouse.X;
            int y = mouse.Y;
            Point p = new Point(x, y);
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (showObjectInfo && selectedObj != null)
                {
                    showCommandInfo = showBackgroundInfo = false;
                    for (int i = 0; i < selectedObj.numPoints; i++)
                    {
                        if (checkContains(selectedObj.xRect[i], p))
                        {
                            selectedPatrolIndex = i;
                            selectedPatrolValue = 1;
                        }
                        else if (checkContains(selectedObj.yRect[i], p))
                        {
                            selectedPatrolIndex = i;
                            selectedPatrolValue = 2;
                        }
                        else if (checkContains(selectedObj.tRect[i], p))
                        {
                            selectedPatrolIndex = i;
                            selectedPatrolValue = 3;
                        }
                    }
                    checkPatrols();
                }
                else if (showCommandInfo) { checkCommandSelected(x, y); }

                if (!clicking)
                {
                    if (showCommandInfo)
                    {
                        checkPlus(x, y);
                        checkMinus(x, y);
                    }
                    else if (showObjectInfo)
                    {
                        checkPatrolPlus(p);
                        checkPatrolMinus(p);
                    }
                    if (checkContains(commandsButtonRect, p))
                    {
                        showBackgroundInfo = showObjectInfo = false;
                        showCommandInfo = !showCommandInfo;
                    }
                    else if (checkContains(backgroundButtonRect, p))
                    {
                        showCommandInfo = showObjectInfo = false;
                        showBackgroundInfo = !showBackgroundInfo;
                    }
                    else if (checkContains(pointButtonRect, p) && selectedObj != null)
                    {
                        showCommandInfo = showBackgroundInfo = false;
                        showObjectInfo = true;
                        selectedObj.points.AddLast(new PatrolPoint(0, 0, 0));
                        selectedObj.numPoints++;
                    }
                    else if (checkContains(addTriggerRect, p) && selectedObj != null)
                    {
                        showBackgroundInfo = showCommandInfo = false;
                        Rectangle triggerRect = new Rectangle(selectedObj.drawRect.X, selectedObj.drawRect.Y, 40, 40);
                        selectedObj.addTrigger(new Trigger(triggerRect, addTriggerTex, selectedObj));
                    }
                }
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                clicking = false;
            }
        }
        public void setObjectChosen(int o)
        {
            objectChosen = o;
        }
        public bool checkExport(int x, int y)
        {
            if (exportButtonRect.Contains(new Point(x, y)))
            {
                drawHighlight(exportButtonRect);
                return true;
            }
            return false;
        }
        public bool checkOpen(int x, int y)
        {
            if (openButtonRect.Contains(new Point(x, y)))
            {
                drawHighlight(openButtonRect);
                return true;
            }
            return false;
        }
        public bool checkContains(Rectangle r, Point p)
        {
            if (r.Contains(p))
            {
                drawHighlight(r);
                clicking = true;
                return true;
            }
            return false;
        }

        public void checkCommandSelected(int x, int y)
        {
            for (int i = 0; i < NUM_COMMANDS; i++)
            {
                if (commandRect[i].Contains(new Point(x, y)))
                {
                    drawHighlight(commandRect[i]);
                    selectedIndex = i;
                }
            }
        }
        public bool checkPlus(int x, int y)
        {
            if (plusRect.Contains(new Point(x, y)))
            {
                //drawHighlight(plusRect);
                counter[selectedIndex]++;
                clicking = true;
                return true;
            }
            return false;
        }
        public bool checkMinus(int x, int y)
        {
            if (minusRect.Contains(new Point(x, y)))
            {
                // drawHighlight(minusRect);
                counter[selectedIndex]--;
                if (counter[selectedIndex] == -2) counter[selectedIndex] = -1;
                clicking = true;
                return true;
            }
            return false;
        }

        public bool checkPatrolPlus(Point p)
        {
            if (plusRect.Contains(p))
            {
                selectedObj.incrementPointAt(selectedPatrolIndex, selectedPatrolValue);
                clicking = true;
                return true;
            }
            return false;
        }
        public bool checkPatrolMinus(Point p)
        {
            if (minusRect.Contains(p))
            {
                selectedObj.decrementPointAt(selectedPatrolIndex, selectedPatrolValue);
                clicking = true;
                return true;
            }
            return false;
        }

        public bool checkBackgroundButtons(Point p)
        {
            for (int i = 0; i < NUM_BACKGROUNDS; i++)
            {
                if (checkContains(backgroundRect[i], p))
                {
                    selectedBackgroundIndex = i;
                    return true;
                }
            }
            return false;
        }
        public bool checkTextureButtons(Point p)
        {
            for (int i = 0; i < NUM_TEXTURES[objectChosen]; i++)
            {
                if (checkContains(texturesRect[i,objectChosen], p))
                {
                    selectedTextureIndex = i + 1;
                    return true;
                }
            }
            return false;
        }
        public void checkPatrols()
        {
            for (int i = 0; i < selectedObj.numPoints; i++)
            {
                selectedObj.xRect[i] = new Rectangle(swidth - 190, 110 + (i + 1) * 25, 20, 20);
                selectedObj.yRect[i] = new Rectangle(swidth - 130, 110 + (i + 1) * 25, 20, 20);
                selectedObj.tRect[i] = new Rectangle(swidth - 70, 110 + (i + 1) * 25, 20, 20);
            }
        }
        public void drawHighlight(Rectangle r)
        {
            if (r != null)
            {
                highlightRect.X = r.X - 5;
                highlightRect.Y = r.Y - 5;
                highlightRect.Width = r.Width + 10;
                highlightRect.Height = r.Height + 10;
            }
        }
        public int[] getCommandNums()
        {
            return counter;
        }
        public void setCommandNums(int[] c)
        {
            counter = c;
        }
    }
}
