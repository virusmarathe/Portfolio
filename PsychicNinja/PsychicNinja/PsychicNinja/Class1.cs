using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace PsychicNinja
{
    public class GestureType : EventArgs
    {
        public GestureSample gesture
        {
            set
            {
                gesture = value;
            }
            get
            {
                return gesture;
            }
        }
    }

    /// <summary>
    /// Broadcasts an event whenever 
    /// </summary>
    public class TouchManager
    {
        public event TouchHandler Touch;
        public GestureType e = null;
        public delegate void TouchHandler(TouchManager m, GestureType e);
        public void Start()
        {
            while (true)
            {
                TouchCollection touches = TouchPanel.GetState();
                if (TouchPanel.IsGestureAvailable)
                {
                    e = new GestureType();
                    e.gesture = TouchPanel.ReadGesture();
                    Touch(this, e);
                }
            }
        }
    }

    class Class1: Microsoft.Xna.Framework.Game
    {


        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!Level.isVisible)
                Overlay.Update(); //overlay contains all things that are drawn instead of the level; for example the titlescreen menus
            else
                Level.Update();
            
            base.Update(gameTime);
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
