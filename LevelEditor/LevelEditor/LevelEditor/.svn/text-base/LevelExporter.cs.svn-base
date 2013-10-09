using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelEditor.WorldObjects;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace LevelEditor
{
    class LevelExporter
    {
        int minX, minY, maxX, maxY;
        const int NUM_PLATFORM_TEX = 41;
        const int NUM_HPLATFORM_TEX = 25;
        const int NUM_DOODAD_TEX = 28;
        string[] platformTex;

        public LevelExporter()
        {
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;
            platformTex = new string[NUM_PLATFORM_TEX+NUM_HPLATFORM_TEX];
            for (int i = 0; i <= NUM_PLATFORM_TEX; i++)
            {
                platformTex[i] = "        platform"+(i+1);
            }
            for (int i = 0; i < NUM_HPLATFORM_TEX; i++)
            {
                platformTex[i+NUM_PLATFORM_TEX] = "        hplatform" + (i + 1);
            }
        }
        public void sendObjects(LinkedList<WorldObject> objects, int[] commandNums, String fileName, int xAdd, Rectangle bounds, int backgroundType)
        {

            LinkedList<String> platforms = new LinkedList<String>();
            LinkedList<String> enemies = new LinkedList<String>();
            LinkedList<String> commands = new LinkedList<String>();
            LinkedList<String> items = new LinkedList<String>();
            LinkedList<String> doodads = new LinkedList<String>();
            LinkedList<String> ropes = new LinkedList<String>();

            String[] commandTypes = { "        ML_Lim", "        MR_Lim", "        Ju_Lim", "        WJ_Lim", "        WS_Lim", "        WC_Lim", "        UI_Lim" };
            String startLoc = "";
            String goalLoc = "";
            String [] platformTexFilter = new String[platformTex.Length];
            String music = "";
            String background = "";

            for (int i = 0; i < commandNums.Length; i++)
            {
                commands.AddLast(commandTypes[i] + " " + commandNums[i]);
            }

            switch (backgroundType)
            {
                case 1:
                    music = "        Music/Synchro Ninja";
                    break;
                case 2:
                    music = "        Music/CaveStory1";
                    break;
                case 3:
                    music = "";
                    break;
                default:
                    music = "";
                    break;
            }


            // before doing everything need to filter things

            foreach (WorldObject o in objects)
            {
                int texNum = o.textNum + 1;
                String grav;
                String through;
                String tile;
                if (o.hasGravity) grav = "yesgravity";
                else grav = "nogravity";
                if (o.goThrough) through = "through";
                else through = "nothrough";
                if (o.isTiled) tile = "tile";
                else tile = "notile";

                switch (o.type)
                {
                    case Util.ObjectType.Platform:
                        if (o.numPoints <= 1)
                        {
                            String s2="";
                            if (o.triggers.Count >= 1)
                            {
                                s2 = "        trigger ";
                                foreach (Trigger t in o.triggers)
                                {
                                    s2 += (t.drawRect.X - minX) + "," + (t.drawRect.Y - minY) + "," + t.drawRect.Width + "," + t.drawRect.Height + ",";
                                }
                                s2 = s2.Substring(0, s2.Length - 1);
                            }
                            else
                            {
                                s2 = "        notrigger";
                            }
                            platforms.AddLast("        rec " + (o.drawRect.X-minX) + "," + (o.drawRect.Y-minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                                + "\n        " + grav + "\n        nopatrol\n        nohazardous\n        platform" + texNum + "\n"+s2+"\n        "+through+"\n        "+tile);
                        }
                        else
                        {
                            String s = "        ptl ";
                            String s2 = "";
                            foreach (PatrolPoint p in o.points)
                            {
                                s += p.x + "," + p.y + "," + p.time + ",";
                            }
                            s = s.Substring(0, s.Length - 1);

                            if (o.triggers.Count >= 1)
                            {
                                s2 = "        trigger ";
                                foreach (Trigger t in o.triggers)
                                {
                                    s2 += (t.drawRect.X - minX) + "," + (t.drawRect.Y- minY) + "," + t.drawRect.Width + "," + t.drawRect.Height + ",";
                                }
                                s2 = s2.Substring(0, s2.Length - 1);
                            }
                            else {
                                s2 = "        notrigger";
                            }

                            platforms.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                            + "\n        " + grav + "\n" + s + "\n        nohazardous\n        platform" + texNum + "\n" + s2 + "\n        " + through + "\n        " + tile);
                        }
                        break;
                    case Util.ObjectType.Enemy:
                        if (o.numPoints == 1)
                        {
                            enemies.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                                + "\n        nopatrol\n        tex " + o.textNum);
                        }
                        else
                        {
                            String s = "        ptl ";
                            foreach (PatrolPoint p in o.points)
                            {
                                s += p.x + "," + p.y + "," + p.time + ",";
                            }
                            s = s.Substring(0, s.Length - 1);
                            enemies.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                            + "\n" + s + "\n        tex "+o.textNum);
                        }
                        break;
                    case Util.ObjectType.Start:
                        startLoc = (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY);
                        break;
                    case Util.ObjectType.Goal:
                        goalLoc = (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height;
                        break;
                    case Util.ObjectType.Shuriken:
                        items.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                            + "\n        shuriken");
                        break;
                    case Util.ObjectType.Sword:
                        items.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                            + "\n        sword");
                        break;
                    case Util.ObjectType.Doodad:
                        doodads.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                                + "\n        tex " + o.textNum);
                        break;
                    case Util.ObjectType.HazardPlatform:
                        if (o.numPoints <= 1)
                        {
                            String s2 = "";
                            if (o.triggers.Count >= 1)
                            {
                                s2 = "        trigger ";
                                foreach (Trigger t in o.triggers)
                                {
                                    s2 += (t.drawRect.X - minX) + "," + (t.drawRect.Y - minY) + "," + t.drawRect.Width + "," + t.drawRect.Height + ",";
                                }
                                s2 = s2.Substring(0, s2.Length - 1);
                            }
                            else
                            {
                                s2 = "        notrigger";
                            }
                            platforms.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                                + "\n        " + grav + "\n        nopatrol\n        hazardous\n        hplatform" + texNum + "\n" + s2 + "\n        " + through + "\n        " + tile);
                        }
                        else
                        {
                            String s = "        ptl ";
                            String s2 = "";
                            foreach (PatrolPoint p in o.points)
                            {
                                s += p.x + "," + p.y + "," + p.time + ",";
                            }
                            s = s.Substring(0, s.Length - 1);
                            if (o.triggers.Count >= 1)
                            {
                                s2 = "        trigger ";
                                foreach (Trigger t in o.triggers)
                                {
                                    s2 += (t.drawRect.X - minX) + "," + (t.drawRect.Y - minY) + "," + t.drawRect.Width + "," + t.drawRect.Height + ",";
                                }
                                s2 = s2.Substring(0, s2.Length - 1);
                            }
                            else {
                                s2 = "        notrigger";
                            }
                                platforms.AddLast("        rec " + (o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height
                                                + "\n        " + grav + "\n" + s + "\n        hazardous\n        hplatform" + texNum + "\n" + s2 + "\n        " + through + "\n        " + tile);
                        }
                        break;
                    case Util.ObjectType.Rope:
                        ropes.AddLast((o.drawRect.X - minX) + "," + (o.drawRect.Y - minY) + "," + o.drawRect.Width + "," + o.drawRect.Height);
                        break;
                    default: break;
                }
            }
