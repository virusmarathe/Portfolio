//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : GameMenu.cs
//  @ Date : 1/14/2011
//  @ Author : 
//
//
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using PsychicNinja.Interface.Engine;
using PsychicNinja.Data.Util;

public class GameMenu : View
{
    private static Rectangle position = new Rectangle(0, 0, 800, 480);
    public GameMenuFlag flag;



    public GameMenu()
    {
        flag = GameMenuFlag.NoFlag;
        //Not sure why this doesnt work, but we'll need it eventually
        //System.IO.DirectoryInfo contentDir = new System.IO.DirectoryInfo("Level");
        //chapterFiles = contentDir.GetFiles();
    }

    public static new void LoadContent(ContentManager Content)
    {
        CompletionScreen.LoadContent(Content);
    }

    public override void Update() 
    {
        base.Update();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override bool ProcessTouch(GestureSample gesture)
    {
        return base.ProcessTouch(gesture);
    }
}