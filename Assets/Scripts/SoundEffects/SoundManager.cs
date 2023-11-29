using System;
using UnityEngine;

namespace SoundEffects
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        public AudioSource coin;
        public AudioSource car;
        public AudioSource move;

    }
}