//            using (System.IO.StreamWriter w = new System.IO.StreamWriter("../../../../../../PsychicNinja/PsychicNinja/PsychicNinjaContent/Level/Development/ch1/3.xml"))
            int width = maxX - minX;
            int height = maxY - minY;

            using (System.IO.StreamWriter w = new System.IO.StreamWriter(fileName))  
            {
                w.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                w.WriteLine("<XnaContent>");
                w.WriteLine(" 	<Asset Type=\"System.String\">");

                #region <levelinfo>
                w.WriteLine("    <levelinfo>");
                w.WriteLine("      <levelname>");
                //level name goes in this part
                //w.WriteLine("        levelname");
                w.WriteLine("      </levelname>");
                w.WriteLine("      <bounds>");
                w.WriteLine("        "+width+","+height);
                w.WriteLine("      </bounds>");
                //music goes in this part
                w.WriteLine("      <music>");
                w.WriteLine(music);
                w.WriteLine("        Music/City-Active");
                w.WriteLine("      </music>");
                w.WriteLine("      <tileset>");
                w.WriteLine("        city-day");
                w.WriteLine("      </tileset>");
                w.WriteLine("    </levelinfo>");
                #endregion
                #region <textures>
                w.WriteLine("    <textures>");
                w.WriteLine("      <background>");

                //replace this
                if (backgroundType == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        w.WriteLine("        backlayer" + i);
                    }
                }
                else if (backgroundType == 2)
                {
                    w.WriteLine("        bgcave1");
                    w.WriteLine("        bgcave3");
                    w.WriteLine("        cavebacklayerscrolling");
                    w.WriteLine("        cavebacklayer3");
                }
                w.WriteLine("      </background>");

                w.WriteLine("      <enemy>");
                //write enemy textures with for loop

                //for something
                w.WriteLine("        Enemies/goonMoveLeft1");
                w.WriteLine("        Enemies/knife artist 1");
                w.WriteLine("        Enemies/rat1");
                w.WriteLine("        Enemies/bat1");

                w.WriteLine("      </enemy>");

                w.WriteLine("      <platform>");
                for (int i = 0; i < platformTex.Length; i++)
                {
                    w.WriteLine(platformTex[i]);
                }

                w.WriteLine("      </platform>");
                w.WriteLine("      <doodad>");
                w.WriteLine("        treeoflife");
                w.WriteLine("        stick");
                w.WriteLine("        lolampnight");
                w.WriteLine("        hydralisklight");
                w.WriteLine("        doodad5");
                w.WriteLine("        doodad6");
                for (int i = 7; i < NUM_DOODAD_TEX; i++)
                {
                    w.WriteLine("        doodad" + i);
                }               
                w.WriteLine("      </doodad>");

                w.WriteLine("    </textures>");
                #endregion
                #region <objects>
                w.WriteLine("    <objects>");
                w.WriteLine("      <startlocation>");
                //write the start location here
                //for something
                w.WriteLine("        " + startLoc);
                w.WriteLine("      </startlocation>");


                w.WriteLine("      <goalregion>");
                w.WriteLine("        " + goalLoc);
                w.WriteLine("      </goalregion>");


                w.WriteLine("      <commands>");
                //write the command limits here
                //for something

                foreach (String s in commands)
                {
                    w.WriteLine(s);
                }

                w.WriteLine("      </commands>");


                w.WriteLine("      <platforms>");
                //write the platform structure here
                //for something
                foreach (String s in platforms)
                {
                    w.WriteLine(s);
                }
                //w.WriteLine("        rec c1,c2,c3,c4");
                //w.WriteLine("        nogravity or yesgravity");
                //w.WriteLine("        nopatrol or yespatrol");
                //w.WriteLine("        tex texturenumber");
                w.WriteLine("      </platforms>");
                w.WriteLine("      <doodads>");
                foreach (String s in doodads)
                {
                    w.WriteLine(s);
                }
                //w.WriteLine("        rec c1,c2,c3,c4");
                //w.WriteLine("        tex texturenumber");
                w.WriteLine("      </doodads>");

                w.WriteLine("      <enemies>");
                //write the enemy structure here
                //for something
                foreach (String s in enemies)
                {
                    w.WriteLine(s);
                }
                //w.WriteLine("        rec c1,c2,c3,c4");
                //w.WriteLine("        nopatrol or ptl x1vel,y1vel,time1,x2vel,y2vel,time2,x3vel,y3vel,time3,.......xnvel,ynvel,timen);
                //w.WriteLine("        tex texturenumber");
                w.WriteLine("      </enemies>");


                w.WriteLine("      <items>");
                //write the items structure here
                //for something
                //w.WriteLine("        rec c1,c2,c3,c4");
                //w.WriteLine("        shuriken or sword or hookshot");
                foreach (String s in items)
                {
                    w.WriteLine(s);
                }
                w.WriteLine("      </items>");
                //ropes
                w.WriteLine("      <ropes>");
                //write the items structure here
                //for something
                //w.WriteLine("        rec c1,c2,c3,c4");
                //w.WriteLine("        shuriken or sword or hookshot");
                foreach (String s in ropes)
                {
                    w.WriteLine("        " + s);
                }
                w.WriteLine("      </ropes>");


                w.WriteLine("    </objects>");
                #endregion

                w.WriteLine("	</Asset>");
                w.WriteLine("</XnaContent>");
                w.Close();
            }
        }
        public void setBounds(int x, int y, int w, int h)
        {
            minX = x;
            minY = y;
            maxX = w+minX;
            maxY = h+minY;
        }
        public void checkBounds(int x, int y, int w, int h)
        {
            if (x < minX)
            {
                minX = x;
            }
            else if (x+w > maxX)
            {
                maxX = x+w;
            }
            if (y < minY)
            {
                minY = y;
            }
            else if (y+h > maxY)
            {
                maxY = y+h;
            }

        }
    }
}
