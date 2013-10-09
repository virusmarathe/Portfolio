using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychicNinja.Data.Util
{
    /// <summary>
    /// Delineates the collision shape of an object.
    /// </summary>
    public enum ObjectShape
    {
        Rectangle,
        Circle
    }

    /// <summary>
    /// Delineates a behavior for a view
    /// </summary>
    public enum ViewAnimationType
    {
        None,
        Slide,
        Blink,
        Glow,
        GrowFade,
        LightSquish,
        AlphaFadeIn,
        Expand
    }

    public enum LayerBehaviorType
    {
        /// <summary>
        /// The background layer moves at the same speed as the interactive layer.
        /// </summary>
        Static,
        /// <summary>
        /// Layer drawn right before interactive/static layers; moves at half speed.
        /// </summary>
        NearBG,
        /// <summary>
        /// Layer drawn right before NearBG layer; moves at third speed.
        /// </summary>
        FarBG,
        /// <summary>
        /// Automated scrolling layer, creates three clouds which move at different speeds.  Drawn before FarBG layer.
        /// </summary>
        Cloud,
        /// <summary>
        /// Automated scrolling layer, moves vertically up.  Drawn before FarBG layer.
        /// </summary>
        GlowingLine,
        /// <summary>
        /// Farthest layer, serves as a general background and does not move.
        /// </summary>
        BackBG,
        Fog,

    }

    public enum SpotlightMotionTransition
    {
        None = 0,
        WorldViewToPlayer,
        FlickToScroll,
        FullZoomAroundPoint,
    }

    /// <summary>
    /// Delineates all sides of an object on which the collision occurs, if any. 
    /// </summary>
    [Flags]
    public enum CollisionType : short
    {
        CollisionNone   = 0,
        CollisionTop    = 1,
        CollisionLeft   = 2,
        CollisionRight  = 4,
        CollisionBottom = 8,
    }

    /// <summary>
    /// Delineates current state of the game.
    /// </summary>
    public enum GameState
    {
        StateLoading,
        StateMenu,
        StateWorld,
        StateRunning,
        StateCompleted,
        StatePaused,
        StateCutscene,
    }

    /// <summary>
    /// Returned by GameUI to indicate actions for Game1
    /// </summary>
    public enum GameButtonPressed
    {
        None = 0,
        StartGame,
        PauseGame,
        StopGame,
        DeleteSelected,
        ShowTutorial,
        ZoomIn,
        ZoomOut,
        FastForward,
    }

    
    public enum GameMenuFlag
    {
        NoFlag,
        LoadDevLevelFlag,
        LoadNextLevel,
        LoadSpecificLevel,
        Replay,
        ToTitleScreen
    }

    /// <summary>
    /// WOM WOM WOM WOM WOM WOM WOM WOM WOM WOM WOM
    /// </summary>
    public enum WOMState
    {
        InLevel,
        Paused,
        LevelComplete,
        RunningLevel,
        StoppedLevel
    }

    /// <summary>
    /// Delineates which action is represented by the command.
    /// </summary>
    public enum CommandType
    {
        MoveLeft = 0,
        MoveRight = 1,
        Jump = 2,
        WallJump = 3,
        WallSlide = 4,
        LedgeClimb = 5,
        ObjectThrow = 6,
        NumCommands = 7
    }

    /// <summary>
    /// Delineates current life state of the entity.
    /// </summary>
    public enum LifeState
    {
        Alive,
        Dying,
        Dead
    }

    /// <summary>
    /// Delineates current position state of the object.
    /// </summary>
    public enum PositionState
    {
        NotSet,
        OnFloor,
        OnWall,
        OnRope,
        InAir,
        DoesNotApply,
    }

    public enum NinjaActionState
    {
        Standing,
        Running,
        Jumping,
        Airborne,
        WallJumping,
        WallClimbing,
        Rolling,
        WallSliding,
        ItemThrowing,
        SwordSwinging,
        SwordRunning,
        ShurikenRunning,
        VictoryPose,
        SquishDying,
        FallDying,
        FireDying,
        ExplodeDying,
        VillainDying,
        WarpingIn,
    }

    public enum EnemyActionState
    {
        Standing,
        Running,
        Attacking,
        Dying,
    }

    /// <summary>
    /// Puts an enumerated name to the enemy type. You MUST specify the int that corresponds to their animation index.
    /// </summary>
    public enum EnemyType
    {
        Goon = 0,
        KnifeArtist = 1,
        Bat = 2,
        Rat = 3,
        NumberOfEnemies = 4,
    }

    /// <summary>
    /// Delineates what type of item the object is.
    /// </summary>
    public enum ItemType
    {
        Shuriken,
        Sword,
        Hookshot
    }

    /// <summary>
    /// Values to be returned by GameMenu.respondToTouch if actions are required by game client.
    /// </summary>
    public enum GameMenuAction
    {
        None,
        LoadLevel,
    }

    /// <summary>
    /// Button values for the Main Game Menu actions.
    /// </summary>
    public enum MenuOptions
    {
        ToNewGame,
        ToChapterSelect,
        ToLevelSelect,
        ToOptions,
        Back,
        BackToChapterSelect
    }

    /// <summary>
    /// Game Menu states.
    /// </summary>
    public enum GameMenuState
    {
        MenuStateMain = 0,
        MenuStateChapterSelect = 1,
        MenuStateLevelSelect = 2,
    }

    public enum SoundEffects
    {
        warpIn,

        //level death sounds
        deathsound1,
        deathsound2,
        splat1,
        levelComplete1,
        levelComplete2,
        throwItem,
        swordCollide,
        cutRope,
        chapterClear
    }

    /// <summary>
    /// Sets a draw style for world object. Defaults to StretchToFit.
    /// </summary>
    public enum WorldObjectDrawStyle
    {
        StretchToFit,
        Tiled,
        Rotating,
    }
}
