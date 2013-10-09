using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using PsychicNinja.Data.Util;

/// <summary>
/// Component that handles 2D animation based on multiple frames 
/// held within a single texture.
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
    /// Texture cintaining the animation frames.
    /// </summary>
    private List<Texture2D> frames;
    private Texture2D currFrame;
    private int iterator = 0;
    private int frameDelay = 1;

    //Bad hack for animating command icons (we can't use Gametime because gametime = 0 when the game is paused
    private int animationCounter = 0;


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
    public AnimationComponent(List<Texture2D> Frames,int FrameX, int FrameY, int frameWidth, int frameHeight, int FD)
    {
        paused = true;
        frameY = FrameY;
        frameX = FrameX;
        currFrame = Frames[0];
        frameDelay = FD;

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
        currFrame = frames[0];
    }


    /// <summary>
    /// Handle updating the animation.
    /// </summary>
    /// <param name="gameTime">Current game time.</param>
    public void Update(int gameTime)
    {
        if (gameTime == 0) reset();
        if (gameTime == lastUpdateTime) return;
        if (gameTime % frameDelay == 0)
        {
            currFrame = frames[iterator];
            iterator++;
            if (iterator == frames.Count)
                iterator = 0;
        }
        lastUpdateTime = gameTime;
    }

    public void CommandUpdate()
    {
        animationCounter++;
        if (animationCounter % 10 == 0)
        {
            if (iterator == frames.Count)
                iterator = 0;
            animationCounter = 0;
            currFrame = frames[iterator];
            iterator++;
        }
    }

    /// <summary>
    ///     //Check to see if non-looping animation is on its last frame, thus completed
    /// </summary>
    public Boolean animationComplete()
    {
        if (iterator == frames.Count-1)
            return true;
        return false;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle rect, SpriteEffects effect)
    {
        //spriteBatch.Draw(currFrame, rect, null, Color.White, 0, Vector2.Zero, effect, 0);
        Draw(spriteBatch, rect, effect, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle rect, SpriteEffects effect, Color drawColor)
    {
        //if(myEffect.Equals(DrawToolEffect.Glow))
        //    spriteBatch.Draw(currFrame, rect, null, new Color(255, 255, 255, Math.Abs(((lastUpdateTime*4) % 510) - 255)), 0, Vector2.Zero, effect, 0);
        //else
            spriteBatch.Draw(currFrame, rect, null, drawColor, 0, Vector2.Zero, effect, 0);
    }

}