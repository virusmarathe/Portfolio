using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PsychicNinja.Data.Object.Entity
{
    class Emitter
    {
        Rectangle region;
        Vector2 initialDirection;
        double radialOffset;
        double angle;
        double intensity;
        double velocityVariance;
        int particleLifespan, maxParticles, icounter;

        List<Texture2D> particleTextures;

        List<Particle> associatedParticles;
        List<Particle> particlesToRemove;

        Vector2 particleDelta;
        Vector2 particleDeltaIdeal;


        /// <summary>
        /// The fraction of all particles emitted by this object that employ the delta object.
        /// </summary>
        private double FractionEmployingDelta;

        private double rotation;


        /// <summary>
        /// boolean flag that determines whether or not this emitter is producing new particles.
        /// Toggle true to emit particles, false to stop emission.  The default value is true.
        /// </summary>
        public bool isEmitting = true;


        /// <summary>
        /// boolean flag that determines whether or not the particles produced by this emitter are drawn
        /// in screen coordinates.  Toggle true to draw in screen coordinates and false to draw in
        /// world coordinates.  The default value is false.
        /// </summary>
        public bool drawInScreenCoordinates = false;


        /// <summary>
        /// boolean flag that determines whether or not the particles produced by this emitter fade
        /// at the end of their lifespan, as opposed to just disappearing.  Toggle true to activate
        /// the fading functionality.  The default value is false.
        /// </summary>
        public bool particlesFade = false;

        /// <summary>
        /// Creates an emitter object that spews out particles.  These particles will be assigned a starting velocity which will never change.
        /// </summary>
        /// <param name="region">The rectangular region this emitter will produce particles from</param>
        /// <param name="direction">A vector2 specifying the general movement of particles immediately upon generation.</param>
        /// <param name="radialOffset">A radian value extending to both sides of the direction vector that can offset the starting velocity
        /// vector of particles being generated.  If this value is Math.PI or greater, particles will be generated in all directions.</param>
        /// <param name="velocityVariance">The amount by which the velocity is allowed to vary for new particles, from 0.0 to 1.0</param>
        /// <param name="intensity">The rate at which this emitter generates particles every 30 frames, with 1.0 being one particle every 30 frames.  This value cannot be less than or equal to
        /// 0, or the program will try to generate an infinite number of particles.</param>
        /// <param name="maxParticles">The maximum number of particles this emitter can generate at a time</param>
        /// <param name="particleLifespan">The lifespan, in frames, of each generated particle.  Use -1 to specify an infinite lifespan</param>
        /// <param name="particleArt">A list of textures that is used to randomly assign a texture to each particle.</param>
        public Emitter(Rectangle region, Vector2 direction, double radialOffset, double velocityVariance, double intensity, int maxParticles, int particleLifespan, List<Texture2D> particleArt)
        {
            this.region = region;
            initialDirection = direction;
            this.radialOffset = radialOffset;
            if (intensity <= 0)
                throw new Exception("intensity cannot be less than or equal to 0.");
            this.intensity = intensity;
            this.maxParticles = maxParticles;
            this.particleLifespan = particleLifespan;
            particleTextures = particleArt;

            if (velocityVariance < 0 || velocityVariance > 1.0)
                throw new Exception("velocityVariance cannot be less than 0 or greater than 1.0");

            this.velocityVariance = velocityVariance;

            //figure out the initial direction's angle
            angle = Math.Atan2(initialDirection.Y, initialDirection.X);

            associatedParticles = new List<Particle>();
            particlesToRemove = new List<Particle>();

            particleDelta = Vector2.Zero;

            rotation = 1.0;

            icounter = 0;
        }

        /// <summary>
        /// Creates an emitter object that spews out particles.  These particles will have a variable velocity.
        /// </summary>
        /// <param name="region">The rectangular region this emitter will produce particles from</param>
        /// <param name="direction">A vector2 specifying the general movement of particles immediately upon generation.</param>
        /// <param name="radialOffset">A radian value extending to both sides of the direction vector that can offset the starting velocity
        /// vector of particles being generated.  If this value is Math.PI or greater, particles will be generated in all directions.</param>
        /// <param name="velocityVariance">The fractional amount by which the velocity is allowed to vary for new particles, from 0.0 to 1.0</param>
        /// <param name="intensity">The rate at which this emitter generates particles every 30 frames, with 1.0 being one particle every 30 frames.  This value cannot be less than or equal to
        /// 0, or the program will try to generate an infinite number of particles.</param>
        /// <param name="maxParticles">The maximum number of particles this emitter can generate at a time</param>
        /// <param name="particleLifespan">The lifespan, in frames, of each generated particle.  Use -1 to specify an infinite lifespan</param>
        /// <param name="particleArt">A list of textures that is used to randomly assign a texture to each particle</param>
        /// <param name="particleDelta">A delta value to change particle velocity by; for example, gravity or wind</param>
        /// <param name="particleDeltaIdeal">The cutoff for particleDelta's effects; for example, a terminal falling velocity</param>
        /// <param name="FractionEmployingDelta">The fraction of generated particles that should use their delta values; 1.0 means all particles will use their delta, 0.0 means none will.  As might be expected, this value
        /// must be between 0.0 and 1.0 or an error will be thrown</param>
        public Emitter(Rectangle region, Vector2 direction, double radialOffset, double velocityVariance, double intensity, int maxParticles, int particleLifespan, List<Texture2D> particleArt, Vector2 particleDelta, Vector2 particleDeltaIdeal, double FractionEmployingDelta)
        {
            this.region = region;
            initialDirection = direction;
            this.radialOffset = radialOffset;
            if (intensity <= 0)
                throw new Exception("intensity cannot be less than or equal to 0.");
            this.intensity = intensity;
            this.maxParticles = maxParticles;
            this.particleLifespan = particleLifespan;
            particleTextures = particleArt;

            if (velocityVariance < 0 || velocityVariance > 1.0)
                throw new Exception("velocityVariance cannot be less than 0 or greater than 1.0"); this.velocityVariance = velocityVariance;

            this.velocityVariance = velocityVariance;

            //figure out the initial direction's angle
            angle = Math.Atan2(initialDirection.Y, initialDirection.X);

            associatedParticles = new List<Particle>();
            particlesToRemove = new List<Particle>();

            this.particleDelta = particleDelta;
            this.particleDeltaIdeal = particleDeltaIdeal;

            changeFractionEmployingDelta(FractionEmployingDelta);

            rotation = 1.0;

            icounter = 0;
        }

        ///<Summary>
        ///Changes the production region associated with this emitter.  This is used in place of being able
        ///to anchor it to an object.
        ///</Summary>
        ///<param name="newRegion">the new region to use</param>
        public void AssignRegion(Rectangle newRegion)
        {
            region = newRegion;
        }

        /// <summary>
        /// Creates a new particle.
        /// </summary>
        private void produceParticle(){
            Texture2D selectedTexture = particleTextures.ElementAt(Game1.RapperRandomDiggityDawg.Next(0, particleTextures.Count));


            
            //generate a delta for the angle, bounded by -radialOffset, radialOffset
            double r = Game1.RapperRandomDiggityDawg.NextDouble();
            double newAngleDelta = (2 * (r * radialOffset)) - radialOffset;

            //make the new angle
            double newAngle = angle + newAngleDelta;

            //figure out the new vector
            Vector2 newVector = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));

            if (velocityVariance != 0.0) //if we have variable velocity, figure out the new vector's length based on the variance
            { 
                double num = (((2 * Game1.RapperRandomDiggityDawg.NextDouble()) * velocityVariance) - velocityVariance) + 1.0;
                newVector = Vector2.Normalize(newVector) * (float)(initialDirection.Length() * num);
            }
            else
                newVector = Vector2.Normalize(newVector) * initialDirection.Length();

//            Vector2 newVector = new Vector2(0f, -1f);

            //figure out starting position within the emitter's region
            Rectangle b = new Rectangle(0,0,0,0);
            if(selectedTexture.Width > region.Width)
                b.X = (region.X + (region.Width/2)) - (selectedTexture.Width / 2);
            else
                b.X = Game1.RapperRandomDiggityDawg.Next(region.X, region.Right - selectedTexture.Width);

            if(selectedTexture.Height > region.Height)
                b.Y = (region.Y + (region.Height/2)) - (selectedTexture.Height / 2);
            else
                b.Y = Game1.RapperRandomDiggityDawg.Next(region.Y, region.Bottom - selectedTexture.Height);

            b.Width = selectedTexture.Width;
            b.Height = selectedTexture.Height;


            //create the particle and add it to the list of associated Particles

            Particle P;

            if (FractionEmployingDelta > 0 && Game1.RapperRandomDiggityDawg.NextDouble() < FractionEmployingDelta)
            {
                P = new Particle(b, selectedTexture, (Game1.RapperRandomDiggityDawg.NextDouble() * rotation) * Math.PI/4, newVector, particleDelta, particleDeltaIdeal, particleLifespan, drawInScreenCoordinates);
            }
            else
                P = new Particle(b, selectedTexture, (Game1.RapperRandomDiggityDawg.NextDouble() * rotation) *  Math.PI/4, newVector, particleLifespan, drawInScreenCoordinates);

            P.fades = particlesFade;
            associatedParticles.Add(P);
        }

        /// <summary>
        /// Teturns true if this emitter has currently generated its maxParticles for the list of associated particles
        /// </summary>
        /// <returns></returns>
        public bool atMaxParticles()
        {
            return associatedParticles.Count >= maxParticles;
        }

        public void Update(){
            icounter++;
            foreach (Particle p in associatedParticles){
                //if this particle has expired, remove it
                if (p.lifespanRemaining == 0)
                    particlesToRemove.Add(p);
                else
                    p.Update();
                }
            if (associatedParticles.Count < maxParticles && icounter >= intensity*30 && isEmitting)
            {
                for(int i = 0; i < icounter / (intensity*30); i++)
                    produceParticle();
                icounter = (int)(icounter % (intensity*30));
            }

            if (particlesToRemove.Count != 0)
                foreach (Particle p in particlesToRemove)
                    associatedParticles.Remove(p);
            particlesToRemove.Clear();
        }

        public void Draw(SpriteBatch sb){
            foreach (Particle p in associatedParticles)
            {
                p.Draw(sb);
            }
        }
        
        /// <summary>
        /// Changes the variance of the rotation assigned to each particle.  The default variance is 1.0; smaller values
        /// will cause the top speed of rotating objects to be reduced, and larger values will increase it.
        /// </summary>
        /// <param name="newRotation"></param>
        public void changeRotationVariance(double newRotation)
        {
            rotation = newRotation;
        }

        /// <summary>
        /// Changes the fraction of particles emitted that employ their delta values.  
        /// Throws an error if the new fraction isn't between 0.0 and 1.0, or if particleDelta is Vector2.Zero
        /// </summary>
        /// <param name="newFraction"></param>
        public void changeFractionEmployingDelta(double newFraction)
        {
            if (newFraction < 0.0 || newFraction > 1.0)
                throw new Exception("newFraction is out of bounds");
            else if (particleDelta.Equals(Vector2.Zero))
                throw new Exception("particleDelta is a zero value; this emitter is currently cannot use particleDelta.");
            else
                FractionEmployingDelta = newFraction;
        }


    }
}
