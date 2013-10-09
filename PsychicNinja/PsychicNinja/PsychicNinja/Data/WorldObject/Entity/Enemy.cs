//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : Enemy.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PsychicNinja.Data.Patrol;

using Microsoft.Xna.Framework.Content;

using PsychicNinja.Data.Util;
using System.Diagnostics;
using System;
using PsychicNinja.Data.WorldObject.Entity;
using PsychicNinja.Metrics;

namespace PsychicNinja.Data.WorldObject
{

    public class Enemy : WorldObject
    {

        private PatrolModel patrol;
        private LifeState state;

        private EnemyActionState actionState;
        private static Texture2D projectileTexture;
        //projectile behavior
        private Boolean shoots;
        private int visionrange;
        private List<Projectile> projectiles;

        //Running Animation
        private static List<Texture2D> RunTextures;
        private static AnimationComponent running;

        //Attack Animation
        private static List<Texture2D> AttackTextures;
        private static AnimationComponent attacking;

        private static string EnemyName;

        //private static Texture2D[] textureArray;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="Velocity"></param>
        /// <param name="tex"></param>
        /// <param name="Shape"></param>
        public Enemy(Rectangle rect, Texture2D tex, string name, ObjectShape Shape) :
            base(rect, tex, Shape)
        {
            shoots = false;

            projectileTexture = null;
            EnemyName = name;
            state = LifeState.Alive;
            actionState = EnemyActionState.Standing;
            RunTextures = new List<Texture2D>();
            AttackTextures = new List<Texture2D>();
        }

        public Enemy(Rectangle rect, Texture2D tex, string name, ObjectShape Shape, int vr) :
            base(rect, tex, Shape)
        {
            shoots = true;
            visionrange = vr;
            projectiles = new List<Projectile>();
            EnemyName = name;
            state = LifeState.Alive;
            actionState = EnemyActionState.Standing;
            RunTextures = new List<Texture2D>();
            AttackTextures = new List<Texture2D>();
        }

        /// <summary>
        /// Move the enemy
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(int timeElapsed, Player n)
        {
            if (patrol != null)
                velocity = patrol.getCurrentVector(timeElapsed);
            
            Point c = GetCenter();

            if(shoots){
                if(facingLeft){
                    if (new Rectangle(c.X - visionrange, c.Y, visionrange, 1).Intersects(n.DrawFrame())){
                        projectiles.Add(new Projectile(c, new Vector2(-6, new Random().Next(-3, 3)), projectileTexture));
                    }
                }
            
               else{
                   if (new Rectangle(c.X, c.Y, visionrange, 1).Intersects(n.DrawFrame()))
                   {
                       projectiles.Add(new Projectile(c, new Vector2(6, new Random().Next(-3, 3)), projectileTexture));
                   }
                }

                List<Projectile> hitlist = new List<Projectile>();

                foreach (Projectile p in projectiles)
                {
                    p.Update();
                    if (p.lifespan < 0)
                        hitlist.Add(p);
                    else if (p.DrawFrame().Intersects(n.DrawFrame()))
                        n.death();
                }
                foreach (Projectile p in hitlist)
                    projectiles.Remove(p);
            }


        



            if (velocity.X == 0)
            {
                actionState = EnemyActionState.Standing;
            }
            else
            {
                actionState = EnemyActionState.Running;
                facingLeft = velocity.X < 0;
            }

            switch (actionState)
            {
                case EnemyActionState.Standing:

                    break;
                case EnemyActionState.Running:
                    running.Update(timeElapsed);
                    break;
                case EnemyActionState.Attacking:
                    attacking.Update(timeElapsed);
                    break;
                case EnemyActionState.Dying:

                    break;
            }
            

            base.Update(timeElapsed);
	    }

        /// <summary>
        /// Draw the enemy
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (state == LifeState.Alive)
            {
                SpriteEffects effect = facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally; // Change based on what direction he's facing

                switch (actionState)
                {
                    case EnemyActionState.Standing:
                        base.Draw(spriteBatch);
                        break;
                    case EnemyActionState.Running:
                        running.Draw(spriteBatch, modifiedDrawRect(), effect);
                        break;
                    case EnemyActionState.Attacking:
                        attacking.Draw(spriteBatch, modifiedDrawRect(), effect);
                        break;
                    case EnemyActionState.Dying:

                        break;
                }
                if (shoots)
                    foreach (Projectile p in projectiles)
                        p.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Load animations based on the current enemy
        /// </summary>
        /// <param name="Content">Content manager to load from.</param>
        public void LoadAnimations(ContentManager Content)
        {
            //Sorry bro-grammers, this is our best bet until we can read these in via the .xml file
            if (EnemyName.Equals("Enemies/Muscle-Goon"))
            {
                RunTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 1"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 2"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 3"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 4"));
                running = new AnimationComponent(RunTextures, 0, 0, 100, 160);
                AttackTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 5"));
                AttackTextures.Add(Content.Load<Texture2D>("Enemies/Muscle-Goon 6"));
                attacking = new AnimationComponent(AttackTextures, 0, 0, 100, 160);

                projectileTexture = Content.Load<Texture2D>("shuriken slide 1");
            }
            else if (EnemyName.Equals("Enemies/knife artist 1"))
            {
                RunTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 1"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 2"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 3"));
                RunTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 4"));
                running = new AnimationComponent(RunTextures, 0, 0, 100, 160);
                AttackTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 5"));
                AttackTextures.Add(Content.Load<Texture2D>("Enemies/knife artist 6"));
                attacking = new AnimationComponent(AttackTextures, 0, 0, 100, 160);
                projectileTexture = Content.Load<Texture2D>("shuriken slide 1");

            }
        }

        public void setPatrolModel(PatrolModel model)
        {
            patrol = model;
        }

        public void attack()
        {
            actionState = EnemyActionState.Attacking;
        }

        /// <summary>
        /// Kills the enemy preventing him from being updated or drawn
        /// </summary>
        public void death()
        {
            state = LifeState.Dead;
            actionState = EnemyActionState.Dying;
        }
    }

}
