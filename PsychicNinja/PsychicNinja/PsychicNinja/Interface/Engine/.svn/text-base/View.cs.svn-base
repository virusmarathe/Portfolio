using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Interface.Engine
{
    public class View
    {
        public static ContentManager Content;

        //private Rectangle drawRect;
        public Rectangle drawRect;
        public Texture2D drawTex;

        public bool hidden;

        private ViewAnimationType animationType;

        private int blinkFrameInterval;

        #region Initialization

        /// <summary>
        /// Default null constructor for view object.
        /// </summary>
        public View() : this(Rectangle.Empty, null)
        {

        }

        /// <summary>
        /// One-arg constructor takes a rectangle for object position.
        /// </summary>
        /// <param name="rect"></param>
        public View(Rectangle rect) : this (rect, null)
        {
            drawRect = rect;
            drawTex = null;
            hidden = true;
        }

        /// <summary>
        /// Default Frame constructor. 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tex"></param>
        public View (Rectangle rect, Texture2D tex)
        {
            drawRect = rect;
            drawTex = tex;
            hidden = false;
        }


        /// <summary>
        /// Called to load all content this object will need.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(ContentManager Content)
        {

        }

        #endregion

        #region Game Loop

        public virtual void Update()
        {
            if (animationType != ViewAnimationType.None)
                UpdateAnimation();
        }

        /// <summary>
        /// Base draw function for frame. Passes member variables to SpriteBatch.Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (hidden) return;

            if (animationType == ViewAnimationType.AlphaFadeIn)
                spriteBatch.Draw(drawTex, drawRect, new Color(1f, 1f, 1f, ((float)(animationTime / animationDuration))));
            else
                spriteBatch.Draw(drawTex, drawRect, Color.White);

        }

        /// <summary>
        /// Base overload draw function for specifying overlay color.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (hidden) return;

            spriteBatch.Draw(drawTex, drawRect, color);

        }

        #endregion

        #region Animated Frame Transition


        private double animationTime = 0.0;
        private int animationDuration = 90;
        private bool accelPolarity = false;
        private Rectangle oldRect;
        private Rectangle animationRect;

        //Four variables used in the expand animation, specifying the difference
        //between the oldRect and the animationRect
        int horizontalDifference, verticalDifference, widthDifference, heightDifference;

        /// <summary>
        /// Causes this object to blink visibible and invisible at the stated interval
        /// for the stated duration.  If duration is -1, the object will blink forever.
        /// </summary>
        public void animatedBlink(int interval, int duration)
        {
            animationTime = 0;
            animationDuration = duration;
            blinkFrameInterval = interval;
            animationType = ViewAnimationType.Blink;
        }

        /// <summary>
        /// Causes this object to blink visibible and invisible at the stated interval
        /// for the stated duration.  If duration is -1, the object will blink forever.
        /// </summary>
        /// <param name="endRect">Ending rectangle size</param>
        /// <param name="duration">Time to take</param>
        /// <param name="useNHP">If true, this view will expand using animatedFrameSlide's negative horizontal parabola</param>
        public void animatedExpand(Rectangle endRect, int duration, bool useNHP)
        {
            animationRect = endRect;
            animationTime = 0;
            animationDuration = duration;
            animationType = ViewAnimationType.Expand;
            oldRect = drawRect;
            accelPolarity = useNHP;

            horizontalDifference = endRect.X - oldRect.X;
            verticalDifference = endRect.Y - oldRect.Y;
            widthDifference = endRect.Width - oldRect.Width;
            heightDifference = endRect.Height - oldRect.Height;
            
        }

        /// <summary>
        /// Animate a frame change over the next XXX tics using a negative horizontal parabola.
        /// </summary>
        /// <param name="newFrame">New Rectangle to occupy.</param>
        /// <param name="durationInTics">Time to take.</param>
        /// <param name="polarity">Whether this animation is accelerating (true) or deccelerating (false).</param>
        public void animatedFrameSlide(Point offset, int duration, bool polarity)
        {
            //if (hidden) // Don't calculate animations if we aren't being shown
            //{
            //    MoveFrame(offset.X, offset.Y);
            //}
            //else
            //{
            animationType = ViewAnimationType.Slide;
                if (animationRect.Equals(Rectangle.Empty))
                    animationRect = drawRect;
            animationTime = 0;
            animationDuration = duration;
            animationRect.Offset(offset);
            oldRect = drawRect;
            accelPolarity = polarity;
            //}
        }


        /// <summary>
        /// Causes this object to blink visibible and invisible at the stated interval
        /// for the stated duration.  If duration is -1, the object will blink forever.
        /// </summary>
        public void animatedAlphaFadeIn(int duration)
        {
            animationTime = 0;
            animationDuration = duration;
            animationType = ViewAnimationType.AlphaFadeIn;
        }

        public bool isAnimating()
        {
            return animationType != ViewAnimationType.None;
        }

        public void stopAnimation()
        {
            animationType = ViewAnimationType.None;
        }

        /// <summary>
        /// Move this object incrementally towards its goal.
        /// </summary>
        private void UpdateAnimation()
        {
            switch (animationType)
            {
                case ViewAnimationType.Slide:
                    //stop sliding if time is greater than duration
                    if (animationTime > animationDuration)
                    {

                        SetDrawFrame(animationRect);
                        animationRect = Rectangle.Empty;
                        animationType = ViewAnimationType.None;
                        break;
                    }

                    double newWeight;
                    if (!accelPolarity)
                        newWeight = Math.Sqrt(animationTime / animationDuration);
                    else
                        newWeight = Math.Pow(animationTime / animationDuration, 4);
                    double oldWeight = 1.0 - newWeight;

                    int newX = (int)((oldWeight * oldRect.X) + (newWeight * animationRect.X));
                    int newY = (int)((oldWeight * oldRect.Y) + (newWeight * animationRect.Y));

                    SetDrawFrame(new Rectangle(newX, newY, drawRect.Width, drawRect.Height));
                    animationTime++;

                    break;

                case ViewAnimationType.Blink:
                    //stop blinking if time is greater than duration, unless duration is -1
                    if (animationTime > animationDuration && animationDuration != -1)
                    {
                        hidden = false;
                        animationType = ViewAnimationType.None;
                    }
                    //otherwise check if blinking this update and toggle hidden if true
                    else
                    {
                        if (animationTime % blinkFrameInterval == 0)
                            hidden = !hidden;
                        animationTime++;
                    }
                    break;
                case ViewAnimationType.AlphaFadeIn:
                    if (animationTime > animationDuration)
                    {
                        animationTime = animationDuration;
                        animationType = ViewAnimationType.None;
                    }
                    else
                        animationTime++;
                    break;

                case ViewAnimationType.Expand:
                    if (animationTime > animationDuration)
                    {
                        animationTime = animationDuration;
                        animationType = ViewAnimationType.None;
                    }

                    else
                    {
                        //case where we're using the negative horizontal parabola thingy or whatever that is,
                        //the same kind of acceleration behavior though
                        if (accelPolarity)
                        {
                            
                        }
                        else
                        {
                            SetDrawFrame(new Rectangle(
                                oldRect.X + (int)(horizontalDifference * (animationTime / animationDuration)),
                                oldRect.Y + (int)(verticalDifference * (animationTime / animationDuration)),
                                oldRect.Width + (int)(widthDifference * (animationTime / animationDuration)),
                                oldRect.Height + (int)(heightDifference * (animationTime / animationDuration))));
                        }

                        animationTime++;

                    }
                    break;

            }

        }

        #endregion

        #region Touch Handling

        /// <summary>
        /// Handle Touch Input
        /// </summary>
        public virtual bool ProcessTouch(GestureSample gesture)
        {
            return (CanRespond() && drawRect.Contains(new Point((int)gesture.Position.X, (int)gesture.Position.Y)));
        }

        private static View RegisteredFirstResponder;

        public static View FirstResponder()
        {
            return RegisteredFirstResponder;
        }

        public bool IsFirstResponder()
        {
            return RegisteredFirstResponder == this;
        }

        public bool CanRespond()
        {
            return (RegisteredFirstResponder == null) || (RegisteredFirstResponder == this);
        }

        /// <summary>
        /// Make this object the first to have a shot at responding to a touch event.
        /// </summary>
        protected void BecomeFirstResponder()
        {
            if (RegisteredFirstResponder != null) return;

            RegisteredFirstResponder = this;
        }

        /// <summary>
        /// Make this object no longer the first responder.
        /// </summary>
        protected void ResignFirstResponder()
        {
            if (RegisteredFirstResponder == this)
                RegisteredFirstResponder = null;
        }

        #endregion

        #region Information

        /// <summary>
        /// Hit test function for Point.
        /// </summary>
        /// <param name="p">Point to test on this Frame.</param>
        /// <returns>Whether or not the point is inside this Frame.</returns>
        public bool Contains(Point p)
        {
            return drawRect.Contains(p);
        }

        /// <summary>
        /// Hit test function for Vector2. Calls Contains(Point)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Contains(Vector2 v)
        {
            return Contains(new Point((int)v.X, (int)v.Y));
        }

        /// <summary>
        /// Hit test function.
        /// </summary>
        /// <param name="x">X-Value of coordinate.</param>
        /// <param name="y">Y-Value of coordinate.</param>
        /// <returns>Whether or not the coordinate is contained in this Frame.</returns>
        public bool Contains(int x, int y)
        {
            return drawRect.Contains(x, y);
        }

        /// <summary>
        /// Hit test function.
        /// </summary>
        /// <param name="x">X-Value of coordinate.</param>
        /// <param name="y">Y-Value of coordinate.</param>
        /// <returns>Whether or not the coordinate is contained in this Frame.</returns>
        public bool Contains(float x, float y)
        {
            return drawRect.Contains((int)x, (int)y);
        }

        /// <summary>
        /// Returns an indication of whether these objects collide at all.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(Rectangle rect)
        {
            if (drawRect.X > rect.X + rect.Width) return false;
            if (drawRect.X + drawRect.Width < rect.X) return false;
            if (drawRect.Y > rect.Y + rect.Height) return false;
            if (drawRect.Y + drawRect.Height < rect.Y) return false;
            return true;
        }

        /// <summary>
        /// Return whether this object responds to a given touch gesture
        /// </summary>
        /// <param name="gesture"></param>
        /// <returns></returns>
        public bool RespondsToGesture(GestureSample gesture)
        {
            return RespondsToWorldTouch(gesture.Position);
        }

        public virtual bool RespondsToWorldTouch(Point p)
        {
            return ((!hidden) && (Contains(p)));
        }

        public bool RespondsToWorldTouch(Vector2 v)
        {
            return RespondsToWorldTouch(new Point((int)v.X, (int)v.Y));
        }

        #endregion

        #region Accessors

        #region Getters

        public Point Center()
        {
            return new Point(drawRect.X + drawRect.Width / 2, drawRect.Y + drawRect.Height / 2);
        }

        public Rectangle SourceFrame()
        {
            return new Rectangle(0, 0, drawRect.Width, drawRect.Height);
        }

        public Rectangle SourceFrame_XReversed()
        {
            return new Rectangle(drawRect.Width, 0, -drawRect.Width, drawRect.Height);
        }

        public Rectangle SourceFrame_YReversed()
        {
            return new Rectangle(0, drawRect.Height, drawRect.Width, -drawRect.Height);
        }

        public Rectangle SourceFrame_Inverted()
        {
            return new Rectangle(drawRect.Width, drawRect.Height, -drawRect.Width, -drawRect.Height);
        }

        public Point Size()
        {
            return new Point(drawRect.Width, drawRect.Height);
        }

        #endregion

        #region Setters
        
        public void SetDrawFrame(Rectangle rect)
        {
            drawRect = rect;
        }
        
        public void MoveFrame(int x, int y)
        {
            drawRect.Offset(x, y);
        }

        public void ResizeFrame(int x, int y)
        {
            drawRect.Width += x;
            drawRect.Height += y;
        }

        public void SetPosition(Point point)
        {
            drawRect = new Rectangle(point.X, point.Y, drawRect.Width, drawRect.Height);
        }

        public void SetPosition(Vector2 vec)
        {
            drawRect = new Rectangle((int)vec.X, (int)vec.Y, drawRect.Width, drawRect.Height);
        }

        public virtual void SetCenter(Point p)
        {
            drawRect = new Rectangle(p.X - drawRect.Width / 2, p.Y - drawRect.Height / 2, drawRect.Width, drawRect.Height);
        }

        public virtual void SetCenter(Vector2 v)
        {
            drawRect = new Rectangle((int)v.X - drawRect.Width / 2, (int)v.Y - drawRect.Height / 2, drawRect.Width, drawRect.Height);
        }

        public void SetDrawTexture(Texture2D tex)
        {
            drawTex = tex;
        }
        
        #endregion

        #endregion


    }
}
