//
//
//
//  @ Project : Untitled
//  @ File Name : CommandMenu.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//

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
//using PsychicNinja.Interface.Command;

namespace PsychicNinja.Interface
{
    /// <summary>
    /// The Sidebar menu that holds and allows selection of commands for the game.
    /// </summary>
    public class CommandMenu : View
    {
        public LinkedList<Command> listOfWorldCommands = new LinkedList<Command>();

        private static Rectangle mainRect = new Rectangle(656, 0, 150, 480);

        private Button toggle;

        private LinkedList<CommandButton> commandButtons;
        /// <summary>
        /// Font used to draw the number of commands remaining; only contains digits from 0-9 as well as 8
        /// </summary>
        public static SpriteFont counterfont;

        private int[] commandlimits;

        #region Textures
        private static Texture2D backgroundTexture;
        private static Texture2D buttonMask;

        private static Texture2D showMenuTexture;
        private static Texture2D hideMenuTexture;

        #endregion

        public bool show = true;

        enum CommandMenuButtonScrollAnimationType
        {
            None = 0,
            ToTop = 1,
            ToBottom = -1,
        }

        CommandMenuButtonScrollAnimationType currentAnimationType;

        #region Menu Button Scrolling

        int MenuButtonVerticalOffset = 0;
        int MENU_BUTTON_OFFSET_MIN = -370;
        int MENU_BUTTON_OFFSET_MAX = 0;

        /// <summary>
        /// Add an amount to the command menu button vertical offsets.
        /// </summary>
        /// <param name="off"></param>
        private void OffsetMenuButtons(int off)
        {
            MenuButtonVerticalOffset += off;
            if (MenuButtonVerticalOffset < MENU_BUTTON_OFFSET_MIN)
            {
                MenuButtonVerticalOffset = MENU_BUTTON_OFFSET_MIN;
                currentAnimationType = CommandMenuButtonScrollAnimationType.None;
            }
            if (MenuButtonVerticalOffset > MENU_BUTTON_OFFSET_MAX)
            {
                MenuButtonVerticalOffset = MENU_BUTTON_OFFSET_MAX;
                currentAnimationType = CommandMenuButtonScrollAnimationType.None;
            }

        }

        public void UpdateMenuItemScrollAnimation()
        {
            if (currentAnimationType == CommandMenuButtonScrollAnimationType.None) return;

            OffsetMenuButtons(15 * (int)currentAnimationType);
        }

        #endregion

        int afterTouchDelay = 0;

        #region Initialization

        /// <summary>
        /// Called to initialize a menu with only the given commands and display the command at the given index at the top of the scroll list. 
        /// </summary>
        /// <param name="Commands">List of all commands available in the menu.</param>
        /// <param name="DisplayStart">Index of the first command to show in the menu.</param>
        public CommandMenu(int[] limits) : base(mainRect, backgroundTexture)
        {
            Point p = new Point(0, 0);

            commandlimits = limits;

            commandButtons = new LinkedList<CommandButton>();

            //realcounter is used to make sure no blank slots are added to the
            //commandButtons list
            int realcounter = 0;

            for (int i = 0; i < (int)CommandType.NumCommands; ++i)
            {

                if (commandlimits[i] != 0)
                {
                    CommandButton b = new CommandButton(new Rectangle(665, 110 + 95 * realcounter, 120, 80));
                    b.setData(new Command(p, (CommandType)i));
                    commandButtons.AddLast(b);
                    realcounter++;
                }
            }

            if (realcounter < 4)
                MENU_BUTTON_OFFSET_MIN = 0;
            else
                MENU_BUTTON_OFFSET_MIN = -(95 * (realcounter-3));

            toggle = new Button(new Rectangle(560, 380, 100, 100), showMenuTexture);
            toggle.SetDrawTexture(hideMenuTexture);

        }

        /// <summary>
        /// Pre-load necessary textures for this class.
        /// </summary>
        /// <param name="Content"></param>
        public static new void LoadContent(ContentManager Content)
        {
            backgroundTexture = Content.Load<Texture2D>("sidebarbeta");
            buttonMask = Content.Load<Texture2D>("CommandButtonBoundryMask");
            showMenuTexture = Content.Load<Texture2D>("sidetab1");
            hideMenuTexture = Content.Load<Texture2D>("sidetab2");
            counterfont = Content.Load<SpriteFont>("counterfont");
        }

        #endregion

        #region Life Cycle

        public void UpdateAnimations(int timeElapsed)
        {
            //Animate the commands
            foreach (Command c in listOfWorldCommands)
                c.Update(timeElapsed);
        }

