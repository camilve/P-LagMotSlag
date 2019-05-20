using UnityEngine.Audio;
using System;
using UnityEngine;
using SoundSpace;

namespace AudioSpace
{
    /// <summary>
    /// AudioManager makes it possible to start and stop songs by their name, and makes it possible to continue a song over several scenes.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        void Awake()
        {
            //check if there exist an instance of AudioManager from before
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                //destroy so that there is only one instance of AudioManager
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        /// <summary>
        /// Finds a song by its name and starts playing it.
        /// </summary>
        /// <param name="name">name of the song</param>
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " is not found");
                return;
            }
            s.source.Play();
        }

        /// <summary>
        /// Finds a song by its name and stop playing it.
        /// </summary>
        /// <param name="name">name of song</param>
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " is not found");
                return;
            }
            s.source.Stop();
        }
    }
}