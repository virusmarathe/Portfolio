using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PsychicNinja.Data.Patrol
{
    public class PatrolModel
    {
        LinkedList<Vector2> patrolVector;
        LinkedList<int> patrolLength;
        public int StartTime;

        public PatrolModel()
        {
            patrolLength = new LinkedList<int>();
            patrolVector = new LinkedList<Vector2>();
        }

        /// <summary>
        /// Adds a new vector to the patrol using the given duration.
        /// </summary>
        /// <param name="newVec">The new velocity vector to add.</param>
        /// <param name="newDuration">The length in seconds this velocity should be kept.</param>
        public void addVector(Vector2 newVec, int newDuration)
        {
            patrolVector.AddLast(newVec);
            patrolLength.AddLast(newDuration);
        }

        /// <summary>
        /// Returns the current velocity vector determined by the Patrol Model.
        /// </summary>
        /// <param name="timeElapsed">The number of seconds that have passed since this patrol started.</param>
        /// <returns></returns>
        public Vector2 getCurrentVector(int gameTime)
        {
            int count = -1;
            int timeElapsed = gameTime - StartTime;
            while (timeElapsed >= 0)
            {
                // structured this way so that the count returned is accurate
                count++;
                if (count == patrolLength.Count)
                    count = 0;
                int time = patrolLength.ElementAt(count);
                if (time == 0) break;
                timeElapsed -= time;
            }
            return patrolVector.ElementAt(count);
        }
    }
}
