using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using PsychicNinja.Data.Object;
using Microsoft.Xna.Framework.Content;

namespace PsychicNinja.Data.Util
{

    static class Spotlight
    {
        private const float ZoomStep = 0.015f;

        private const int LEASH = 0;
        public static GameState oldState;
        //public static WorldObject target;

        public static Vector2 middle = new Vector2(400,240);


        public static Vector2 offset = new Vector2();

        private static float zoom = 1f;
        private static float maxZoom = 0.5f;
        private static float minZoom = 1f;

        public static Vector2 stageCorner = new Vector2();

        private static bool SavedDataIsValid = false;
        private static Vector2 savedOffset = new Vector2();
        private static float savedZoom = 1f;


        private const int screenWidth = 800;
        private const int screenHeight = 480;

        private static bool fastForwarding = false;

        public static Rectangle viewArea;


        /// <summary>
        /// Update the position and zoom state of this object if necessary.
        /// </summary>
        /// <param name="state"></param>
        public static void Update(GameState state)
        {            
            UpdateSpotlightMotionTransition();

            shouldFollowFocusObject = (state == GameState.StateRunning);
            UpdateObjectFocus();

            if (state != GameState.StateRunning) { fastForwarding = false; }

            #region Old Keyboard Controls
            /*
            if (state == GameState.StateRunning || state == GameState.StateWorld)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Z)) { AdjustStageZoomLevel(true); }
                if (Keyboard.GetState().IsKeyDown(Keys.X)) { AdjustStageZoomLevel(false); }
            }
            */
            /*
            if (state == GameState.StateWorld) 
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) { offset.X += 5; }
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) { offset.X -= 5; }
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) { offset.Y -= 5; }
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) { offset.Y += 5; }
            }
            */
            #endregion

            EnforceStageBoundaries();
            oldState = state;
            viewArea.X = (int)offset.X;
            viewArea.Y = (int)offset.Y;
            viewArea.Width = (int)(800 / zoom);
            viewArea.Height = (int)(480 / zoom);

        }

        public static void loadOffset()
        {
            if (!SavedDataIsValid) return;
            offset = savedOffset;
            zoom = savedZoom;
            savedOffset = Vector2.Zero;
            savedZoom = 0.0f;
            SavedDataIsValid = false;
        }

        public static void saveOffset() 
        {
            savedOffset = offset;
            savedZoom = zoom;
            SavedDataIsValid = true;
        }


        #region Spotlight Focus

        private static WorldObject currentFocusTarget = null;
        private static WorldObject savedFocusTarget;
        private static bool shouldFollowFocusObject = false;

        public static void SetSpotlightFocus(WorldObject obj)
        {
            currentFocusTarget = obj;
        }


        private static void UpdateObjectFocus()
        {
            if (currentFocusTarget == null) return;

            if (!shouldFollowFocusObject) return;

            if (currentFocusTarget.drawRect.Left > (offset.X + middle.X + LEASH)) offset.X = currentFocusTarget.drawRect.Left - middle.X - LEASH;
            if (currentFocusTarget.drawRect.Left < (offset.X + middle.X - LEASH)) offset.X = currentFocusTarget.drawRect.Left - middle.X + LEASH;
            if (currentFocusTarget.drawRect.Top > (offset.Y + middle.Y + LEASH)) offset.Y = currentFocusTarget.drawRect.Top - middle.Y - LEASH;
            if (currentFocusTarget.drawRect.Top < (offset.Y + middle.Y - LEASH)) offset.Y = currentFocusTarget.drawRect.Top - middle.Y + LEASH;

        }

        public static void TemporaryZoomOnObject(WorldObject newFocus, Vector2 fingerLocation)
        {
            if (zoom < 1.3f)
            {
                savedZoom = zoom;
                minZoom = 1.3f;
                zoom = 1.3f;
                savedFocusTarget = currentFocusTarget;
                currentFocusTarget = newFocus;
                Point bounds = newFocus.Center();
                offset = new Vector2(bounds.X - (fingerLocation.X/zoom), bounds.Y - (fingerLocation.Y/zoom));
                currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                
            }
            else
            {
                if (fingerLocation.X < 50)
                {
                    offset.X -= 15;
                }
                if (fingerLocation.X > 750)
                {
                    offset.X += 15;
                }
                if (fingerLocation.Y < 50)
                {
                    offset.Y -= 15;
                }
                if (fingerLocation.Y > 430)
                {
                    offset.Y += 15;
                }
                EnforceStageBoundaries();
            }
        }