        /// <summary>
        /// Called to make the menu test for button presses and react accordingly.
        /// </summary>
        public override void Update()
        {
            base.Update();

            toggle.Update();

            foreach (CommandButton b in commandButtons)
                b.Update();

            if (afterTouchDelay != 0)
            {
                afterTouchDelay++;
                if (afterTouchDelay > 15)
                    afterTouchDelay = 0;
            }

            UpdateMenuItemScrollAnimation();
        }

        /// <summary>
        /// Draws the Command Menu. Uses base.Draw to draw the background.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw this with.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            toggle.Draw(spriteBatch);

            base.Draw(spriteBatch);

            if (hidden) return;

            foreach (Button b in commandButtons)
            {
                int numCommandsRemaining = commandlimits[(int)(((CommandButton)b).getData().getCommandType())];
                Rectangle buttonDrawRect = b.drawRect;

                Color drawColor = (numCommandsRemaining == 0)? Color.Gray : Color.White;

                spriteBatch.Draw(b.drawTex, new Rectangle(buttonDrawRect.Left, buttonDrawRect.Top + MenuButtonVerticalOffset, buttonDrawRect.Width, buttonDrawRect.Height), drawColor);

                string remainingString = (numCommandsRemaining == -1)? "∞" : numCommandsRemaining.ToString();

                spriteBatch.DrawString(counterfont, remainingString, new Vector2(buttonDrawRect.Left, buttonDrawRect.Top + MenuButtonVerticalOffset + 50), Color.Black);
                
            }


            spriteBatch.Draw(buttonMask, new Rectangle(drawRect.Left+3, drawRect.Top, 141, buttonMask.Height), Color.White);
            spriteBatch.Draw(buttonMask, new Rectangle(drawRect.Left+3, (drawRect.Top + drawRect.Height) - buttonMask.Height, 141, buttonMask.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                        


            View fr = View.FirstResponder();
            if (fr != null && fr is Command)
                spriteBatch.Draw(fr.drawTex, Spotlight.TranslateWorldRectangleToScreenRectangle(fr.drawRect), Color.White);

            #region Andrew rage section in comments
            /*
            if (speciallyDrawnCommand != null)
                //spriteBatch.Draw(speciallyDrawnCommand.drawTex,
                //    Spotlight.TranslateWorldVectorToScreenVector(speciallyDrawnCommand.Position()), null, Color.White, 0, Vector2.Zero, 1f, speciallyDrawnCommand.FacesLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                spriteBatch.Draw(speciallyDrawnCommand.drawTex,
                    Spotlight.TranslateWorldRectangleToScreenRectangle(speciallyDrawnCommand.drawRect), null, Color.White, 0, Vector2.Zero, speciallyDrawnCommand.FacesLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
             */
            #endregion

        }

        public void DrawCommands(SpriteBatch spriteBatch)
        {
            foreach (Command c in listOfWorldCommands)
                if (!c.IsFirstResponder())
                    c.Draw(spriteBatch);
        }

        #endregion
       
        /// <summary>
        /// Called to make the menu change to display state.
        /// </summary>
        public void Show()
        {
            if (show) return;

            show = true;
            toggle.SetDrawTexture(hideMenuTexture);
            MenuAnimatedHorizontalSlide(-150);
        }

        /// <summary>
        /// Called to make the menu chage to hidden state.
        /// </summary>
        public void Hide()
        {
            if (!show) return;

            show = false;
            toggle.SetDrawTexture(showMenuTexture);
            MenuAnimatedHorizontalSlide(150);
        }

        public void MenuAnimatedHorizontalSlide(int off)
        {
            foreach (CommandButton cb in commandButtons)
                cb.animatedFrameSlide(new Point(off, 0), 15, off>0? true: false);

            animatedFrameSlide(new Point(off, 0), 15, off > 0 ? true : false);
            toggle.animatedFrameSlide(new Point(off, 0), 15, off > 0 ? true : false);
        }

        #region Touch Handling

        private CommandType selectedCommand = CommandType.NumCommands;

        /// <summary>
        /// Handles the touch gesture. Returns a response based on what we did with the touch.
        /// </summary>
        /// <param name="gesture"></param>
        /// <returns>Whether or not this object handled the touch.</returns>
        public bool ProcessTouch(GestureSample gesture, LinkedList<Platform> listOfWorldPlatforms)
        {
            Point hit = new Point((int)gesture.Position.X, (int)gesture.Position.Y);
            Point worldHit = Spotlight.TranslateScreenPointToWorldPoint(hit);


            #region Menu Touches
            if (drawRect.Contains(hit) && CanRespond())
            {
                if (selectedCommand == CommandType.NumCommands)
                {
                    Point adjustedHit = new Point(hit.X, hit.Y - MenuButtonVerticalOffset);
                    foreach (CommandButton cb in commandButtons)
                    {
                        if (cb.Contains(adjustedHit))
                        {
                            selectedCommand = cb.getData().getCommandType();
                            break;
                        }
                    }
                }

                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        OffsetMenuButtons((int)gesture.Delta.Y);
                        BecomeFirstResponder();
                        return true;
                }
                return true; // Eat all touches in this draw frame
            }
            else if (IsFirstResponder())
            {
                switch (gesture.GestureType)
                {
                    case GestureType.FreeDrag:
                        ResignFirstResponder();
                        if (selectedCommand == CommandType.NumCommands || commandlimits[(int)selectedCommand] == 0) return false; // earlier touch wasnt on a button.
                        Command created = new Command(worldHit, selectedCommand);
                        created.ProcessTouch(gesture);
                        Hide();
                        listOfWorldCommands.AddLast(created);
                        if (commandlimits[(int)selectedCommand] != -1)
                            commandlimits[(int)selectedCommand]--;
                        selectedCommand = CommandType.NumCommands;
                        return true;
                    case GestureType.DragComplete:
                        ResignFirstResponder();
                        selectedCommand = CommandType.NumCommands;
                        return false;
                    case GestureType.Flick:
                        if (gesture.Delta.Y > 0)
                        {
                            currentAnimationType = CommandMenuButtonScrollAnimationType.ToTop;
                        }
                        else
                        {
                            currentAnimationType = CommandMenuButtonScrollAnimationType.ToBottom;
                        }
                        ResignFirstResponder();
                        selectedCommand = CommandType.NumCommands;
                        return true;
                }
            }
            #endregion
            #region Toggle Touches
            if (toggle.ProcessTouch(gesture))
            {
                if (show)
                    Hide();
                else
                    Show();

                return true;
            }
            #endregion
            #region Command Touches
            foreach (Command c in listOfWorldCommands)
            {
                if (c.ProcessTouch(gesture, listOfWorldPlatforms))
                {
                    if (gesture.GestureType == GestureType.DragComplete)
                    {
                        if (drawRect.Contains(Spotlight.TranslateWorldPointToScreenPoint(c.drawRect.Location)))
                        {
                            listOfWorldCommands.Remove(c);
                            if (commandlimits[(int)c.getCommandType()] != -1)
                                commandlimits[(int)c.getCommandType()] += 1;
                        }
                        if (c.newlyCreated)
                        {
                            Show();
                            c.newlyCreated = false;
                        }
                    }
                    return true;
                }
            }
            #endregion
            return false;
        }

