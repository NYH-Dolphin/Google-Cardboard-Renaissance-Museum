using System.Collections;
using cakeslice;
using UnityEngine;

namespace DefaultNamespace.Museum
{
    public class InteractableObjectBehaviour : InteractableBehaviour
    {
        [SerializeField] private AudioClip clip;


        public AudioClip GetAudioClip()
        {
            if (clip != null)
            {
                return clip;
            }
            return null;
        }
    }
}