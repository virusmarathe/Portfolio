//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : WorldObject.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class WorldObject
{
    protected const float gravity = 1;
    protected const float terminalVelocity = 90;

    protected Vector2 pos;
    protected Vector2 velocity;
    protected Vector2 size;
    protected PositionState positionMask;
    protected Texture2D texture;
    protected ObjectShape shape;
    protected float rotationAngle;
    protected bool hasGravity;

    /*!
     * Method       WorldObject
     * Description  Default contsructor for WorldObject. Allows subclasses to do all initialization.
     */
    public WorldObject()
    {
        positionMask = PositionState.NotSet;
    }

    
    public WorldObject(Vector2 Pos, Vector2 Velocity, Vector2 Size, Texture2D tex, ObjectShape Shape)
    {
        pos = Pos;
        velocity = Velocity;
        size = Size;
        positionMask = PositionState.NotSet;
        texture = tex;
        shape = Shape;
        rotationAngle = 0.0f;
    }
    public Vector2 getCoord()
    {
        return pos;       //placeholder code
    }
    public Vector2 getVelocity()
    {
        return velocity;       //placeholder code
    }
    public Vector2 getSize()
    {
        return size;
    }
    public ObjectShape getShape()
    {
        return shape;
    }
    public float getRotationAngle()
    {
        return rotationAngle;
    }
    public void setRotationAngle(float theta)
    {
        rotationAngle = theta;
    }
    public void setCoord(Vector2 Pos)
    {
        pos = Pos;
    }
    public void setVelocity(Vector2 Vel){
        velocity = Vel;
    }
    public void setPositionState(PositionState P)
    {
        positionMask = P;
    }
    public virtual void Update(GameTime gameTime)
    {
        if (hasGravity && velocity.Y < terminalVelocity) velocity.Y += gravity;
        if (hasGravity && velocity.Y > terminalVelocity) velocity.Y = terminalVelocity;
        pos += velocity;
        if (pos.Y + size.Y > 480) { pos.Y = 480 - size.Y; velocity.Y = 0; positionMask = PositionState.OnFloor; }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y), new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y), Color.White, rotationAngle, new Vector2(0, 0), SpriteEffects.None, 0.5f);
    }
}
