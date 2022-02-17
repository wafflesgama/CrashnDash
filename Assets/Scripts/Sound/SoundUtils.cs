using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSoundManager;

public static class SoundUtils
{
    [Serializable]
    public struct Sound
    {
        [Range(0, 3)] public float volume;
        public AudioClip clip;
    }

    public static void PlaySound(this AudioSource source, Sound sound)
    {
        source.PlayOneShot(sound.clip, sound.volume);
    }
}
