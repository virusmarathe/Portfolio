using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelEditor.WorldObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LevelEditor.Util;

namespace LevelEditor
{
    public class Trigger : WorldObject
    {
        WorldObject attachedTo;
        public Trigger(Rectangle rect, Texture2D tex, WorldObject w) : base(rect, tex, ObjectType.Trigger, false, false)
        {
            drawRect = rect;
            drawTex = tex;
            attachedTo = w;
        }

        public Trigger(Rectangle rect, Texture2D tex) : base (rect, tex, ObjectType.Trigger, false, false)
        {
            drawRect = rect;
            drawTex = tex;
        }

    }
}
