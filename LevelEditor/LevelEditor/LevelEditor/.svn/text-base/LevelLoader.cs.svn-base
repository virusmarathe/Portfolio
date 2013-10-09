using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelEditor.WorldObjects;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LevelEditor
{
    class LevelLoader
    {
        String fileName;
        ContentManager cm;
        const int SCREEN_WIDTH = 1024;
        const int NUM_PLATFORMS = 38;
        public LevelLoader(String s, ContentManager c)
        {
            fileName = s;
            cm = c;
        }
        public void parseFile(LinkedList<WorldObject> objects, Texture2D [,] textures, Menu m) {
            while (objects.First != null)
            {
                objects.RemoveLast();
            }
            parseStartAndGoal(objects, textures);
            parsePlatforms(objects, textures);
            parseEnemies(objects, textures);
            parseItems(objects);
            parseDoodads(objects, textures);
            parseCommands(objects,m);
            parseRopes(objects);
        }
        public Rectangle parseBounds()
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("levelinfo");
                r.ReadToFollowing("bounds");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                r.Close();
            }
            String[] boundCoord = content.Split(',');
            Rectangle b = new Rectangle(0, 0, Convert.ToInt32(boundCoord[0]), Convert.ToInt32(boundCoord[1]));
            return b;
        }
        public void parseStartAndGoal(LinkedList<WorldObject> objects, Texture2D[,] textures)
        {
            String content, content2;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("startlocation");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                //                "\n        rec 160,530,490,100\n        nogravity\n        nopatrol\n        tex 0\n      "
                r.ReadToFollowing("goalregion");
                content2 = r.ReadElementContentAsString();
                content2 = content2.Trim();
                r.Close();
            }
            if (content.Length > 0)
            {
                String[] startCoord = content.Split(',');
                Rectangle startRect = new Rectangle(Convert.ToInt32(startCoord[0]), Convert.ToInt32(startCoord[1]), 50, 75);
                Texture2D startTex = cm.Load<Texture2D>("start");
                objects.AddLast(new WorldObject(startRect, startTex, Util.ObjectType.Start, false, false));
            }
            if (content2.Length > 0)
            {
                String[] endCoord = content2.Split(',');
                Rectangle endRect = new Rectangle(Convert.ToInt32(endCoord[0]), Convert.ToInt32(endCoord[1]), 50, 50);
                Texture2D endTex = cm.Load<Texture2D>("finish");
                objects.AddLast(new WorldObject(endRect, endTex, Util.ObjectType.Goal, false, false));
            }
        }
        public void parsePlatforms(LinkedList<WorldObject> objects, Texture2D[,] textures)
        {
            String content;
            int platformVars = 8;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("platforms");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                //                "\n        rec 160,530,490,100\n        nogravity\n        ptl x1vel,y1vel,time1,x2vel,y2vel,time2\n        tex 0\n      "
                r.Close();
            }
            // TODO: add properties for hazardous here to level load
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = properties[i].Trim();
                }
                for (int i = 0; i < properties.Length; i += platformVars)
                {
                    String[] platformDim = new String[5];
                    platformDim = properties[i].Split(',');
                    platformDim[0] = platformDim[0].Substring(4);
                    Rectangle platformRect = new Rectangle(Convert.ToInt32(platformDim[0]), Convert.ToInt32(platformDim[1]), Convert.ToInt32(platformDim[2]), Convert.ToInt32(platformDim[3]));
                    String textName = "";
//                                    String textName = properties[i + 4].Substring(4);
  //                                int texNum = Convert.ToInt32(textName);    
                    
                    if (properties[i + 4][0] == 'p')
                    {
                        textName = properties[i + 4].Substring(8);
                    }
                    else if (properties[i+4][0] == 'h')
                    {
                        textName = properties[i + 4].Substring(9);
                    }
                    int texNum = Convert.ToInt32(textName)-1;
                    String isHazardous = properties[i + 3];
                    Texture2D platformTex = textures[texNum, 0];
                    WorldObject w;
                    bool goThrough;
                    bool tile;
                    if (properties[i+6].Equals("through")) goThrough = true;
                    else goThrough = false;
                    if (properties[i + 7].Equals("tile")) tile = true;
                    else tile = false;
                    if (isHazardous.Equals("hazardous"))
                    {
                        platformTex = textures[texNum, 3];
                        w = new WorldObject(platformRect, platformTex, Util.ObjectType.HazardPlatform, texNum, goThrough, tile);
                    }
                    else {
                        w = new WorldObject(platformRect, platformTex, Util.ObjectType.Platform, texNum, goThrough, tile);
                    }
                    String hasGravity = properties[i + 1];
                    if (hasGravity.Equals("yesgravity")) w.hasGravity = true;
                    else w.hasGravity = false;
                    String s = properties[i + 2];
                    s = s.Substring(4);
                    String [] ptlPoints = s.Split(',');
                    for (int j = 0; j < ptlPoints.Length-1; j += 3)
                    {
                        PatrolPoint p = new PatrolPoint(Convert.ToInt32(ptlPoints[j]), Convert.ToInt32(ptlPoints[j + 1]), Convert.ToInt32(ptlPoints[j + 2]));
                        w.points.AddLast(p);
                        w.numPoints++;
                    }
                    String s2 = properties[i + 5];
                    s2 = s2.Substring(8);
                    String[] triggerPoints = s2.Split(',');
                    for (int j = 0; j < triggerPoints.Length - 1; j += 4)
                    {
                        Trigger t = new Trigger(new Rectangle(Convert.ToInt32(triggerPoints[j]), Convert.ToInt32(triggerPoints[j+1]), Convert.ToInt32(triggerPoints[j+2]), Convert.ToInt32(triggerPoints[j+3])),
                            cm.Load<Texture2D>("trigger"));
                        w.triggers.AddLast(t);
                    }
                    objects.AddLast(w);
                }
            }

        }
        public void parseEnemies(LinkedList<WorldObject> objects, Texture2D[,] textures)
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("enemies");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                //                "\n        rec 160,530,490,100\n        nogravity\n        nopatrol\n        tex 0\n      "
                r.Close();
            }
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = properties[i].Trim();
                }
                for (int i = 0; i < properties.Length; i += 3)
                {
                    String[] platformDim = new String[5];
                    platformDim = properties[i].Split(',');
                    platformDim[0] = platformDim[0].Substring(4);
                    Rectangle platformRect = new Rectangle(Convert.ToInt32(platformDim[0]), Convert.ToInt32(platformDim[1]), Convert.ToInt32(platformDim[2]), Convert.ToInt32(platformDim[3]));
                    String textName = properties[i + 2].Substring(4);
                    int texNum = Convert.ToInt32(textName);
                    Texture2D enemyTex = textures[texNum, 1];
                    WorldObject w = new WorldObject(platformRect, enemyTex, Util.ObjectType.Enemy, texNum, false, false);
                    String s = properties[i + 1];
                    s = s.Substring(4);
                    String[] ptlPoints = s.Split(',');
                    for (int j = 0; j < ptlPoints.Length - 1; j += 3)
                    {
                        PatrolPoint p = new PatrolPoint(Convert.ToInt32(ptlPoints[j]), Convert.ToInt32(ptlPoints[j + 1]), Convert.ToInt32(ptlPoints[j + 2]));
                        w.points.AddLast(p);
                        w.numPoints++;
                    }
                    objects.AddLast(w);
                }
            }
        }
        public void parseItems(LinkedList<WorldObject> objects)
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("items");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                //                "\n        rec 160,530,490,100\n        nogravity\n        nopatrol\n        tex 0\n      "
                r.Close();
            }
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = properties[i].Trim();
                }
                for (int i = 0; i < properties.Length; i += 2)
                {
                    String[] itemDim = properties[i].Split(',');
                    itemDim[0] = itemDim[0].Substring(4);
                    Rectangle itemRect = new Rectangle(Convert.ToInt32(itemDim[0]), Convert.ToInt32(itemDim[1]), Convert.ToInt32(itemDim[2]), Convert.ToInt32(itemDim[3]));
                    Texture2D itemTex = cm.Load<Texture2D>(properties[i+1]);
                    if (properties[i+1].Equals("sword")) {
                        objects.AddLast(new WorldObject(itemRect, itemTex, Util.ObjectType.Sword, false, false));
                    }
                    else {
                        objects.AddLast(new WorldObject(itemRect, itemTex, Util.ObjectType.Shuriken, false, false));    
                    }
                }
            }
        }
        public void parseDoodads(LinkedList<WorldObject> objects, Texture2D[,] textures)
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("doodads");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                //                "\n        rec 160,530,490,100\n        nogravity\n        nopatrol\n        tex 0\n      "
                r.Close();
            }
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = properties[i].Trim();
                }
                for (int i = 0; i < properties.Length; i += 2)
                {
                    String[] platformDim = new String[5];
                    platformDim = properties[i].Split(',');
                    platformDim[0] = platformDim[0].Substring(4);
                    Rectangle platformRect = new Rectangle(Convert.ToInt32(platformDim[0]), Convert.ToInt32(platformDim[1]), Convert.ToInt32(platformDim[2]), Convert.ToInt32(platformDim[3]));
                    String textName = properties[i + 1].Substring(4);
                    int texNum = Convert.ToInt32(textName);
                    Texture2D enemyTex = textures[texNum, 2];
                    WorldObject w = new WorldObject(platformRect, enemyTex, Util.ObjectType.Doodad, texNum, false, false);
                    
                    objects.AddFirst(w);
                }
            }
        }

        public void parseCommands(LinkedList<WorldObject> objects, Menu m)
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("commands");

                content = r.ReadElementContentAsString();
                content = content.Trim();
                r.Close();
            }
            String[] properties = content.Split('\n');
            int[] commandCounter = new int[7];
            if (properties.Length > 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = properties[i].Trim();
                    properties[i] = properties[i].Substring(7);
                    commandCounter[i] = Convert.ToInt32(properties[i]);
                }
                m.setCommandNums(commandCounter);
            }            
        }

        public void parseRopes(LinkedList<WorldObject> objects)
        {
            String content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(fileName))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToFollowing("ropes");

                content = r.ReadElementContentAsString();
                content.Trim();
                r.Close();
            }
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 1; i < properties.Length-1; i++)
                {
                    properties[i] = properties[i].Trim();
                    String[] ropeRect = properties[i].Split(',');
                    Rectangle endRect = new Rectangle(Convert.ToInt32(ropeRect[0]), Convert.ToInt32(ropeRect[1]), Convert.ToInt32(ropeRect[2]), Convert.ToInt32(ropeRect[3]));

                    Texture2D endTex = cm.Load<Texture2D>("hanging rope");
                    objects.AddLast(new WorldObject(endRect, endTex, Util.ObjectType.Rope, false, true));
                }
            }
        }
    }
}
