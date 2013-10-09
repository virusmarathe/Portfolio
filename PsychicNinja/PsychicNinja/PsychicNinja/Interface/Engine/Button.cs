using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
 

namespace PsychicNinja.Interface.Engine
{
    public class Button : View
    {
        bool selected = false;
        public Color selectedTint = Color.LightCyan;
        const int holdSelectedTint = 10;
        int selectedDuration = 0;
        int extraData = -1;

        public Button(Rectangle rect) : base(rect)
        {
           
        }

        /// <summary>
        /// Default constructor of Button object.
        /// </summary>
        /// <param name="rect">Rectangle for size and position.</param>
        /// <param name="tex">Texture of this button</param>
        public Button(Rectangle rect, Texture2D tex) : base(rect, tex)
        {
            
        }

        
        public override void Update()
        {
            base.Update();

            selectedDuration--;
            if (selectedDuration <= 0)
                selected = false;
        }

        /// <summary>
        /// Draw this object. No customization atm. 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (selected)
                base.Draw(spriteBatch, selectedTint);
            else 
                base.Draw(spriteBatch);            

        }

        public void SetSelected(bool select)
        {
            selected = select;
            selectedDuration = holdSelectedTint;
        }

        public void setExtra(int i)
        {
            extraData = i;
        }
        public int getExtra()
        {
            return extraData;
        }
        public override bool ProcessTouch(GestureSample gesture)
        {
            if (hidden) return false;
            if (!base.ProcessTouch(gesture)) return false; //Easy out

            if (gesture.GestureType != GestureType.Tap) return false;

            SetSelected(true);
            return true;
        }
    }
}
