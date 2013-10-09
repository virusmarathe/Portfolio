using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using PsychicNinja.Data.Util;
using PsychicNinja.Data.Patrol;
using PsychicNinja.Data.Object;
using PsychicNinja.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace PsychicNinja.Data.Parser
{

    /// <summary>
    /// Class for loading in levels and storing their information.  LevelParser keeps track
    /// of the starting state of all objects in the level in public variables, allowing for 
    /// them to be easily referrenced upon resetting the level.
    /// </summary>
    public static class LevelParser
    {

        private static string currentReadTarget;
        private static string chapterInfoReadTarget;

        private static bool infoPathDirty = true;

        /// <summary>
        /// Change the file that this library reads from
        /// </summary>
        /// <param name="filename"></param>
        public static void setReadTarget(string filename)
        {
            currentReadTarget = filename;
            infoPathDirty = true;
        }

        /// <summary>
        /// Returns the full path of the current target of the LevelParaser
        /// </summary>
        /// <returns></returns>
        public static string getReadTarget()
        {
            return currentReadTarget;
        }

        public static int getCurrentLevelNumber()
        {
            string thing = (getPathComponents())[4];
            return Convert.ToInt32(thing.Substring(0, thing.Length - 4));
        }

        public static int getCurrentChapterNumber()
        {
            string content;

            //if the readTarget was changed recently, correct infoPath
            if (infoPathDirty)
            {
                string[] pc = getPathComponents();
                chapterInfoReadTarget = pc[0] + "/" + pc[1] + "/" + pc[2] + "/" + pc[3] + "/chapterinfo.xml";
                infoPathDirty = false;
            }


            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(chapterInfoReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("chapternumber");

                content = r.ReadElementContentAsString();

                r.Close();
            }
            string butt = content.Trim();
            return Convert.ToInt32(butt);
        }


        public static int getChapterLevelCount()
        {
            string content;
            
            //if the readTarget was changed recently, correct infoPath
            if (infoPathDirty)
            {
                string[] pc = getPathComponents();
                chapterInfoReadTarget = pc[0] + "/" + pc[1] + "/" + pc[2] + "/" + pc[3] + "/chapterinfo.xml";
                infoPathDirty = false;
            }


            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(chapterInfoReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("levels");

                content = r.ReadElementContentAsString();

                r.Close();
            }
            string butt = content.Trim();
            return Convert.ToInt32(butt);
        }

        /// <summary>
        /// Loads in the level music
        /// </summary>
        public static string[] readLevelMusic()
        {
            string content;

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("music");

                content = r.ReadElementContentAsString();

                r.Close();
            }

            string[] stuff = CleanUp(content);
            //stuff[0].Insert(0, "
            //stuff[0].Insert(stuff[0].Length - 1, ".mp3");
            //stuff[1].Insert(stuff[1].Length - 1, ".mp3");
            return stuff;
        }

        public static string[] readDoodadTextures()
        {
            string content, content2;
            const int doodadObjSize = 2;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readPlatformTextures");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("textures");
                r.ReadToDescendant("doodad");

                content = r.ReadElementContentAsString();

                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("doodads");

                content2 = r.ReadElementContentAsString();

                r.Close();
            }
            string[] doodadNames = CleanUp(content);
            string[] doodadString = CleanUp(content2);
            int[] filternums = new int[doodadNames.Length];
            int uniqueTex = 0;
            for (int i = 0; i < filternums.Length; i++)
            {
                filternums[i] = 0;
            }
            for (int i = 0; i < doodadString.Length / doodadObjSize; i++)
            {
                int[] textureNums = getNumbers(doodadString[i * doodadObjSize + 1].Substring(4));
                if (filternums[textureNums[0]] == 0)
                {
                    filternums[textureNums[0]] = 1;
                    uniqueTex++;
                }
            }
            string [] texNames = new string[uniqueTex];
            int counter = 0;
            for (int i = 0; i < filternums.Length; i++)
            {
                if (filternums[i] == 1)
                {
                    texNames[counter] = doodadNames[i];
                    counter++;
                }
            }
            return texNames;
        }
        /// <summary>
        /// Read the textures required for the enemies in a level
        /// </summary>
        /// <returns></returns>
        public static string[] readEnemyTextures()
        {
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readEnemyTextures");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("textures");
                r.ReadToDescendant("enemy");


                content = r.ReadElementContentAsString();

                r.Close();
            }

                return CleanUp(content);
            
        }

        /// <summary>
        /// Read the desired background textures for the level.
        /// </summary>
        /// <returns></returns>
        public static string[] readBackgroundTextures()
        {
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readBackgroundTexture");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("textures");
                r.ReadToDescendant("background");

                content = r.ReadElementContentAsString();

                r.Close();
            }
            string[] stuff = CleanUp(content);

            return stuff;
            
        }

        public static Vector2 readLevelBounds()
        {
            string content;
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("levelinfo");
                r.ReadToDescendant("bounds");

                content = r.ReadElementContentAsString();

                r.Close();
            }
            int[] a = getNumbers(content);
            return new Vector2(a[0], a[1]);
        }

        public static LayerBehaviorType readTilesetBehavior()
        {
            string content;

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("levelinfo");
                r.ReadToDescendant("tileset");

                content = r.ReadElementContentAsString();
                
                r.Close();
            }

            content = CleanUp(content)[0];

            if (content == "city-day" || content == "city-noon" || content == "city-night")
                return LayerBehaviorType.Cloud;
            if (content == "lair-glowing")
                return LayerBehaviorType.GlowingLine;
            return LayerBehaviorType.Static;
        }

        /// <summary>
        /// reads and returns the ninja start location associated with this level
        /// </summary>
        /// <returns></returns>
        public static Point readStartLocation()
        {
            int[] values;
            string content, stuff;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readCommandLimits");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("startlocation");
                content = r.ReadElementContentAsString();
                r.Close();
            }

            stuff = (CleanUp(content))[0];
            if (stuff.Length == 0) return new Point(0, 0); // Don't error if we don't put in a start.
            values = getNumbers(stuff);
            return new Point(values[0], values[1]);
        }

        /// <summary>
        /// Read the location that serves as the winning condition for the ninja.
        /// </summary>
        /// <returns></returns>
        public static Rectangle readGoalRegion()
        {
            int[] values;
            string content, stuff;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readCommandLimits");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("goalregion");
                content = r.ReadElementContentAsString();
                r.Close();
            }

            stuff = (CleanUp(content))[0];
            values = getNumbers(stuff);
            return new Rectangle(values[0], values[1], values[2], values[3]);
        }

        // readRopes
        public static LinkedList<SuspendRope> readRopes()
        {
            string content;
            LinkedList<SuspendRope> ropes = new LinkedList<SuspendRope>();

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readCommandLimits");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("ropes");
                content = r.ReadElementContentAsString();
                r.Close();
            }
            String[] properties = content.Split('\n');
            if (properties.Length > 1)
            {
                for (int i = 1; i < properties.Length-1; i++)
                {
                    String[] ropeRect = properties[i].Split(',');
                    Rectangle endRect = new Rectangle(Convert.ToInt32(ropeRect[0]), Convert.ToInt32(ropeRect[1]), Convert.ToInt32(ropeRect[2]), Convert.ToInt32(ropeRect[3]));
                    SuspendRope r = SuspendRope.droppableDevRope(endRect, new Vector2(0, 0), false);
                    ropes.AddLast(r);
                }
            }
            return ropes;
        }
        /// <summary>
        /// returns an array containing the command limits in the order specified
        /// by the XML level file standard
        /// </summary>
        /// <param name="information"></param>
        public static int[] readCommandLimits()
        {
            int[] commandLimits = new int[(int)CommandType.NumCommands];
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readCommandLimits");

            System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget);

            r.MoveToContent();
            r.ReadToFollowing("objects");
            r.ReadToDescendant("commands");
            content = r.ReadElementContentAsString();
            r.Close();

            String[] CommandLimits = CleanUp(content);
            commandLimits[(int)CommandType.MoveLeft]    = Convert.ToInt32(CommandLimits[0].Substring(7));
            commandLimits[(int)CommandType.MoveRight]   = Convert.ToInt32(CommandLimits[1].Substring(7));
            commandLimits[(int)CommandType.Jump]        = Convert.ToInt32(CommandLimits[2].Substring(7));
            commandLimits[(int)CommandType.WallJump]    = Convert.ToInt32(CommandLimits[3].Substring(7));
            commandLimits[(int)CommandType.WallSlide]   = Convert.ToInt32(CommandLimits[4].Substring(7));
            commandLimits[(int)CommandType.LedgeClimb]  = Convert.ToInt32(CommandLimits[5].Substring(7));
            commandLimits[(int)CommandType.ObjectThrow] = Convert.ToInt32(CommandLimits[6].Substring(7));

            return commandLimits;
        }

        /// <summary>
        /// Read all the platform objects from the currentReadTarget file
        /// </summary>
        /// <returns></returns>
        public static LinkedList<Platform> readPlatforms()
        {
            const int platformObjSize = 8;
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readPlatforms");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("platforms");

                content = r.ReadElementContentAsString();
                r.Close();
            }

            LinkedList<Platform> platforms = new LinkedList<Platform>();

            String[] PlatformString = CleanUp(content);
            for (int i = 0; i < PlatformString.Length / platformObjSize; i++)
            {
                //create the rectangle for this platform
                int[] rectangleValues = getNumbers(PlatformString[i*platformObjSize].Substring(4));
                Rectangle r = new Rectangle(rectangleValues[0], rectangleValues[1], rectangleValues[2], rectangleValues[3]);

                //determine whether or not this platform has gravitic properties
                bool hasGravity = PlatformString[i*platformObjSize + 1].Equals("yesgravity", StringComparison.CurrentCultureIgnoreCase);

                // check hazard status
                bool isHazardous = PlatformString[i * platformObjSize + 3].Equals("hazardous", StringComparison.CurrentCultureIgnoreCase);
                bool goThrough = PlatformString[i*platformObjSize+6].Equals("through", StringComparison.CurrentCultureIgnoreCase);
                bool isTiled = PlatformString[i*platformObjSize+7].Equals("tile", StringComparison.CurrentCultureIgnoreCase);
                bool isAnimated = PlatformString[i*platformObjSize+4].Contains("hplatform");
                // parse texture
                string texName = "Interactive/" + PlatformString[i * platformObjSize + 4];

                Platform final = new Platform(r, texName, isHazardous, isAnimated, hasGravity, goThrough, isTiled);

                //get the patrol values for this platform (out of parse order because it has to be created after the platform object)
                if (!PlatformString[i * platformObjSize + 2].Equals("nopatrol"))
                {
                    int[] patrolValues = getNumbers(PlatformString[i * platformObjSize + 2].Substring(4));
                    PatrolModel p = new PatrolModel();

                    for (int j = 0; j < patrolValues.Length / 3; j++)
                    {
                        p.addVector(new Vector2((float)patrolValues[j * 3], (float)patrolValues[j * 3 + 1]), patrolValues[j * 3 + 2]);
                    }
                    final.SetPatrol(p);

                }

                // read trigger
                if (!PlatformString[i * platformObjSize + 5].Equals("notrigger"))
                {
                    int [] triggerValues = getNumbers(PlatformString[i*platformObjSize+5].Substring(8));
                    for (int j = 0; j < triggerValues.Length; j+=4)
                    {
                        final.addTrigger(new Trigger(new Rectangle(triggerValues[j],triggerValues[j+1],triggerValues[j+2],triggerValues[j+3]), final));
                    }
                }
                platforms.AddFirst(final);
            }
            return platforms;

        }

        /// <summary>
        /// Read all of the doodads in the present level
        /// </summary>
        /// <returns></returns>
        public static LinkedList<Doodad> readDoodads()
        {
            String content, content2;            
            const int doodadObjSize = 2;
            LinkedList<Doodad> doodads = new LinkedList<Doodad>();
            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("textures");
                r.ReadToDescendant("doodad");

                content2 = r.ReadElementContentAsString();

                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("doodads");

                content = r.ReadElementContentAsString();
                r.Close();
            }
            String [] doodadString = CleanUp(content);
            String[] doodadNames = CleanUp(content2);
            int[] filternums = new int[doodadNames.Length];
            int uniqueTex = 0;
            for (int i = 0; i < filternums.Length; i++)
            {
                filternums[i] = 0;
            }
            for (int i = 0; i < doodadString.Length / doodadObjSize; i++)
            {
                int[] textureNums = getNumbers(doodadString[i * doodadObjSize + 1].Substring(4));
                if (filternums[textureNums[0]] == 0)
                {
                    filternums[textureNums[0]] = 1;
                    uniqueTex++;
                }
            }
            string[] texNames = new string[uniqueTex];
            int counter = 0;
            for (int i = 0; i < filternums.Length; i++)
            {
                if (filternums[i] == 1)
                {
                    texNames[counter] = doodadNames[i];
                    filternums[i] = counter;
                    counter++;
                }
            }
            for (int i = 0; i < doodadString.Length / doodadObjSize; i++)
            {
                int[] rectangleValues = getNumbers(doodadString[i * doodadObjSize].Substring(4));
                Rectangle r = new Rectangle(rectangleValues[0], rectangleValues[1], rectangleValues[2], rectangleValues[3]);
                int[] textureNums = getNumbers(doodadString[i * doodadObjSize+1].Substring(4));
                Doodad final = new Doodad(r, filternums[textureNums[0]], ObjectShape.Rectangle);
                doodads.AddLast(final);
            }
            return doodads;
        }

        /// <summary>
        /// Read all item objects present in the level.
        /// </summary>
        /// <returns></returns>
        public static LinkedList<Item> readItems()
        {
            const int itemLength = 2;

            LinkedList<Item> items = new LinkedList<Item>();
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readItems");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("items");

                content = r.ReadElementContentAsString();
                r.Close();
            }

            String[] ItemString = CleanUp(content);
            for (int i = 0; i < ItemString.Length / itemLength; i++)
            {
                int[] rectangleValues = getNumbers(ItemString[i*itemLength].Substring(4));
                Rectangle r = new Rectangle(rectangleValues[0], rectangleValues[1], rectangleValues[2], rectangleValues[3]);
                
                ItemType type;
                if (ItemString[i * itemLength + 1].Equals("shuriken", StringComparison.CurrentCultureIgnoreCase)) type = ItemType.Shuriken;
                else if (ItemString[i * itemLength + 1].Equals("sword", StringComparison.CurrentCultureIgnoreCase)) type = ItemType.Sword;
                else if (ItemString[i * itemLength + 1].Equals("hookshot", StringComparison.CurrentCultureIgnoreCase)) type = ItemType.Hookshot;

                else
                    throw new Exception("I dunno what you specified in the xml file there but that definitely isn't an item type right now");

                items.AddLast(new Item(new Point(r.X, r.Y), type));
            }

            return items;
        }

        /// <summary>
        /// Read all enemy objects present in the level.
        /// </summary>
        /// <returns></returns>
        public static LinkedList<Enemy> readEnemies()
        {
            int EnemyCount;
            LinkedList<Enemy> enemies = new LinkedList<Enemy>();
            string content;

            if (currentReadTarget == null)
                throw new Exception("File read target not set. LevelParser.readEnemies");

            using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(currentReadTarget))
            {
                r.MoveToContent();
                r.ReadToFollowing("objects");
                r.ReadToDescendant("enemies");

                content = r.ReadElementContentAsString();
                r.Close();
            }

            String[] EnemyString = CleanUp(content);

            EnemyCount = EnemyString.Length / 3;

            for (int i = 0; i < EnemyCount; ++i)
            {
                int[] rectangleValues = getNumbers(EnemyString[i * 3].Substring(4));
                Rectangle r = new Rectangle(rectangleValues[0], rectangleValues[1], rectangleValues[2], rectangleValues[3]);

                //get the patrol values for this enemy
                int[] textureNums = getNumbers(EnemyString[(i * 3) + 2].Substring(4));


                Enemy final = new Enemy(r, textureNums[0], ObjectShape.Rectangle);
                if (!EnemyString[(i * 3) + 1].Equals("nopatrol"))
                {
                    int[] patrolValues = getNumbers(EnemyString[(i * 3) + 1].Substring(4));
                    PatrolModel p = new PatrolModel();

                    for (int j = 0; j < patrolValues.Length / 3; j++)
                    {
                        p.addVector(new Vector2((float)patrolValues[j * 3], (float)patrolValues[j * 3 + 1]), patrolValues[j * 3 + 2]);
                    }
                    final.setPatrolModel(p);

                }

                enemies.AddLast(final);
            }

            return enemies;
        }

        

        #region Helpers

        /// <summary>
        /// Makes the XML syntax less of an unreadable mess in terms of
        /// string characters
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        private static String[] CleanUp(String information)
        {
            String[] delimiter = new String[] { "\n        " };
            information = information.Trim();
            String[] stuff = information.Split(delimiter, StringSplitOptions.None);
            return stuff;
        }

        /// <summary>
        /// returns an array of numbers from a string, as
        /// seperated by a comma
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        private static int[] getNumbers(String information)
        {
            String[] a = information.Split(',');
            int[] ret = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                ret[i] = Convert.ToInt32(a[i]);
            }

            return ret;

        }

        /// <summary>
        /// Returns a string array containing each folder of the path
        /// element [0] = root of folder, usually "content"
        /// element [1] = dev, story, or user folder
        /// element [2] = chapter number
        /// element [3] = filename of the current target
        /// </summary>
        /// <returns></returns>
        private static string[] getPathComponents()
        {
            string[] delimiter = new string[] { "/" };
            string[] stuff = currentReadTarget.Split(delimiter, StringSplitOptions.None);
            return stuff;

        }

        #endregion


    }
}
