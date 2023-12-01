using System.Collections;
using cakeslice;
using UnityEngine;

namespace DefaultNamespace.Museum
{
    public class InteractableObjectBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        private Outline _outline;
        private OutlineEffect _outlineEffect;
        private static bool _bLock;
        private IEnumerator _thread;
        public static InteractableObjectBehaviour Instance;

        private void Start()
        {
            _outline = GetComponent<Outline>();
            _outlineEffect = Camera.main.GetComponent<OutlineEffect>();
            _outline.eraseRenderer = true;
        }

        public AudioClip GetAudioClip()
        {
            if (clip != null)
            {
                return clip;
            }
            return null;
        }

        private void OnPointerEnter()
        {
            _outline.eraseRenderer = false;
            Instance = this;
            if (!_bLock)
            {
                if (_thread != null)
                {
                    StopCoroutine(_thread);
                }

                _thread = LineFade(1f);
                StartCoroutine(_thread);
            }
        }

        private void OnPointerExit()
        {
            _outline.eraseRenderer = true;
            Instance = null;
        }

        IEnumerator LineFade(float time)
        {
            _bLock = true;
            float t = 0;
            while (t < time)
            {
                t += Time.deltaTime;
                if (t < time / 2)
                {
                    _outlineEffect.fillAmount = 0.3f * (t / (time / 2f));
                }
                else
                {
                    _outlineEffect.fillAmount = 0.3f * (1 - (t - time / 2f) / (time / 2f));
                }

                yield return null;
            }

            _outline.eraseRenderer = true;
            _bLock = false;
        }
    }
}