        public static void EndTemporaryObjectZoom(WorldObject focus)
        {
            if (currentFocusTarget != focus) return;

            Point center = focus.Center();
            Point oldScreenLoc = Spotlight.TranslateWorldPointToScreenPoint(center);

            minZoom = 1f;
            zoom = savedZoom;

            offset = new Vector2(center.X - (oldScreenLoc.X / zoom), center.Y - (oldScreenLoc.Y / zoom));

            currentFocusTarget = savedFocusTarget;
            savedFocusTarget = null;
        }

        #endregion

        #region Touch Handling

        private static float lastDeltaSpace = 0;
        /// <summary>
        /// Process a gesture sample. 
        /// </summary>
        /// <param name="gesture"></param>
        public static void ProcessTouch(GestureSample gesture)
        {
            switch (gesture.GestureType)
            {
                case GestureType.Pinch:
                    float deltaSpace = Vector2.Distance(gesture.Position, gesture.Position2);
                    if (lastDeltaSpace != 0)
                    {
                        if (lastDeltaSpace == deltaSpace) return;
                        AdjustStageZoomLevel(lastDeltaSpace > deltaSpace);
                    }
                    lastDeltaSpace = deltaSpace;
                    break;
                case GestureType.FreeDrag:
                    offset -= gesture.Delta;
                    EnforceStageBoundaries();
                    currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                    break;
                case GestureType.Tap:
                    currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                    break;
                case GestureType.Flick:
                    ExecuteViewMotionTransition(SpotlightMotionTransition.FlickToScroll, gesture.Delta);
                    break;
                case GestureType.DoubleTap:
                    ExecuteViewMotionTransition(SpotlightMotionTransition.FullZoomAroundPoint, gesture.Position);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region SpotlightMotionTransitions

        private static SpotlightMotionTransition currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
        private static Vector2 ViewMotionInformationVector = Vector2.Zero;
        private static bool ViewMotionTransitionZoomingIn = true;

        /// <summary>
        /// Update the SpotlightMotionTransition if one is active
        /// </summary>
        private static void UpdateSpotlightMotionTransition()
        {
            switch (currentSpotlightAnimatedTransition)
            {
                case SpotlightMotionTransition.None:
                    return;
                case SpotlightMotionTransition.WorldViewToPlayer:
                    miniAdjustStageZoomLevel(!ViewMotionTransitionZoomingIn);
                    offset = Vector2.Lerp(offset, ViewMotionInformationVector, 0.5f);
                    if (zoom <= maxZoom) ViewMotionTransitionZoomingIn = true;
                    if (zoom >= minZoom) currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                    break;
                case SpotlightMotionTransition.FlickToScroll:
                    offset = Vector2.Lerp(offset, ViewMotionInformationVector, 0.1f);
                    if (Vector2.Distance(offset, ViewMotionInformationVector) < 5.0f)
                        currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                    break;

                case SpotlightMotionTransition.FullZoomAroundPoint:
                    AdjustStageZoomLevel(ViewMotionTransitionZoomingIn);
                    offset += ViewMotionInformationVector;
                    if (zoom == maxZoom || zoom == minZoom) currentSpotlightAnimatedTransition = SpotlightMotionTransition.None;
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Set the current spotlight animation transition.
        /// Make sure when calling this that it is in an appropriate context and will not cause a jarring reset on animated transition change.
        /// </summary>
        /// <param name="transition">Enumerated value denoting what transition to perform.</param>
        public static void ExecuteViewMotionTransition(SpotlightMotionTransition transition, Vector2 modification)
        {
            currentSpotlightAnimatedTransition = transition;
            
            // Perform Conditional Transition Setup
            switch (transition)
            {
                case SpotlightMotionTransition.WorldViewToPlayer:
                    offset = modification;
                    zoom = minZoom;
                    EnforceZoomLimits();
                    EnforceStageBoundaries();
                    // redundancy protection
                    SavedDataIsValid = false;
                    ViewMotionTransitionZoomingIn = false;
                    if (currentFocusTarget != null)
                        ViewMotionInformationVector = currentFocusTarget.Position();
                    break;

                case SpotlightMotionTransition.FlickToScroll:
                    modification.Normalize();
                    modification *= (400 / zoom);
                    ViewMotionInformationVector = offset - modification;
                    break;

                case SpotlightMotionTransition.FullZoomAroundPoint:
                    ViewMotionTransitionZoomingIn = (zoom > (minZoom + maxZoom) / 2);
                    int numSteps = (int)((ViewMotionTransitionZoomingIn) ? (zoom - maxZoom) / ZoomStep : (minZoom - zoom) / ZoomStep);
                    Vector2 center = offset + new Vector2(halfScreenWidth, halfScreenHeight);
                    ViewMotionInformationVector = (TranslateScreenVectorToWorldVector(modification) - center) / numSteps;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Getters

        public static float getScale() 
        {
            return zoom;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Change the current zoom level, and bound it to acceptable limits.
        /// </summary>
        /// <param name="shouldZoomIn">Decrease Zoom level. (Zoom out)</param>
        public static void AdjustStageZoomLevel(bool shouldZoomOut) 
        {
            if (shouldZoomOut) 
            { 
                zoom -= ZoomStep; 
            }
            else 
            { 
                zoom += ZoomStep;
            }
            EnforceZoomLimits();
        }

        public static void miniAdjustStageZoomLevel(bool shouldZoomOut)
        {
            if (shouldZoomOut)
            {
                zoom -= (ZoomStep / 3);
            }
            else
            {
                zoom += (ZoomStep / 3);
            }
            EnforceZoomLimits();
        }
        /// <summary>
        /// Set the bottom right corner of the stage to the given point (for scrolling limitations)
        /// </summary>
        /// <param name="stage"></param>
        public static void setStage(Vector2 stage) 
        { 
            stageCorner = stage;
        }

        public static void SetMaxZoom(float zoom)
        {
            maxZoom = zoom;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Enables Fast Forwarding, or prolongs its duration if already active. Called when the fast-forward button is touched.
        /// </summary>
        public static void toggleFastForward() {
            fastForwarding = !fastForwarding;
        }

        public static double getNewFPS() {
            double FPS = 30.0;
            if (oldState == GameState.StateRunning && fastForwarding) { FPS = 90.0; }
            return (1000.0 / FPS);
        }

        /// <summary>
        /// Readjusts spotlight so that
        /// 1) We don't zoom in or out too far.
        /// 2) We don't scroll too far.
        /// </summary>
        private static void EnforceStageBoundaries()
        {
            /*double tempX = ((offset.X + middle.X) * zoom) - middle.X;
            double tempY = ((offset.Y + middle.Y) * zoom) - middle.Y;
            if (tempX < 0) offset.X = (float)(((middle.X - 0) / zoom) - middle.X);
            if (tempY < 0) offset.Y = (float)(((middle.Y - 0) / zoom) - middle.Y);
            if (tempX > (((stageCorner.X * zoom) - screenWidth))) offset.X = (float)((stageCorner.X - middle.X - (middle.X / zoom)));
            if (tempY > (((stageCorner.Y * zoom) - screenHeight))) offset.Y = (float)((stageCorner.Y - middle.Y - (middle.Y / zoom)));*/

            float horizontalScrollLimit = stageCorner.X - (screenWidth / zoom);
            float verticalScrollLimit = stageCorner.Y - (screenHeight / zoom);

            offset.X = Math.Min(offset.X, horizontalScrollLimit);
            offset.Y = Math.Min(offset.Y, verticalScrollLimit);

            offset.X = Math.Max(offset.X, 0);
            offset.Y = Math.Max(offset.Y, 0);

        }

        private static void EnforceZoomLimits()
        {
            zoom = Math.Max(zoom, maxZoom);
            zoom = Math.Min(zoom, minZoom);
        }

        /// <summary>
        /// Builds min and max zoom limits from the current level and screen size. 
        /// </summary>
        private static void CalculateZoomLimits()
        {
            float x = screenWidth / (stageCorner.X);
            float y = screenHeight / (stageCorner.Y);

            maxZoom = (x > y)? x : y;
            
            if (minZoom < maxZoom) { maxZoom = minZoom; }
        }


        #endregion

        #region Conversions

        static int halfScreenWidth = screenWidth / 2;
        static int halfScreenHeight = screenHeight / 2;

        #region Screen To World
        /// <summary>
        /// Translates a point (from a touch) into a location in the game world.
        /// </summary>
        /// <param name="pointInScreenSpace"></param>
        /// <returns></returns>
        public static Point TranslateScreenPointToWorldPoint(Point pointInScreenSpace)
        {
            int xval = (int)((pointInScreenSpace.X / zoom) + offset.X);
            int yval = (int)((pointInScreenSpace.Y / zoom) + offset.Y);

            return new Point(xval, yval);
        }

        /// <summary>
        /// Translaters a vector (from a touch) into a location in the game world.
        /// </summary>
        /// <param name="vectorInScreenSpace"></param>
        /// <returns></returns>
        public static Point TranslateScreenVectorToWorldPoint(Vector2 vectorInScreenSpace)
        {
            int xval = (int)((vectorInScreenSpace.X / zoom) + offset.X);
            int yval = (int)((vectorInScreenSpace.Y / zoom) + offset.Y);

            return new Point(xval, yval);
        }

        /// <summary>
        /// Translates a Screen Point into a world vector
        /// </summary>
        /// <param name="pointInScreenSpace"></param>
        /// <returns></returns>
        public static Vector2 TranslateScreenPointToWorldVector(Point pointInScreenSpace)
        {
            float xval = (pointInScreenSpace.X / zoom) + offset.X;
            float yval = (pointInScreenSpace.Y / zoom) + offset.Y;

            return new Vector2(xval, yval);
        }

        /// <summary>
        /// Translates a Screen Vector into a world Vector
        /// </summary>
        /// <param name="vectorInScreenSpace"></param>
        /// <returns></returns>
        public static Vector2 TranslateScreenVectorToWorldVector(Vector2 vectorInScreenSpace)
        {
            //float xval = ((vectorInScreenSpace.X - halfScreenWidth) / zoom) + offset.X + halfScreenWidth;
            //float yval = ((vectorInScreenSpace.Y - halfScreenHeight) / zoom) + offset.X + halfScreenHeight;
            float xval = (vectorInScreenSpace.X / zoom) + offset.X;
            float yval = (vectorInScreenSpace.Y / zoom) + offset.Y;

            return new Vector2(xval, yval);
        }

        #endregion

        public static Point TranslateWorldPointToScreenPoint(Point pointInWorldSpace)
        {
            int xVal = (int)((pointInWorldSpace.X - offset.X) * zoom);
            int yVal = (int)((pointInWorldSpace.Y - offset.Y) * zoom);
            return new Point(xVal, yVal);
        }

        public static Vector2 TranslateWorldVectorToScreenVector(Vector2 vectorInWorldSpace)
        {
            //float xval = ((vectorInScreenSpace.X - halfScreenWidth) / zoom) + offset.X + halfScreenWidth;
            //float yval = ((vectorInScreenSpace.Y - halfScreenHeight) / zoom) + offset.X + halfScreenHeight;
            float xval = (vectorInWorldSpace.X - offset.X) * zoom;
            float yval = (vectorInWorldSpace.Y - offset.Y) * zoom;

            return new Vector2(xval, yval);
        }

        /// <summary>
        /// special method used exclusively in CommandMenu for drawing dragged rectangle
        /// </summary>
        /// <param name="rectangleInWorldSpace"></param>
        /// <returns></returns>
        public static Rectangle TranslateWorldRectangleToScreenRectangle(Rectangle rectangleInWorldSpace)
        {
            int left = (int)((rectangleInWorldSpace.Left - offset.X) * zoom);
            int top = (int)((rectangleInWorldSpace.Top - offset.Y) * zoom);
            int width = (int)((rectangleInWorldSpace.Width * zoom));
            int height = (int)((rectangleInWorldSpace.Height * zoom));

            return new Rectangle(left, top, width, height);
        }

        #endregion

        #region Old
        /*public static void MoveSpotlightToInclude(PsychicNinja.Data.WorldObject.WorldObject star) {
            //Point starPOS = TranslateScreenPointToWorld(star.drawRect.Location);
            Rectangle starRect = star.modifiedDrawRect();
            if (starRect.X < 0) offset.X += (0-starRect.X);

        }*/
        /*public static Rectangle modifiedDrawRect(Rectangle rect)
        {
            Vector2 temp = Spotlight.getOffset();
            double scale = Spotlight.getScale();
            double rectX = ((rect.X - temp.X - 400) * scale) + 400; // adjusts for the offset 
            double rectY = ((rect.Y - temp.Y - 240) * scale) + 240; // and zoom rate
            return new Rectangle((int)rectX, (int)rectY, (int)(rect.Width * scale), (int)(rect.Height * scale));
        }*/
        #endregion

        public static void SetZoomedOut()
        {
            zoom = maxZoom;
            SavedDataIsValid = false;
        }
    }
}
