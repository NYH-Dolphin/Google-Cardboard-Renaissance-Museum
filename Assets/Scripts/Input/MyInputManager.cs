﻿using System;
using DefaultNamespace.Museum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class MyInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject camera;
        [SerializeField] private AudioSource speech;


        private InputController _inputs;


        private void OnEnable()
        {
            if (_inputs == null)
            {
                _inputs = new InputController();
            }

            _inputs.Control.Enable();
            _inputs.Museum.Enable();
            
            _inputs.Museum.Touch.performed += OnTouchPerformed;
            _inputs.Museum.Touch.canceled += OnTouchCanceled;
            _inputs.Museum.Click.performed += OnClickPerformed;

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
            TouchUpdate();
            RotationUpdate();
        }

        private void OnClickPerformed(InputAction.CallbackContext value)
        {
            PlaySpeech();
        }
        
        void PlaySpeech()
        {
            if (InteractableObjectBehaviour.Instance != null)
            {
                if (speech.clip != null)
                {
                    speech.Stop();
                }
                AudioClip clip = InteractableObjectBehaviour.Instance.GetAudioClip();
                speech.clip = clip;
                speech.Play();
            }
        }
        
        
        

        #region [Movement]

        [SerializeField] private float fMoveSpeed = 5f;
        private Vector3 _vecMovDir;



        

        private void TouchUpdate()
        {
            if (_bTouch)
            {
                Vector3 move = camera.transform.forward;
                player.GetComponent<Rigidbody>().velocity = move * fMoveSpeed;
            }
        }


        private bool _bTouch;

        private void OnTouchPerformed(InputAction.CallbackContext value)
        {
            _bTouch = true;
        }
        
        

        private void OnTouchCanceled(InputAction.CallbackContext value)
        {
            _bTouch = false;
        }

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
            Vector3 moveF = transform.forward * _vecMovDir.z;
            Vector3 moveR = transform.right * _vecMovDir.x;
            Vector3 movement = moveF + moveR;
            player.GetComponent<Rigidbody>().velocity = movement * fMoveSpeed;
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