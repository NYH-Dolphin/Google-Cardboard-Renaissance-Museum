using SoundEffects;
using UnityEngine;

namespace Environment
{
    public class CarBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                SoundManager.Instance.car.Play();
                PlayerController.Instance.OnLose();
                Destroy(gameObject);
            }
        }
    }
}