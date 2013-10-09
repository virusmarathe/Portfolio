using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PsychicNinja.Interface.Engine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using PsychicNinja.Data.Util;
using PsychicNinja.Logic;

namespace PsychicNinja
{
    public class PauseMenu : GameMenu
    {
        private static Rectangle position = new Rectangle(0, 0, 0, 0);

        private static Texture2D pauseScreenTexture;
        private static Texture2D muteOnTexture;
        private static Texture2D muteOffTexture;
        private static Texture2D resumeTexture;
        private static Texture2D nextLevelTexture;
        private static Texture2D titleScreenTexture;
        private static Texture2D restartTex;
        private static Texture2D pauseLabelTexture;

        private static View pauseLabel;

        private Button mute;
        private Button resume;
        private Button nextLevel;
        private Button toTitleScreen;
        private Button restart;

        public bool tempLoadDevLevelFlag;
        

        public PauseMenu()
        {
            SetDrawFrame(position);
            drawTex = pauseScreenTexture;

            mute = new Button(new Rectangle(0, 0, 0, 0), muteOffTexture);
            resume = new Button(new Rectangle(0, 0, 0, 0), resumeTexture);
            restart = new Button(new Rectangle(0, 0, 0, 0), restartTex);
            nextLevel = new Button(new Rectangle(0, 0, 0, 0), nextLevelTexture);
            toTitleScreen = new Button(new Rectangle(0, 0, 0, 0), titleScreenTexture);
            pauseLabel = new View(new Rectangle(0, 0, 0, 50), pauseLabelTexture);

            this.animatedExpand(new Rectangle(0, 0, 800, 480), 15, false);

            mute.animatedExpand(new Rectangle(500, 180, 64, 64), 15, false);
            resume.animatedExpand(new Rectangle(0, 280, 256, 50), 15, false);
            restart.animatedExpand(new Rectangle(0, 350, 286, 50), 15, false);
            nextLevel.animatedExpand(new Rectangle(400, 280, 200, 100), 15, false);
            toTitleScreen.animatedExpand(new Rectangle(0, 420, 500, 50), 15, false);
            pauseLabel.animatedExpand(new Rectangle(0, 0, 800, 50), 15, false);

            flag = GameMenuFlag.NoFlag;
        }

        public static new void LoadContent(ContentManager Content)
        {
            muteOnTexture = Content.Load<Texture2D>("soundoff");
            muteOffTexture = Content.Load<Texture2D>("soundon");
            pauseScreenTexture = Content.Load<Texture2D>("levelcompletebg");
            resumeTexture = Content.Load<Texture2D>("resumebutton");
            nextLevelTexture = Content.Load<Texture2D>("nextlevel");
            titleScreenTexture = Content.Load<Texture2D>("ToTitlescreen2");
            restartTex = Content.Load<Texture2D>("restart");
            pauseLabelTexture = Content.Load<Texture2D>("pauselabel");
        }

        public override void Update() 
        {
            base.Update();

            if (MusicManager.soundEnabled)
                mute.drawTex = muteOffTexture;
            else
                mute.drawTex = muteOnTexture;
            mute.Update();
            resume.Update();
            nextLevel.Update();
            toTitleScreen.Update();
            restart.Update();
            pauseLabel.Update();
            //slick animations here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            mute.Draw(spriteBatch);
            resume.Draw(spriteBatch);
            nextLevel.Draw(spriteBatch);
            toTitleScreen.Draw(spriteBatch);
            restart.Draw(spriteBatch);
            pauseLabel.Draw(spriteBatch);
        }

        public override bool ProcessTouch(GestureSample gesture)
        {
            if (mute.RespondsToGesture(gesture))
            {
                MusicManager.StopMusic();
                MusicManager.soundEnabled = !MusicManager.soundEnabled;
                if (MusicManager.soundEnabled)
                    MusicManager.StartMusic();

            }
            else if (resume.RespondsToGesture(gesture))
            {
                flag = GameMenuFlag.Replay;
            }
            //if (base.ProcessTouch(gesture))

            else if (nextLevel.RespondsToGesture(gesture))
            {
                flag = GameMenuFlag.LoadNextLevel;
            }
            else if (toTitleScreen.RespondsToGesture(gesture))
            {
                flag = GameMenuFlag.ToTitleScreen;
            }
            else if (restart.RespondsToGesture(gesture))
            {
                flag = GameMenuFlag.LoadSpecificLevel;
            }

            return flag != GameMenuFlag.NoFlag;
        }
        
    }
}
