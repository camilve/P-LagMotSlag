using UnityEngine.Audio;
using UnityEngine;
namespace SoundSpace
{
    /// <summary>
    /// Help class to get the names, etc. of the sounds.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        public bool loop;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]
        public float pitch;

        [HideInInspector]
        public AudioSource source;
    }
}
