//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : Platform.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PsychicNinja.Data.Patrol;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Data.Object
{

    public class Platform : WorldObject
    {
        public bool isHazardous;
        public bool triggered;
        //List of ropes the platoform depends on, and suspend boolean
        public List<SuspendRope> suspendRopes;
        public LinkedList<Trigger> triggers = new LinkedList<Trigger>();
        public bool suspended = false;

        #region Init

        /// <summary>
        /// A platform used for Unit testing. DONT CALL THIS IN THE PROJECT
        /// </summary>
        /// <param name="rect"></param>
        public Platform(Rectangle rect) : this (rect, "texturemissing", false, false, false, false, false)
        {

        }

        /// <summary>
        /// Simple constructor of a motionless platform. 
        /// </summary>
        /// <param name="rect">Position and size of the platform.</param>
        /// <param name="Velocity">Vector2: initial velocity of the platform.</param>
        /// <param name="tex">Texture2D: texture for the platform.</param>
        /// <param name="Shape">ObjectShape: shape of the platform.</param>
        /// <param name="IsHazardous">boolean: Platform is hazardous.</param>
        /// <param name="gravity">boolen: Platform is affected by gravity.</param>
        /// 

        public Platform(Rectangle rect, Texture2D tex, bool IsHazardous, bool IsAnimated, bool gravity) : this(rect, tex, IsHazardous, IsAnimated, gravity, false, true)
        {

        }
        
        public Platform(Rectangle rect, string texName, bool IsHazardous, bool IsAnimated, bool gravity, bool through, bool tile) : this (rect, Content.Load<Texture2D>(texName), IsHazardous, IsAnimated, gravity, through, tile)
        {

        }


        public Platform(Rectangle rect, Texture2D texName, bool IsHazardous, bool isAnimated, bool gravity, bool through, bool tile)
            : base(rect, texName)
        {
            goThrough = through;
            isTiled = tile;
            if (isTiled) drawStyle = WorldObjectDrawStyle.Tiled;
            else drawStyle = WorldObjectDrawStyle.StretchToFit;
            isHazardous = IsHazardous;
            hasGravity = gravity;
            patrol = null;
            triggered = false;
            suspendRopes = new List<SuspendRope>();
        }

        #endregion

        #region Life Cycle

        public override void Update(int timeElapsed)
        {
            if (triggered)
            {
                if (patrol != null)
                    velocity = patrol.getCurrentVector(timeElapsed);
                else
                    hasGravity = true;
            }
            else if (triggers.Count == 0 && patrol!=null)
            {
                velocity = patrol.getCurrentVector(timeElapsed);
            }
            if (suspended && suspendRopes.Count == 0)           //The platform falls
            {
                hasGravity = true;
                velocity = new Vector2(0, 0);
                suspended = false;
            }

            if (PlatformFlashingAnimationDuration != 0)
                UpdateFlashing();

            base.Update(timeElapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (SuspendRope s in suspendRopes)
            {
                s.Draw(spriteBatch);
            }
            base.Draw(spriteBatch, flashOn? Color.DarkGoldenrod : Color.White);

        }

        #endregion

        public void addRope(SuspendRope r)
        {
            suspended = true;
            suspendRopes.Add(r);
        }

        public void cutRope(SuspendRope r)
        {
            suspendRopes.Remove(r);
        }


        public void addTrigger(Trigger t)
        {
            triggers.AddLast(t);
        }

        public void StartPatrol(int gameTime)
        {
            if (triggered) return;
            triggered = true;
            if (patrol != null)
                patrol.StartTime = gameTime;
        }

        #region Flashing Animation

        int PlatformFlashingAnimationDuration = 0;
        const int PlatformFlashingAnimationLength = 40;

        bool flashOn = false;

        public void Flash()
        {
            if (PlatformFlashingAnimationDuration != 0) return; // Already started

            PlatformFlashingAnimationDuration++;

        }

        public void UpdateFlashing()
        {
            PlatformFlashingAnimationDuration++;
            
            flashOn = (((PlatformFlashingAnimationDuration >> 3) & 1) == 0);

            if (PlatformFlashingAnimationDuration > PlatformFlashingAnimationLength)
                PlatformFlashingAnimationDuration = 0;
        }
        #endregion
    }
}