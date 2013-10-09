using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PsychicNinja.Data.Parser;

namespace PsychicNinja.Data.Object
{
    public class Dialogue:WorldObject
    {
        private static Texture2D dialogueTex;
        const int NUM_TUTORIAL_LEVELS = 11;
        Dictionary<int, string[]> dList = new Dictionary<int, string[]>();
        private string[] genericDialogues = {"If I wanted to kill myself, I would have become\na Samurai!",
                                          "Oh no! I'm dead! Now I'll have to move icons\naround! Oh, the inconvenience!",
                                          "The only way I could fail more miserably would\nbe if I wrote stupid quotes for a suicidal ninja",
                                          "I have three eyes, you know. THREE! And yet I\ncouldn't see that. Maybe I need glasses.",
                                          "Hello player! Are you tired of hearing these\nquotes yet? How about now?",
                                          "I got distracted by those awesome clouds!",
                                          "Man, I didn't know I had that many bones!",
                                          "Ooh, that doesn't look like it'd be comfortable.\nMaybe I should try something else?",
                                          "As a Psychic Ninja, I want to NOT die",
                                          "Come on, that hazard was at least 40x40 and had\nat least 32 bits of color precision!",
                                          "They spent nine months slaving away on my game\nand that's the best you have to show?",
                                          "I think they they put more effort into modelling\nmy gore than they did into collision detection",
                                          "It's brilliant! The color of my insides, I\nmean. Although I suppose I do see them a lot!",
                                          "I'm a psychic ninja, not Superman, although\nI'm sure kryptonite would kill me anyways",
                                          "And mamma always told me to look both ways\nbefore pressing the \"go\" button (not really)",
                                          "*eyeball-rollingly lame quip*",
                                          "That almost works except for the part where I\ndie horribly",
                                          "Well, okay, I die if I do that, but at least\nI take the framerate down with me!",
                                          "GAME STUPID. I QUIT",
                                          "You are really discrediting the whole \"Ninjas\nare better than pirates\" argument...\n",
                                          "Yeah! Almost there! Go me, you can do i--\nOh wait, I die there. Rats!"
                                         };
        // level 1 specific quotes
        private string[] levelDialogue1 = {"Well I don't want to JUMP to any conclusions...",
                                          "I should try placing commands to plan out my path!",
                                          "Maybe hitting that jump command will lead me to\nvictory!",
                                          "Pressing the play button makes me GO!",
                                          "The fast forward button makes me GO faster...",
                                          };
        private string[] levelDialogue2 = {"Sometimes I have to change directions!"

                                          };
        private string[] levelDialogue3 = {"It seems those red girders like to move!"
                                          };
        private string[] levelDialogue4 = {"I should use wall jump to get to higher ground!"
                                          };
        private string[] levelDialogue5 = {"That sure is a long fall!"
                                          };
        private string[] levelDialogue6 = {"I need something to kill this goon!"
                                          };
        private string[] levelDialogue7 = {"Maybe I can use the sword in another way!"
                                          };
        private string[] levelDialogue8 = {"Double tap to zoom out the level!"
                                          };
        private string[] levelDialogue9 = {"I may want to re-use this shuriken!",
                                              "This shuriken will make short work of \nthese goons!"
                                          };
        private string[] levelDialogue10 = {"I bet this shuriken can cut ropes!"
                                          };
        private string[] levelDialogue11 = {"I should ledge climb to get that extra\nboost!"};


        private string dialogue;
        private int numDialogues;
        private static SpriteFont dialogueFont;
        private bool show;
        private int moveDialogue;
        public int dialogueNumber;
        private int showTime = 0;

        public Dialogue(Rectangle rect, int num) :
            base(rect, dialogueTex)
        {
            dialogueNumber = num;
//            dialogue = dialogueList[dialogueNumber,Game1.RapperRandomDiggityDawg.Next(0, numDialogues-1)];
            moveDialogue = -100;
            if (LevelParser.getCurrentChapterNumber() == 1 || (LevelParser.getCurrentChapterNumber() ==2 && num == 1))
                show = true;
            dList.Add(0, genericDialogues);
            dList.Add(1, levelDialogue1);
            dList.Add(2, levelDialogue2);
            dList.Add(3, levelDialogue3);
            dList.Add(4, levelDialogue4);
            dList.Add(5, levelDialogue5);
            dList.Add(6, levelDialogue6);
            dList.Add(7, levelDialogue7);
            dList.Add(8, levelDialogue8);
            dList.Add(9, levelDialogue9);
            dList.Add(10, levelDialogue10);
            dList.Add(11, levelDialogue11);

            if (dialogueNumber >= NUM_TUTORIAL_LEVELS || LevelParser.getCurrentChapterNumber() > 1)
            {
                dialogueNumber = 0;
            }
            if (LevelParser.getCurrentChapterNumber() == 2 && num == 1)
            {
                dialogueNumber = 11;
            }
            numDialogues = dList[dialogueNumber].Length;
            dialogue = dList[dialogueNumber][Game1.RapperRandomDiggityDawg.Next(0, numDialogues-1)];
        }

        public override void Update(int timeElapsed)
        {
            if (show)
            {
                showTime++;
                if (moveDialogue >= 0)
                {
                    moveDialogue = 0;
                }
                else
                {
                    moveDialogue += 5;
                }
                if (showTime > 100)
                {
                    show = false;
                    showTime = 0;
                }
                this.SetDrawFrame(new Rectangle(100, moveDialogue, 650, 100));
            }
            else
            {
                if (moveDialogue <= -100)
                {
                    moveDialogue = -100;
                    numDialogues = dList[dialogueNumber].Length;
                    dialogue = dList[dialogueNumber][Game1.RapperRandomDiggityDawg.Next(0, numDialogues - 1)];
                }
                else
                {
                    moveDialogue -= 5;
                }
                showTime = 0;
                //dialogue = dialogueList[dialogueNumber,Game1.RapperRandomDiggityDawg.Next(0, numDialogues - 1)];
                this.SetDrawFrame(new Rectangle(100, moveDialogue, 650, 100));
            }
            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (moveDialogue != -100)
            {
                spriteBatch.Draw(dialogueTex, drawRect, Color.White);
                spriteBatch.DrawString(dialogueFont, dialogue, new Vector2(drawRect.X + 150, drawRect.Y + 25), Color.Black);
            }
            //base.Draw(spriteBatch);
        }

        public static new void LoadContent(ContentManager Content) 
        {
            dialogueTex = Content.Load<Texture2D>("thoughtbubble");
            dialogueFont = Content.Load<SpriteFont>("dialogueFont");
        }

        public void setDialogue(string s)
        {
            dialogue = s;
        }

        public string getDialogue()
        {
            return dialogue;
        }
        public void showDialog(bool b)
        {
            show = b;
        }
        public void setDialogueNumber(int n)
        {
            if (dialogueNumber >= NUM_TUTORIAL_LEVELS || LevelParser.getCurrentChapterNumber() > 0)
            {
                dialogueNumber = 0;
            }
            else if (LevelParser.getCurrentChapterNumber() == 2 && n == 1)
            {
                dialogueNumber = 11;
            }
            else
            {
                dialogueNumber = n;
            }
        }
        public int getDialogueNumber()
        {
            return dialogueNumber;
        }
    }
}
