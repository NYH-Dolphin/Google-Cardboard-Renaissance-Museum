using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class MyInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject camera;


        private InputController _inputs;


        private void OnEnable()
        {
            if (_inputs == null)
            {
                _inputs = new InputController();
            }

            _inputs.Control.Enable();
            _inputs.Control.Movement.performed += OnMovementPerformed;
            _inputs.Control.Movement.canceled += OnMovementCanceled;

            _inputs.Control.Rotation.performed += OnRotationPerformed;
            _inputs.Control.Rotation.canceled += OnRotationCanceled;

            _inputs.Control.DPad.performed += OnDPadPerformed;
            _inputs.Control.NSWE.performed += OnNSWEPerformed;
        }

        private void OnDisable()
        {
            _inputs.Control.Disable();
            _inputs.Control.Movement.performed -= OnMovementPerformed;
            _inputs.Control.Movement.canceled -= OnMovementCanceled;

            _inputs.Control.Rotation.performed -= OnRotationPerformed;
            _inputs.Control.Rotation.performed -= OnRotationCanceled;

            _inputs.Control.DPad.performed -= OnDPadPerformed;
            _inputs.Control.NSWE.performed -= OnNSWEPerformed;
        }

        private void Awake()
        {
            Cursor.visible = false;
        }


        private void Update()
        {
            MovementUpdate();
            RotationUpdate();
        }


        #region [Movement]

        [SerializeField] private float fMoveSpeed = 20f;
        private Vector3 _vecMovDir;

        private void OnMovementPerformed(InputAction.CallbackContext value)
        {
            Vector2 vec2 = value.ReadValue<Vector2>();
            _vecMovDir = new Vector3(vec2.x, 0f, vec2.y);
        }


        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            _vecMovDir = Vector3.zero;
        }

        private void MovementUpdate()
        {
            Vector3 movement = (transform.forward + transform.right).normalized;
            movement.x *= _vecMovDir.x;
            movement.z *= _vecMovDir.z;
            movement.y = 0;
            player.transform.Translate(movement * (fMoveSpeed * Time.deltaTime));
        }

        #endregion


        private void OnDPadPerformed(InputAction.CallbackContext value)
        {
            Vector2 vec2 = value.ReadValue<Vector2>();
        }


        #region [Rotation]

        [SerializeField] private float fRotSpeed = 100f;
        private Vector3 _vecRotDir;
        private readonly float _fRotDamping = 0.1f; // Adjust the damping factor as needed

        private void OnRotationPerformed(InputAction.CallbackContext value)
        {
            Vector2 vec2 = value.ReadValue<Vector2>();
            _vecRotDir = new Vector3(vec2.x, 0f, vec2.y);
        }

        private void OnRotationCanceled(InputAction.CallbackContext value)
        {
            _vecRotDir = Vector3.zero;
        }

        private void RotationUpdate()
        {
            // Calculate the target rotation for the player
            Quaternion playerTargetRotation = Quaternion.Euler(0, _vecRotDir.x * fRotSpeed * Time.deltaTime, 0);

            // Use RotateTowards to smoothly interpolate between current and target rotation for the player
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation,
                player.transform.rotation * playerTargetRotation, _fRotDamping * Mathf.Rad2Deg);

            // Calculate the target rotation for the camera
            Quaternion cameraTargetRotation = Quaternion.Euler(-1 * _vecRotDir.z * fRotSpeed * Time.deltaTime, 0, 0);

            // Use RotateTowards to smoothly interpolate between current and target rotation for the camera
            camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation,
                camera.transform.rotation * cameraTargetRotation, _fRotDamping * Mathf.Rad2Deg);
        }

        #endregion


        private void OnNSWEPerformed(InputAction.CallbackContext value)
        {
            Vector2 vec2 = value.ReadValue<Vector2>();
        }
    }
}