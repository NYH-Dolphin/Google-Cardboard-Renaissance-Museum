using UnityEngine;

namespace SoundEffects
{
    public class BGMBehaviour : MonoBehaviour
    {
        private static BGMBehaviour _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}