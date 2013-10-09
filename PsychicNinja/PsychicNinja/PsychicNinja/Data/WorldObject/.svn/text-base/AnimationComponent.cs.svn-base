using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


/// </summary>
public class AnimationComponent
{
    /// <summary>
    /// Screen position of the animation top-right corner.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// Whether or not the animation will repeat when completed.
    /// </summary>
    public bool Loop;

    /// <summary>
    /// Default delay between animation frames (in milliseconds).
    /// </summary>
    public int FrameDelay;

    /// <summary>
    /// Texture cintaining the animation frames.
    /// </summary>
    List<Texture2D> frames;
    Texture2D currFrame;
    int iterator = 0;

    /// <summary>
    /// Path to the texture to animate.
    /// </summary>
    string texturePath;


    //Top left corner of Frame
    int frameX;
    int frameY;
    //Width and Height of Frame
    int width;
    int height;


    /// <summary>
    /// Whether or not the animation is running.
    /// </summary>
    bool paused;

    int lastUpdateTime;

    /// <summary>
    /// Constructor, takes in List of frame textures and proportions
    /// </summary>
    public AnimationComponent(List<Texture2D> Frames,int FrameX, int FrameY, int frameWidth, int frameHeight)
    {
        paused = true;
        FrameDelay = 66;
        frameY = FrameY;
        frameX = FrameX;
        currFrame = Frames[0];

        lastUpdateTime = 0;

        // save the texture information
        width = frameWidth;
        height = frameHeight;
        frames = Frames;
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    /// <remarks>Resets the animation to the first frame if it was
    /// not already playing.</remarks>
    public void Play()
    {
        if (paused)
        {
            frameX = frameY = 0;
        }
        paused = false;
    }

    /// <summary>
    /// Pause the animation at the current frame.
    /// </summary>
    public void Pause()
    {
        paused = true;
    }

    /// <summary>
    /// Reset the animation's frame to the default first frame
    /// </summary>
    public void reset()
    {
        iterator = 0;
    }


    /// <summary>
    /// Handle updating the animation.
    /// </summary>
    /// <param name="gameTime">Current game time.</param>
    public void Update(int gameTime)
    {
        if (gameTime == 0) reset();
        if (gameTime == lastUpdateTime) return;
        if (gameTime % 2 == 0)
        {
            currFrame = frames[iterator];
            iterator++;
            if (iterator == frames.Count)
                iterator = 0;
        }
        lastUpdateTime = gameTime;
    }

    /// <summary>
    ///     //Check to see if non-looping animation is on its last frame, thus completed
    /// </summary>
    public Boolean animationComplete()
    {
        if (currFrame == frames[frames.Count - 1])
            return true;
        return false;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle rect, SpriteEffects effect)
    {
        spriteBatch.Draw(currFrame, rect, null, Color.White, 0, Vector2.Zero, effect, 0);

    }

}