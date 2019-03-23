using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace BulletHell4_0
{
    static class AudioMgr
    {
        static public AudioSource MainSource;

        static Dictionary<string, AudioClip> clips;
        static AudioClip mainClip;

        static AudioMgr()
        {
            clips = new Dictionary<string, AudioClip>();
            MainSource = new AudioSource();
            //MainSource.Volume *= 0.3f;            

            AddClip("laser", "Assets/Audio/laser4.wav");
            AddClip("explosion", "Assets/Audio/explosion3.wav");
            AddClip("Game_ST", "Assets/Audio/FrozenJam_Loop.wav");
            AddClip("Menu_ST", "Assets/Audio/Where's my Spaceship.wav");
            AddClip("crash", "Assets/Audio/crash_0.wav");
            //AddClip("", "Assets/Audio/");
            mainClip = GetClip("Game_ST");
        }


        static public void AddClip(string filename, string file)
        {
            if (!clips.ContainsKey(filename))
            {
                clips.Add(filename, new AudioClip(file));
            }
        }

        static public AudioClip GetClip(string filename)
        {
            return clips.ContainsKey(filename) ? clips[filename] : null;
        }

        static public void RemoveClip(string filename)
        {
            if (clips.ContainsKey(filename))
            {
                clips.Remove(filename);
            }
        }

        static public void RemoveAll()
        {
            if (clips.Count > 0)
                clips.Clear();
        }

        static public void StopBGMClip()
        {
            MainSource.Stop();
        }

        static public void SetVolume(float volume = 1)
        {
            MainSource.Volume = volume;
        }

        
        static public void Update(string themeName)
        {
            MainSource.Stream(GetClip(themeName), Game.window.deltaTime);            
        }
    }
}
