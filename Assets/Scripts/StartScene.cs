using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class StartScene : MonoBehaviour
    {
        private InputController _inputs;
        private void OnEnable()
        {
            if (_inputs == null)
            {
                _inputs = new InputController();
            }

            _inputs.ParkourGame.Enable();
            _inputs.ParkourGame.Start.performed += OnStartPerformed;
        }

        private void OnStartPerformed(InputAction.CallbackContext value)
        {
            _inputs.ParkourGame.Disable();
            SceneManager.LoadScene("Game");
        }

    }
}