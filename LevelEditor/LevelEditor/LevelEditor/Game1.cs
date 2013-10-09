using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LevelEditor.WorldObjects;
using System.Diagnostics;

namespace LevelEditor
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int SCREEN_WIDTH = 1024;
        const int SCREEN_HEIGHT = 768;
        const int NUM_PLATFORM_TEX = 14;
        const int TILE_SIZE = 32;
        float zoom_scale;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch, fontSpriteBatch;
        private System.Windows.Forms.SaveFileDialog saveBox;
        private System.Windows.Forms.OpenFileDialog openBox;
        MouseState mouseStateCurrent;
        int oldScrollValue, newScrollValue;
        int minX, minY, maxX, maxY;
        int backgroundType = 1;
        Vector2 pickDiff = new Vector2(0, 0);
        int xAdd;
        Menu menu;
        LinkedList<WorldObject> objects;
        WorldObject pickedUpObject, selectedObject;
        Texture2D platformTexture, highlightTexture, enemyTexture, startTexture, finishTexture, exportButtonTex, backgroundTex, swordTex, shurikenTex, gridTex, doodadTex;
        Texture2D menuOpenTex, ropeTex, boundsTex;
        Rectangle highlightRect, palette, gridRect, backgroundRect, menuOpenRect, boundsRect;
        bool addObject, resizing, saveFile, openFile, openMenu, checkrelease;
        bool addPlatform, addEnemy, addDoodad, addItem, addHazardousPlatform, addStart, addGoal, addRope;
        bool keyDown;
        bool showBounds;
        SpriteFont font;
        LevelExporter levelExport;
        LevelLoader levelLoad;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            saveBox = new System.Windows.Forms.SaveFileDialog();
            saveBox.Title = "Save Level As";
            saveBox.Filter = "Psychic Ninja Level File | *.xml";
            openBox = new System.Windows.Forms.OpenFileDialog();
            openBox.Title = "Load Psychic Ninja Level";
            openBox.Filter = "Psychic Ninja Level File | *.xml";
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            objects = new LinkedList<WorldObject>();
            highlightRect = new Rectangle(-100, -100, 80, 30);
            palette = new Rectangle(0, 0, SCREEN_WIDTH - 200, SCREEN_HEIGHT);
            gridRect = new Rectangle(0, 0, 10, 10);
            backgroundRect = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            menuOpenRect = new Rectangle(0,0,100,70);
            menu = new Menu();
            levelExport = new LevelExporter();
            saveFile = false;
            openFile = false;
            showBounds = false;
            keyDown = false;
            addPlatform = addEnemy = addDoodad = addItem = addHazardousPlatform = addStart = addGoal = addRope = false;
            openMenu = false;
            oldScrollValue = 0;
            newScrollValue = 0;
            xAdd = 0;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
            zoom_scale = 1;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontSpriteBatch = new SpriteBatch(GraphicsDevice);
            platformTexture = Content.Load<Texture2D>("platforms/platform1");
            highlightTexture = Content.Load<Texture2D>("bounds");
            enemyTexture = Content.Load<Texture2D>("enemy");
            startTexture = Content.Load<Texture2D>("start");
            ropeTex = Content.Load<Texture2D>("hanging rope");
            finishTexture = Content.Load<Texture2D>("finish");
            menuOpenTex = Content.Load<Texture2D>("openmenu");
            exportButtonTex = Content.Load<Texture2D>("exportbutton");
            backgroundTex = Content.Load<Texture2D>("background1");
            shurikenTex = Content.Load<Texture2D>("shuriken");
            doodadTex = Content.Load<Texture2D>("doodadTemplate");
            swordTex = Content.Load<Texture2D>("sword");
            gridTex = Content.Load<Texture2D>("grid");
            boundsTex = Content.Load<Texture2D>("bounds");
            font = Content.Load<SpriteFont>("Courier New");
            menu.loadContent(Content, SCREEN_WIDTH, SCREEN_HEIGHT);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            checkInput();
            if (openMenu)
            {
                menu.Update(gameTime, mouseStateCurrent);
            }
            if (saveFile)
            {
                saveBox.ShowDialog();
                if (saveBox.FileName.Length != 0)
                {
                    foreach (WorldObject o in objects)
                    {
                        levelExport.checkBounds(o.drawRect.X, o.drawRect.Y, o.drawRect.Width, o.drawRect.Height);
                    }
                    levelExport.setBounds(boundsRect.X, boundsRect.Y, boundsRect.Width, boundsRect.Height);
                    levelExport.sendObjects(objects, menu.getCommandNums(), saveBox.FileName, xAdd, boundsRect, backgroundType);
                }
                saveFile = false;
            }
            else if (openFile)
            {
                openBox.ShowDialog();
                if (openBox.FileName.Length != 0)
                {
                    levelLoad = new LevelLoader(openBox.FileName, Content);
                    boundsRect = levelLoad.parseBounds();
                    levelLoad.parseFile(objects,menu.texturesTex, menu);
                }
                openFile = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // cool texture tiling doesnt work without directx10 -_-
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Begin();
            //spriteBatch.Draw(backgroundTex, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            spriteBatch.Draw(backgroundTex, Vector2.Zero, backgroundRect, Color.White, 0.0f, Vector2.Zero, zoom_scale, SpriteEffects.None, 1);
            for (int i = 0; i < SCREEN_WIDTH / (TILE_SIZE/4); i++)
            {
                for (int j = 0; j < SCREEN_HEIGHT / (TILE_SIZE/4); j++)
                {
                    spriteBatch.Draw(gridTex, new Vector2(i * (TILE_SIZE/4)*zoom_scale, j*(TILE_SIZE/4)*zoom_scale), new Rectangle(0, 0, TILE_SIZE/4, TILE_SIZE/4), Color.White, 0, Vector2.Zero, zoom_scale, SpriteEffects.None, 1);
                }
            }
            foreach (WorldObject w in objects.Reverse())
            {
                w.Draw(spriteBatch, zoom_scale);
                if (selectedObject != null)
                {
                    if (selectedObject == w || selectedObject.type == Util.ObjectType.Trigger)
                    {
                        foreach (Trigger t in w.triggers)
                        {
                            t.Draw(spriteBatch, zoom_scale);
                        }
                    }
                }
            }
            spriteBatch.Draw(highlightTexture, highlightRect, Color.White);
            if (showBounds)
            {
                spriteBatch.Draw(boundsTex, boundsRect, Color.White);
            }
            spriteBatch.End();

            // draw coordinates and size
            fontSpriteBatch.Begin();
            fontSpriteBatch.Draw(menuOpenTex, menuOpenRect, Color.White);
            if (openMenu) {
                menu.Draw(fontSpriteBatch, selectedObject, font);
            }
            fontSpriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        // checks the mouse and keyboard input

        public void checkInput()
        {
            int x = mouseStateCurrent.X;
            int y = mouseStateCurrent.Y;
            mouseStateCurrent = Mouse.GetState();
            if (menuOpenRect.Contains(new Point(x,y)) && mouseStateCurrent.LeftButton == ButtonState.Pressed && !checkrelease){
                checkrelease = true;
                openMenuAnimate();
            }
            else if (mouseStateCurrent.LeftButton == ButtonState.Released)
            {
                checkrelease = false;
            }
            if (openMenu)
            {
                if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
                {
                    if (menu.checkExport(x, y))
                    {
                        saveFile = true;
                    }
                    else if (menu.checkOpen(x, y))
                    {
                        openFile = true;
                    }
                    else if (menu.showBackgroundInfo && menu.checkBackgroundButtons(new Point(x, y)))
                    {
                        backgroundTex = Content.Load<Texture2D>("background" + (menu.selectedBackgroundIndex + 1));
                        backgroundType = menu.selectedBackgroundIndex +1;
                    }
                    else if (menu.checkTextureButtons(new Point(x, y)))
                    {
                        if (selectedObject != null)
                        {
                            switch (selectedObject.type)
                            {
                                case Util.ObjectType.Platform:
                                    selectedObject.drawTex = Content.Load<Texture2D>("platforms/"+menu.objectStrings[menu.objectChosen] + (menu.selectedTextureIndex));
                                    break;
                                case Util.ObjectType.Enemy:
                                    selectedObject.drawTex = Content.Load<Texture2D>("enemies/"+menu.objectStrings[menu.objectChosen] + (menu.selectedTextureIndex));
                                    break;
                                case Util.ObjectType.Doodad:
                                    selectedObject.drawTex = Content.Load<Texture2D>("noninteractive/"+menu.objectStrings[menu.objectChosen] + (menu.selectedTextureIndex));
                                    break;
                                case Util.ObjectType.HazardPlatform:
                                    selectedObject.drawTex = Content.Load<Texture2D>("hplatforms/"+menu.objectStrings[menu.objectChosen] + (menu.selectedTextureIndex));
                                    break;
                                default: break;
                            }
                            selectedObject.textNum = menu.selectedTextureIndex - 1;
                        }
                    }

                }
            }
            checkMouseClicks();
            checkKeyClicks();
        }
        public void checkMouseClicks()
        {
            // left click used for selecting
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStateCurrent.X < SCREEN_WIDTH - 200)
            {
                if (showBounds && resizing)
                {
                    boundsRect.Width = (mouseStateCurrent.X - boundsRect.X - (mouseStateCurrent.X - boundsRect.X) % (TILE_SIZE / 4));
                    boundsRect.Height = (mouseStateCurrent.Y - boundsRect.Y - (mouseStateCurrent.Y - boundsRect.Y) % (TILE_SIZE / 4));
                }
                // add a platform to the list
                else if (selectedObject != null && resizing)
                {
                    if (selectedObject.type == Util.ObjectType.Platform || selectedObject.type == Util.ObjectType.HazardPlatform)
                    {
                        selectedObject.drawRect.Width = (mouseStateCurrent.X - selectedObject.drawRect.X - (mouseStateCurrent.X - selectedObject.drawRect.X) % TILE_SIZE);
                        selectedObject.drawRect.Height = (mouseStateCurrent.Y - selectedObject.drawRect.Y - (mouseStateCurrent.Y - selectedObject.drawRect.Y) % TILE_SIZE);
                    }
                    else
                    {
                        selectedObject.drawRect.Width = (mouseStateCurrent.X - selectedObject.drawRect.X - (mouseStateCurrent.X - selectedObject.drawRect.X) % (TILE_SIZE / 4));
                        selectedObject.drawRect.Height = (mouseStateCurrent.Y - selectedObject.drawRect.Y - (mouseStateCurrent.Y - selectedObject.drawRect.Y) % (TILE_SIZE / 4));
                    }
                }

                else if (!resizing)
                {
                    if (!openMenu && mouseStateCurrent.X < SCREEN_WIDTH - 200)
                        checkPicking();
                    addObject = true;
                    checkIconClick();
                }

                drawHighlight(selectedObject);
            }
            else if (mouseStateCurrent.LeftButton == ButtonState.Released)
            {
                if (addObject == true)
                {
                    placeWorldObject();
                    addObject = false;
                }
                resizing = false;
            }

            // right click used for moving
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && !resizing && mouseStateCurrent.X < SCREEN_WIDTH - 200)
            {
                checkPicking();
            }
            else if (mouseStateCurrent.LeftButton == ButtonState.Released)
            {
                pickedUpObject = null;
                pickDiff = new Vector2(0, 0);
            }

            if (mouseStateCurrent.ScrollWheelValue != newScrollValue)
            {
                newScrollValue = mouseStateCurrent.ScrollWheelValue;
            }
            
            if (newScrollValue > oldScrollValue)
            {
                zoom_scale += 0.1f;
                oldScrollValue = newScrollValue;
            }
            else if (newScrollValue < oldScrollValue)
            {
                if (zoom_scale>1.1)
                    zoom_scale -= 0.1f;
                oldScrollValue = newScrollValue;
            }
            
        }

        public void checkIconClick()
        {
            Point p = new Point(mouseStateCurrent.X, mouseStateCurrent.Y);
            if (menu.checkContains(menu.platformIconRect, p))
            {
                addPlatform = true;
                addEnemy = addDoodad = addItem = addHazardousPlatform = addStart = addGoal = addRope = false;            
            }
            else if (menu.checkContains(menu.enemyIconRect, p))
            {
                addEnemy = true;
                addPlatform = addDoodad = addItem = addHazardousPlatform = addStart = addGoal =  addRope = false;            
            }
            else if (menu.checkContains(menu.doodadIconRect, p))
            {
                addDoodad = true;
                addPlatform = addEnemy = addItem = addHazardousPlatform = addStart = addGoal = addRope = false;            
            }
            else if (menu.checkContains(menu.itemIconRect, p))
            {
                addItem = true;
                addPlatform = addEnemy = addDoodad = addHazardousPlatform = addStart = addGoal = addRope = false;            
            }
            else if (menu.checkContains(menu.hazardousPlatformIconRect, p))
            {
                addHazardousPlatform = true;
                addPlatform = addEnemy = addDoodad = addItem = addStart = addGoal = addRope = false;            
            }
            else if (menu.checkContains(menu.startIconRect, p))
            {
                addStart = true;
                addPlatform = addEnemy = addDoodad = addItem = addHazardousPlatform = addGoal = addRope = false;
            }
            else if (menu.checkContains(menu.goalIconRect, p))
            {
                addGoal = true;
                addPlatform = addEnemy = addDoodad = addItem = addStart = addHazardousPlatform = addRope = false;
            }
            else if (menu.checkContains(menu.ropeIconRect, p))
            {
                addRope = true;
                addPlatform = addEnemy = addDoodad = addItem = addStart = addHazardousPlatform = addGoal = false;
            }
        }

        public void checkKeyClicks()
        {
            KeyboardState key = Keyboard.GetState();

            // resize
            if (key.IsKeyDown(Keys.R))
            {
                resizing = true;
            }
            // delete
            else if (key.IsKeyDown(Keys.Delete) || key.IsKeyDown(Keys.Back))
            {
                objects.Remove(selectedObject);
                selectedObject = null;
                highlightRect = new Rectangle(0, 0, 0, 0);
            }
            else if (key.IsKeyDown(Keys.Space) && !keyDown)
            {
                showBounds = !showBounds;
                keyDown = true;
            }
            else if (key.IsKeyDown(Keys.G) && !keyDown && selectedObject != null) {
                selectedObject.hasGravity = !selectedObject.hasGravity;
                keyDown = true;
            }
            else if (key.IsKeyDown(Keys.T) && !keyDown && selectedObject != null)
            {
                selectedObject.isTiled = !selectedObject.isTiled;
                keyDown = true;
            }
            else if (key.IsKeyDown(Keys.Y) && !keyDown && selectedObject != null)
            {
                selectedObject.goThrough = !selectedObject.goThrough;
                keyDown = true;
            }
            else if (key.IsKeyDown(Keys.D) && !keyDown && selectedObject != null)
            {
                if (selectedObject.points.Count != 0)
                {
                    selectedObject.points.RemoveLast();
                    selectedObject.numPoints--;
                }
                keyDown = true;
            }
            // copy
            else if (key.IsKeyDown(Keys.C))
            {
                // this doesnt really work right now
                if (selectedObject != null)
                {
                    WorldObject w = new WorldObject(selectedObject.drawRect, selectedObject.drawTex, selectedObject.type, selectedObject.goThrough, selectedObject.isTiled);
                    w.points = selectedObject.points;
                    w.numPoints = selectedObject.numPoints;
                    w.textNum = selectedObject.textNum;
                    objects.AddFirst(w);
                    selectedObject = null;
                }
            }
            else if (key.IsKeyDown(Keys.Left))
            {
                backgroundRect.X -= (TILE_SIZE/4);
                boundsRect.X += (TILE_SIZE/4);
                foreach (WorldObject w in objects)
                {
                    w.setLocation(w.drawRect.X + (TILE_SIZE/4), w.drawRect.Y);
                    foreach (Trigger t in w.triggers)
                    {
                        t.setLocation(t.drawRect.X + (TILE_SIZE/4), t.drawRect.Y);
                    }
                }
                highlightRect.X = highlightRect.X + (TILE_SIZE/4);
            }
            else if (key.IsKeyDown(Keys.Up))
            {
                backgroundRect.Y -= (TILE_SIZE/4);
                boundsRect.Y += (TILE_SIZE/4);
                foreach (WorldObject w in objects)
                {
                    w.setLocation(w.drawRect.X, w.drawRect.Y + (TILE_SIZE/4));
                    foreach (Trigger t in w.triggers)
                    {
                        t.setLocation(t.drawRect.X, t.drawRect.Y + (TILE_SIZE/4));
                    }
                }
                highlightRect.Y = highlightRect.Y + (TILE_SIZE/4);
            }
            else if (key.IsKeyDown(Keys.Right))
            {
                backgroundRect.X += (TILE_SIZE/4);
                boundsRect.X -= (TILE_SIZE/4);
                foreach (WorldObject w in objects)
                {
                    w.setLocation(w.drawRect.X - (TILE_SIZE/4), w.drawRect.Y);
                    foreach (Trigger t in w.triggers)
                    {
                        t.setLocation(t.drawRect.X - (TILE_SIZE/4), t.drawRect.Y);
                    }
                }
                highlightRect.X = highlightRect.X - (TILE_SIZE/4);
                // xAdd += (TILE_SIZE/4);
            }
            else if (key.IsKeyDown(Keys.Down))
            {
                backgroundRect.Y += (TILE_SIZE/4);
                boundsRect.Y -= (TILE_SIZE/4);
                foreach (WorldObject w in objects)
                {
                    w.setLocation(w.drawRect.X, w.drawRect.Y - (TILE_SIZE/4));
                    foreach (Trigger t in w.triggers)
                    {
                        t.setLocation(t.drawRect.X, t.drawRect.Y - (TILE_SIZE/4));
                    }
                }
                highlightRect.Y = highlightRect.Y - (TILE_SIZE/4);
            }
            if (key.IsKeyUp(Keys.Space) && key.IsKeyUp(Keys.G) && key.IsKeyUp(Keys.T) && key.IsKeyUp(Keys.Y)&& key.IsKeyUp(Keys.D)) keyDown = false;
        }

        public void placeWorldObject()
        {
            KeyboardState key = Keyboard.GetState();
            int xPos = mouseStateCurrent.X - mouseStateCurrent.X % TILE_SIZE;
            int yPos = mouseStateCurrent.Y - mouseStateCurrent.Y % TILE_SIZE;
            WorldObject o = null;
            if (openMenu && yPos < 50)
            {
                return;
            }

            if (addPlatform)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, TILE_SIZE, TILE_SIZE), platformTexture, Util.ObjectType.Platform, true, true);
                objects.AddFirst(o);
            }
            else if (addHazardousPlatform)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, TILE_SIZE, TILE_SIZE), platformTexture, Util.ObjectType.HazardPlatform, true, true);
                o.isHazardous = true;
                objects.AddFirst(o);
            }
            else if (addStart)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 50, 75), startTexture, Util.ObjectType.Start, false, false);
                objects.AddFirst(o);
            }
            else if (addGoal)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 50, 50), finishTexture, Util.ObjectType.Goal, false, false);
                objects.AddFirst(o);
            }
            else if (addEnemy)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 20, 50), enemyTexture, Util.ObjectType.Enemy, false, false);
                objects.AddFirst(o);
            }
            else if (key.IsKeyDown(Keys.Z))
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 20, 20), shurikenTex, Util.ObjectType.Shuriken, false, false);
                objects.AddFirst(o);
            }
            else if (key.IsKeyDown(Keys.X) || addItem)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 20, 20), swordTex, Util.ObjectType.Sword, false, false);
                objects.AddFirst(o);
            }
            else if (addDoodad)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, 20, 20), doodadTex, Util.ObjectType.Doodad, false, false);
                objects.AddFirst(o);
            }
            else if (addRope)
            {
                o = new WorldObject(new Rectangle(xPos, yPos, TILE_SIZE/4, TILE_SIZE), ropeTex, Util.ObjectType.Rope, true, false);
                objects.AddFirst(o);
            }
            if (o != null)
            {
                selectedObject = o;
                drawHighlight(o);
            }
            checkBounds(xPos, yPos);
        }

        public void checkBounds(int x, int y)
        {
            if (x < minX)
            {
                minX = x;
            }
            else if (x > maxX)
            {
                maxX = x;
            }
            if (y < minY)
            {
                minY = y;
            }
            else if (y > maxY)
            {
                maxY = y;
            }
        }
        public void openMenuAnimate()
        {
            if (openMenu)
            {
                // open menu
            }
            else
            {
                //close menu
            }
            openMenu = !openMenu;
        }
        public void checkPicking()
        {
            int move_dist = TILE_SIZE/4;
            // if already picking something up then keep picking it up
            if (showBounds)
            {
                if (pickDiff.X == 0)
                {
                    pickDiff.X = mouseStateCurrent.X - boundsRect.X;
                    pickDiff.Y = mouseStateCurrent.Y - boundsRect.Y;
                }
                boundsRect.X = (mouseStateCurrent.X - (int)pickDiff.X) - (mouseStateCurrent.X - (int)pickDiff.X) % move_dist;
                boundsRect.Y = (mouseStateCurrent.Y - (int)pickDiff.Y) - (mouseStateCurrent.Y - (int)pickDiff.Y) % move_dist;
            }
            else
            {
                if (selectedObject != null)
                {
                    foreach (Trigger t in selectedObject.triggers)
                    {
                        if (t.checkPicked(mouseStateCurrent.X, mouseStateCurrent.Y))
                        {
                            selectedObject = t;
                            pickedUpObject = t;
                        }
                    }
                }
                if (pickedUpObject != null)
                {
                    if (pickDiff.X == 0)
                    {
                        pickDiff.X = mouseStateCurrent.X - pickedUpObject.drawRect.X;
                        pickDiff.Y = mouseStateCurrent.Y - pickedUpObject.drawRect.Y;
                    }
                    if (pickedUpObject.type == Util.ObjectType.Start)
                    {
                        move_dist /= 2;
                    }
                    pickedUpObject.drawRect.X = (mouseStateCurrent.X - (int)pickDiff.X) - (mouseStateCurrent.X - (int)pickDiff.X) % move_dist;
                    pickedUpObject.drawRect.Y = (mouseStateCurrent.Y - (int)pickDiff.Y) - (mouseStateCurrent.Y - (int)pickDiff.Y) % move_dist;
                    drawHighlight(pickedUpObject);
                }
                else
                {
                    // check for picking
                    foreach (WorldObject w in objects)
                    {
                        if (w.checkPicked(mouseStateCurrent.X, mouseStateCurrent.Y))
                        {
                            menu.showObjectInfo = true;
                            pickedUpObject = w;
                            selectedObject = w;
                            if (selectedObject.type == Util.ObjectType.Platform)
                            {
                                menu.setObjectChosen(0);
                            }
                            else if (selectedObject.type == Util.ObjectType.Enemy)
                            {
                                menu.setObjectChosen(1);
                            }
                            else if (selectedObject.type == Util.ObjectType.Doodad)
                            {
                                menu.setObjectChosen(2);
                            }
                            else if (selectedObject.type == Util.ObjectType.HazardPlatform)
                            {
                                menu.setObjectChosen(3);
                            }
                            else
                            {
                                menu.setObjectChosen(4);
                            }
                            addPlatform = addEnemy = addDoodad = addItem = addHazardousPlatform = addGoal = addStart = addRope = false;
                            break;
                        }
                    }
                }
            }
        }

        public void drawHighlight(WorldObject w)
        {
            if (w != null)
            {
                highlightRect.X = w.drawRect.X - 1;
                highlightRect.Y = w.drawRect.Y - 1;
                highlightRect.Width = w.drawRect.Width + 2;
                highlightRect.Height = w.drawRect.Height + 2;
            }
        }
    }
}
