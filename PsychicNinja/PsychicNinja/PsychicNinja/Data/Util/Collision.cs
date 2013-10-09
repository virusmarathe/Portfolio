using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using PsychicNinja.Data.Object;
using PsychicNinja.Interface.Engine;

namespace PsychicNinja.Data.Util
{
    public static class Collision
    {
        /// <summary>
        /// Andrew's code modified to return the side the collision happened               
        ///        |
        ///        V
        ///     ___1____  
        ///     |       |
        ///--->4| obj1  |2<----
        ///     |_______|
        ///        3  
        ///        ^
        ///        |
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns>Returns a number corresponding to the side of the collision, relative to Obj1 or 0.</returns>
        public static int checkCollisions(WorldObject obj1, WorldObject obj2)
        {
            Rectangle rect1 = obj1.drawRect;
            Rectangle rect2 = obj2.drawRect;
            Rectangle rect2i = rect2;
            rect2i.Inflate(2, 2);
            if (!rect1.Intersects(rect2i)) return 0;

            int obj1Left = rect1.Left;
            int obj1Right = rect1.Right;
            int obj1Top = rect1.Top;
            int obj1Bottom = rect1.Bottom;
            int obj2Left = rect2.Left;
            int obj2Right = rect2.Right;
            int obj2Top = rect2.Top;
            int obj2Bottom = rect2.Bottom;

            // intersect check
            if (obj1Left > obj2Right || obj1Right < obj2Left || obj1Top > obj2Bottom || obj1Bottom < obj2Top) return 0;
            
            //check for rectangular collisions; change leftOffset, rightOffset, topOffset, bottomOffset based on 
            #region RectangleCollisionCheck
            // initially set to 1, if the side is in obj2 it gets set to 10
            int leftOffset = 0, rightOffset = 0, topOffset = 0, bottomOffset = 0;
            
            // set the flag for colliding sides
            if (obj1Left >= obj2Left && obj1Left <= obj2Right) leftOffset = 1;
            if (obj1Right <= obj2Right && obj1Right >= obj2Left) rightOffset = 1;
            if (obj1Top >= obj2Top && obj1Top <= obj2Bottom) topOffset = 1;
            if (obj1Bottom <= obj2Bottom && obj1Bottom >= obj2Top) bottomOffset = 1;

            //add up the offsets to see how many sides intersect obj2
            int sum = leftOffset + rightOffset + topOffset + bottomOffset;
            
            if (sum == 3) // 3 sides intersect
            {
                if (leftOffset == 0) return 2;
                else if (rightOffset == 0) return 4;
                else if (bottomOffset == 0) return 1;
                else if (topOffset == 0) return 3;
            }
            else if (sum == 2) // 2 sides intersect           
            {
                //These variables used to calculate edge cases the +5 is length of a foot for realistic detection
                float Xdist1 = Math.Abs(obj2Right - obj1Left);
                float Ydist1 = Math.Abs(obj2Top - obj1Bottom) + 5;
                float Xdist2 = Math.Abs(obj2Left - obj1Right);
                float Ydist2 = Math.Abs(obj2Bottom - obj1Top) + 5;
                
                //Check and calculate corner cases where two sides intersect
                //bot left corner obj1 intersects obj2
                if (bottomOffset == 1 && leftOffset == 1)
                {
                    if (Xdist1 > Ydist1 && obj1.velocity.Y > 0) return 3;
                    return 4;
                }
                //top left corner obj1 intersects obj2
                else if (leftOffset == 1 && topOffset == 1)
                {
                    if (Xdist1 > Ydist2 && obj1.velocity.Y < 0) return 1;
                    return 4;
                }
                //top right corner obj1 intersects obj2
                else if (topOffset == 1 && rightOffset == 1)
                {
                    if (Xdist2 > Ydist2 && obj1.velocity.Y < 0) return 1;
                    return 2;
                }
                //bot right corner obj1 intersects obj2
                else if (bottomOffset == 1 && rightOffset == 1)
                {
                    if (Xdist2 > Ydist1 && obj1.velocity.Y > 0) return 3;
                    return 2;
                }                
            }
            else if (sum == 1) // 1 side intersects
            {
                if (rightOffset == 1) return 2;
                else if (leftOffset == 1) return 4;
                else if (bottomOffset == 1) return 3;
                else if (topOffset == 1) return 1;
            }
            else if (sum == 0) return 3; //this object completely surrounds obj2
            #endregion

            return 0; //never happens
        }