        #endregion

        public void CommandCollisions(Player ninja, LinkedList<Platform> listOfWorldPlatforms)
        {

            if (ninja.ninjaLifeState == LifeState.Alive)
            {

                //Tests for collision between ninja and all commands
                foreach (Command c in listOfWorldCommands)
                {
                    //only bother with this command if it still has charges
                    if (c.charges <= 0) continue;

                    //if there is a collision with command, get the player ready to execute it
                    if (ninja.Contains(c.drawRect))
                    {
                        switch (c.getCommandType())
                        {
                            case CommandType.MoveLeft:
                                if (ninja.facingLeft == false)
                                {
                                    ninja.Action_Move(true);
                                    c.charges--;
                                }
                                break;
                            case CommandType.MoveRight:
                                if (ninja.facingLeft == true)
                                {
                                    ninja.Action_Move(false);
                                    c.charges--;
                                }
                                break;
                            case CommandType.Jump:
                                if (ninja.Action_Jump()) c.charges--;
                                break;
                            case CommandType.WallJump:
                                if (ninja.positionMask != PositionState.OnFloor)
                                {
                                    livingWallConnect(c, listOfWorldPlatforms);
                                    if (ninja.Action_WallJump(c)) c.charges--;
                                }
                                break;
                            case CommandType.WallSlide:
                                livingWallConnect(c, listOfWorldPlatforms);
                                if (ninja.Action_WallSlide(c)) c.charges--;
                                break;
                            case CommandType.LedgeClimb:
                                livingWallConnect(c, listOfWorldPlatforms);
                                if (ninja.Action_LedgeClimb(c, listOfWorldPlatforms)) c.charges--;
                                break;
                            case CommandType.ObjectThrow:
                                if (ninja.Action_ThrowItem(c)) c.charges--;
                                break;
                            default:
                                throw new Exception("Invalid CommandType Exception: WorldObjectManager.UpdateAll()");
                        }
                    }
                }
            }
        }

        //used to attach a wall command to a wall durring runtime
        private void livingWallConnect(Command c, LinkedList<Platform> listOfWorldPlatforms)
        {
            foreach (Platform p in listOfWorldPlatforms)
            {
                //if(p.drawRect.Intersects(c.drawRect))
                    c.ConnectToPlatform(p);
            }
        }

        /// <summary>
        /// Resets the charges of all commands placed on the field.
        /// </summary>
        public void ResetCommandCharges()
        {
            foreach (Command c in listOfWorldCommands)
                c.refreshCharges();
        }



    }
}