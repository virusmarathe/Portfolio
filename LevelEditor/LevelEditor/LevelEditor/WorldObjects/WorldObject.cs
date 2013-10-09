
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using LevelEditor.Util;


namespace LevelEditor.WorldObjects
{

    public class WorldObject
    {
        public Vector2 velocity;
        public Texture2D drawTex;
        public Rectangle drawRect;
        public float rotationAngle;
        public bool hasGravity;
        const int TILE_SIZE = 32;
        public bool isHazardous;
        public bool isPicked;
        public bool goThrough;
        public bool isTiled;
        public int mouseX, mouseY;
        public int textNum;
        public LinkedList<PatrolPoint> points;
        public LinkedList<Trigger> triggers = new LinkedList<Trigger>();

        public int numPoints = 1;

        const int MAX_PATROL = 100;
        public Rectangle[] xRect = new Rectangle[MAX_PATROL];
        public Rectangle[] yRect = new Rectangle[MAX_PATROL];
        public Rectangle[] tRect = new Rectangle[MAX_PATROL];


        public ObjectType type;

        public WorldObject(Rectangle rect, Texture2D tex, ObjectType t, bool through, bool tile)
        {
            drawRect = rect;
            drawTex = tex;
            isPicked = false;
            isHazardous = false;
            goThrough = through;
            isTiled = tile;
            velocity = new Vector2(0, 0);
            points = new LinkedList<PatrolPoint>();
            type = t;
        }
        public WorldObject(Rectangle rect, Texture2D tex, ObjectType t, int tN, bool through, bool tile)
        {
            drawRect = rect;
            drawTex = tex;
            isPicked = false;
            velocity = new Vector2(0, 0);
            points = new LinkedList<PatrolPoint>();
            goThrough = through;
            isTiled = tile;
            textNum = tN;
            type = t;
        }        
        public virtual void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, float zoom)
        {
            if (isTiled)
            {
                if (type == ObjectType.Platform || type == ObjectType.HazardPlatform)
                {
                    for (int i = 0; i < drawRect.Width / (TILE_SIZE); i++)
                    {
                        for (int j = 0; j < drawRect.Height / (TILE_SIZE); j++)
                        {
                            spriteBatch.Draw(drawTex, new Rectangle(drawRect.X + i * TILE_SIZE, drawRect.Y + j * TILE_SIZE, TILE_SIZE, TILE_SIZE), Color.White);
                            // spriteBatch.Draw(drawTex, new Vector2(drawRect.X + i * 20, drawRect.Y + j * 20), new Rectangle(0,0,40,40), Color.White, 0, Vector2.Zero, zoom/2.0f, SpriteEffects.None, 1);
                        }
                    }
                }
                else if (type == ObjectType.Rope)
                {
                    for (int i = 0; i < drawRect.Width / (TILE_SIZE/4); i++)
                    {
                        for (int j = 0; j < drawRect.Height / (TILE_SIZE); j++)
                        {
                            spriteBatch.Draw(drawTex, new Rectangle(drawRect.X + i * (TILE_SIZE/4), drawRect.Y + j * TILE_SIZE, TILE_SIZE/4, TILE_SIZE), Color.White);
                            // spriteBatch.Draw(drawTex, new Vector2(drawRect.X + i * 20, drawRect.Y + j * 20), new Rectangle(0,0,40,40), Color.White, 0, Vector2.Zero, zoom/2.0f, SpriteEffects.None, 1);
                        }
                    }
                }
            }
            else
            {
                spriteBatch.Draw(drawTex, drawRect, Color.White);
                //                spriteBatch.Draw(drawTex, new Vector2(drawRect.X, drawRect.Y), new Rectangle(0,0,drawRect.Width,drawRect.Height), Color.White, 0, Vector2.Zero, zoom, SpriteEffects.None, 1);
            }
        }

        public bool checkPicked(int x, int y)
        {
            if (drawRect.Contains(new Point(x, y)))
            {
                return true;
            }
            return false;
        }

        public void setLocation(int x, int y)
        {
            drawRect.X = x;
            drawRect.Y = y;
        }

        public Point Location()
        {
            return drawRect.Location;
        }

        public void addTrigger(Trigger t)
        {
            triggers.AddLast(t);
        }

        public LinkedList<Trigger> getTriggers()
        {
            return triggers;
        }

        public void Offset(int x, int y)
        {
            drawRect.Offset(x, y);
        }

        public Point Size()
        {
            return new Point(drawRect.Width, drawRect.Height);
        }

        public void setVelocity(float x, float y)
        {
            velocity = new Vector2(x, y);
        }

        public Vector2 getVelocity()
        {
            return velocity;
        }

        public bool Gravity()
        {
            return hasGravity;
        }

        public void setGravity(bool falls)
        {
            hasGravity = falls;
        }

        public void incrementPointAt(int i, int choice)
        {
            int counter = 1;
            foreach (PatrolPoint p in points)
            {
                if (counter == i)
                {
                    switch (choice)
                    {
                        case 1: p.x++;
                            break;
                        case 2: p.y++;
                            break;
                        case 3: p.time+=30;
                            break;
                        default:
                            break;
                    }
                }
                counter++;
            }
        }
        public void decrementPointAt(int i, int choice)
        {
            int counter = 1;
            foreach (PatrolPoint p in points)
            {
                if (counter == i)
                {
                    switch (choice)
                    {
                        case 1: p.x--;
                            break;
                        case 2: p.y--;
                            break;
                        case 3: p.time-=30;
                            break;
                        default:
                            break;
                    }
                }
                counter++;
            }
        }

    }

}