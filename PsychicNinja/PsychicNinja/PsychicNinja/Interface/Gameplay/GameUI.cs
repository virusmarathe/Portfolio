using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using PsychicNinja.Data.Util;
using PsychicNinja.Interface.Engine;
using PsychicNinja.Data.Parser;
using PsychicNinja.Data.Object;


namespace PsychicNinja.Interface.Gameplay
{
    /// <summary>
    /// This class is responsible for managing the random buttons that display over the game world outside of the command menu.
    /// </summary>
    public class GameUI
    {
        private Button playGame;
        private Button stopGame;
        private Button pauseGame;
        private Button fastForward;

        // This needs to contain exactly the next two rectangles
        private static Rectangle RunningOptionRectContainer = new Rectangle(330, 410, 140, 70);
        private static Rectangle CenterLeftOptionRect = new Rectangle(0, 380, 100, 100);
        private static Rectangle CenterRightOptionRect = new Rectangle(400, 410, 70, 70);
        private static Rectangle TopLeftOptionRect = new Rectangle(10, 10, 70, 70);

        private static Rectangle TutorialButtonTopRight = new Rectangle(630, 15, 70, 70);

        private static Rectangle FastForwardButton = new Rectangle(110, 410, 70, 70);

        private static Texture2D startButtonTexture;
        private static Texture2D stopButtonTexture;
        private static Texture2D pauseButtonTexture;
        private static Texture2D fastForwardButtonTexture;

        public GameUI()
        {
            playGame = new Button(CenterLeftOptionRect, startButtonTexture);
            stopGame = new Button(CenterLeftOptionRect, stopButtonTexture);
            pauseGame = new Button(TopLeftOptionRect, pauseButtonTexture);
            fastForward = new Button(FastForwardButton, fastForwardButtonTexture);
        }

        /// <summary>
        /// Loads static content for this class. This is textures that are always in use and never change.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(ContentManager Content)
        {
            startButtonTexture = Content.Load<Texture2D>("buttgreen");
            stopButtonTexture = Content.Load<Texture2D>("buttred");
            pauseButtonTexture = Content.Load<Texture2D>("buttpause");
            fastForwardButtonTexture = Content.Load<Texture2D>("buttplay");
        }

        /// <summary>
        /// Update the buttons in this menu.
        /// </summary>
        /// <param name="state">Game state to update with it.</param>
        /// <param name="selectedRef">Ref to the command that is selected. Pass null if no command is selected so the button doesn't show.</param>
        public void Update(GameState state, Command selectedRef)
        {
            switch (state)
            {
                case GameState.StateWorld:
                    playGame.hidden = false;
                    pauseGame.hidden = false;
                    stopGame.hidden = true;
                    fastForward.hidden = true;
                    break;
                case GameState.StateRunning:
                    playGame.hidden = true;
                    stopGame.hidden = false;
                    pauseGame.hidden = false;
                    fastForward.hidden = false;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playGame.Draw(spriteBatch);
            pauseGame.Draw(spriteBatch);
            stopGame.Draw(spriteBatch);
            fastForward.Draw(spriteBatch);
        }

        /// <summary>
        /// Conditionally handle a touch input and return and indication of what happened.
        /// </summary>
        /// <param name="gesture"></param>
        /// <returns>An action if a button was pressed. None if nothing was touched.</returns>
        public GameButtonPressed ProcessInput(GestureSample gesture)
        {
            if (playGame.ProcessTouch(gesture))
            {
                return GameButtonPressed.StartGame;
            }
            else if (stopGame.ProcessTouch(gesture))
            {
                return GameButtonPressed.StopGame;
            }
            else if (pauseGame.ProcessTouch(gesture))
            {
                return GameButtonPressed.PauseGame;
            }
            else if (fastForward.ProcessTouch(gesture))
            {
                return GameButtonPressed.FastForward;
            } 
            else
            {
                return GameButtonPressed.None;
            }
        }
    }
}
