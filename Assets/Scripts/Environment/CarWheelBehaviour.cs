using UnityEngine;

namespace Environment
{
    public class CarWheelBehaviour : MonoBehaviour
    {
        public float fRotSpeed = 400f;

        void Update()
        {
            if (!PlayerController.Instance.bLose)
            {
                transform.Rotate(fRotSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}