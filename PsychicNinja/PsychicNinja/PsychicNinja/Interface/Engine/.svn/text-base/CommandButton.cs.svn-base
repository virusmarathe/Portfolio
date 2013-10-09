using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PsychicNinja.Data.Object;

namespace PsychicNinja.Interface.Engine
{
    class CommandButton : Button 
    {
        private Command data;

        public CommandButton(Rectangle frame) : base(frame)
        {

        }

        public static new void LoadContent(ContentManager Content)
        {
            
        }

        #region Accessors

        public void setData(Command c)
        {
            data = c;
            drawTex = c.drawTex;
        }

        public Command getData()
        {
            return data;
        }

        #endregion

        public void OverlayWithCount(int count)
        {

        }
    }
}