        //Depending on the side of collision, calculate how far obj1 is intersecting obj2, and move obj1 out of obj2
        //and set both objects's X or Y velocity to 0
        public static void collisionResolution(int collisionState, WorldObject obj1, WorldObject obj2)
        {
            Point obj1Coor = obj1.drawRect.Location;
            Point obj2Coor = obj2.drawRect.Location;
            Point obj1Size = obj1.Size();
            Point obj2Size = obj2.Size();

            float distX;
            float distY;
        
            switch (collisionState)
            {
                case 1:                             //obj1's top coollides with obj2's bottom, move obj1 down and set Yvel to 0
                    distY = Math.Abs(obj1Coor.Y - (obj2Coor.Y + obj2Size.Y));
                    // If the ninja is on the ground, and a platform strikes his head, he gets squished and dies.
                    if (obj1 is Player && obj2 is Platform && (obj2.velocity.Y > 0) && obj1.positionMask == PositionState.OnFloor)
                    {
                        Player temp = (Player)obj1;
                        temp.explodeDeath();
                        break;
                    }
                    //obj1.MoveFrame(0, (int)distY);
                    if (obj1 is Player && obj2 is Platform)
                    {
                        obj1.MoveFrame(0, (int)distY);
                        //If Player is going up and hits his head, zero his Y velocity
                        if(obj1.velocity.Y < 0) obj1.setVelocity(obj1.velocity.X, 0);
                        break;
                    }
                    else if (obj2 is Player && obj1 is Platform)
                    {
                        obj2.setVelocity(obj1.velocity.X, 0);
                        obj2.setPositionState(PositionState.OnFloor);
                        break;
                    }
                    obj1.MoveFrame(0, (int)distY);
                    if (obj1.velocity.Y < 0) obj1.setVelocity(obj1.velocity.X, 0);
                    obj2.setVelocity(obj2.velocity.X, 0);
                    break;
                case 2:                             //obj1's right coollides with obj2's left,  move obj1 left and set Xvel to 0
                    distX = Math.Abs(obj1Coor.X + obj1Size.X - obj2Coor.X);
                    obj1.MoveFrame((int)-distX, 0);
                    //obj1.setVelocity(0, obj1.velocity.Y);
                    //obj2.setVelocity(0, obj2.velocity.Y);
                
                    break;
                case 3:                             //obj1's bottom coollides with obj2's top, move obj1 up and set Yvel to 0                                            
                    distY = Math.Abs(obj1Coor.Y + obj1Size.Y - obj2Coor.Y);
                    //obj1.MoveFrame(0, (int)-distY);
                    // if a player hits a platform with more than a certain fallspeed, he dies on impact.
                    if (obj1 is Player && obj1.velocity.Y >= WorldObject.terminalVelocity)
                    {
                        Player temp = (Player)obj1;
                        temp.fallDeath();

                    }

                    if (obj1 is Player && obj2 is Platform)
                    {
                        obj1.MoveFrame(0, (int)-distY);
                        obj1.setVelocity(obj1.velocity.X, 0);
                        obj1.setPositionState(PositionState.OnFloor);
                        break;
                    }
                    else if (obj2 is Player && obj1 is Platform)
                    {
                        obj2.setVelocity(obj1.velocity.X, 0);
                        break;
                    }
                    obj1.MoveFrame(0, (int)-distY);
                    obj1.setVelocity(obj1.velocity.X, 0); // Only stop this object from moving down.
                    obj2.setVelocity(obj2.velocity.X, 0); // Only stop this object from moving up.
                    obj1.setPositionState(PositionState.OnFloor);
                    //if a platform stacks on top of another, we don't want it to continue to accelerate downwards
                    if (obj1 is Platform) obj1.setGravity(false);
                    // if a player hits a platform with more than a certain fallspeed, he dies on impact.
                    break;
                case 4:                             //obj1's left coollides with obj2's right,  move obj1 right and set Xvel to 0
                    distX = Math.Abs(obj1Coor.X - (obj2Coor.X + obj2Size.X));
                    obj1.MoveFrame((int)distX, 0);
                    //obj1.setVelocity(0, obj1.velocity.Y);
                    //obj2.setVelocity(0, obj2.velocity.Y);
                
                    break;
                default:
                    throw new Exception("Invalid CollisionState: WorldObjectManager.collisionResolution");
                
            }
        }
    }
}
