using System;
using System.Collections;
using UnityEngine;

namespace Environment
{
    public class ObjectGenerator : MonoBehaviour
    {
        [SerializeField] private bool bRegenerated; // reuse the object or not

        public static float fSpeed = 5f;


        private void Start()
        {
            StartCoroutine(AddSpeed());
        }

        IEnumerator AddSpeed()
        {
            while (true)
            { 
                fSpeed += Time.deltaTime / 500f;
                yield return null;
            }
        }


        private static float fEnd = -84;
        private static float fStart = 48;


        private void Update()
        {
            transform.Translate(-transform.forward * (fSpeed * Time.deltaTime));
            if (transform.position.z <= fEnd)
            {
                if (bRegenerated)
                {
                    Vector3 pos = transform.position;
                    pos.z = fStart;
                    transform.position = pos;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}