using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using PsychicNinja.Data.Util;

namespace PsychicNinja.Logic
{
    public static class MusicManager
    {
        public static Song currentsong;
        public static Song playingsong;

        //sound effects used throughout game

        public static SoundEffect[] sounds;

        public static bool soundEnabled = true;

        public static void SetSong(Song s)
        {
            currentsong = s;
        }
        /// <summary>
        /// loads all the sound effects to be used by the game
        /// </summary>
        /// <param name="c"></param>
        public static void LoadContent(ContentManager c)
        {
            sounds = new SoundEffect[10];
            sounds[(int)SoundEffects.cutRope] = c.Load<SoundEffect>("Sounds/velcro-strap-2");

            sounds[(int)SoundEffects.chapterClear] = c.Load<SoundEffect>("Sounds/44167__Stickinthemud__chimes_6sec_gliss_up");

            sounds[(int)SoundEffects.warpIn] = c.Load<SoundEffect>("Sounds/33637__HerbertBoland__CinematicBoomNormAmpShortened");
            sounds[(int)SoundEffects.levelComplete1] = c.Load<SoundEffect>("Sounds/Whoosh");
            sounds[(int)SoundEffects.levelComplete2] = c.Load<SoundEffect>("Sounds/33380__DJ_Chronos__Boom2");

            sounds[(int)SoundEffects.throwItem] = c.Load<SoundEffect>("Sounds/76417__Benboncan__SwishAmpShortened2");

            sounds[(int)SoundEffects.swordCollide] = c.Load<SoundEffect>("Sounds/109423__Black_Snow__Sword_Slice_14Amp");

            sounds[(int)SoundEffects.deathsound1] = c.Load<SoundEffect>("Sounds/steelswordAmp");
            sounds[(int)SoundEffects.deathsound2] = c.Load<SoundEffect>("Sounds/81042_Rock_Savage_Blood_Hitting_WindowAmpShortened");
            sounds[(int)SoundEffects.splat1] = c.Load<SoundEffect>("Sounds/splat1");

        }

        /// <summary>
        /// loads the music this particular level will use
        /// </summary>
        /// <param name="c"></param>
        /// <param name="songpaths"></param>

        //play whatever SoundEffect is specified
        public static void PlaySoundEffect(SoundEffects name)
        {
            //if(soundEnabled)
                sounds[(int)name].Play();
        }

        public static void StartMusic()
        {
            if (currentsong != playingsong )
            {
                playingsong = currentsong;
                if (soundEnabled)
                {
                    MediaPlayer.Play(currentsong);
                }
            }
        }

        public static void PauseMusic()
        {
            MediaPlayer.Pause();
            soundEnabled = false;
        }

        public static void StopMusic()
        {
            playingsong = null;
            MediaPlayer.Stop();
        }
    }
}
