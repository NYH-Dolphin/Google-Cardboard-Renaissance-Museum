using System.Collections;
using cakeslice;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

namespace DefaultNamespace.Museum
{
    [RequireComponent(typeof(VideoPlayer))]
    public class InteractableVideoBehaviour : InteractableBehaviour
    {
        [SerializeField] private GameObject light;
        private bool bPlay;
        private VideoPlayer _videoPlayer;

        protected void Start()
        {
            base.Start();
            light.SetActive(false);
            _videoPlayer = GetComponent<VideoPlayer>();
            _videoPlayer.Pause();
        }

        public bool TriggerVideo()
        {
            if (!bPlay)
            {
                light.SetActive(true);
                _videoPlayer.Play();
                bPlay = true;
            }
            else
            {
                light.SetActive(false);
                _videoPlayer.Pause();
                bPlay = false;
            }

            return bPlay;
        }
    }
}