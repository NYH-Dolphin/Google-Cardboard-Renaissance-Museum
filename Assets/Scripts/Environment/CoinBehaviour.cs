using System;
using SoundEffects;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Collider))]
    public class CoinBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                PlayerController.Instance.AddOneCoin();
                SoundManager.Instance.coin.Play();
                Destroy(gameObject);
            }
        }
    }
}