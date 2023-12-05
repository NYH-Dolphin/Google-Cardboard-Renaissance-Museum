using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using cakeslice;

namespace DefaultNamespace.Museum
{
    [RequireComponent(typeof(Outline))]
    public class InteractableBehaviour : MonoBehaviour
    {
        
        protected Outline _outline;
        protected OutlineEffect _outlineEffect;
        private static bool _bLock;
        private IEnumerator _thread;
        public static InteractableBehaviour Instance;
        
        protected void Start()
        {
            _outline = GetComponent<Outline>();
            _outlineEffect = Camera.main.GetComponent<OutlineEffect>();
            _outline.eraseRenderer = true;
        }
        
        
        protected virtual void OnPointerEnter()
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

        protected virtual void OnPointerExit()
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