using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PsychicNinja.Data.WorldObject.Entity
{
    class Projectile : WorldObject
    {
        private Vector2 velocity;

        public int lifespan;

        public Projectile(Point center, Vector2 v, Texture2D tex) : base(new Rectangle(center.X - tex.Width, center.Y - tex.Height, tex.Width, tex.Height ), tex)
        {
            lifespan = 90;
            velocity = v;
        }

        public void Update()
        {
            lifespan--;
            Point center = GetCenter();
            base.SetCenter(new Point((int)(center.X + velocity.X), (int)(center.Y + velocity.Y)));

        }
    }
}
