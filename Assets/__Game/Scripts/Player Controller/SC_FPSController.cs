﻿// Modified from: https://sharpcoderblog.com/blog/unity-3d-fps-controller
namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]

    public class SC_FPSController : MonoBehaviour, IPlayerController
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        public float rightStickLookSpeed = 10f;

        private const string jumpInput = "Jump";
        private const string horizontalInputAxis = "Horizontal";
        private const string verticalInputAxis = "Vertical";
        private const string mouseXInput = "Mouse X";
        private const string mouseYInput = "Mouse Y";
        private const string rightStickXInput = "Right Stick X";
        private const string rightStickYInput = "Right Stick Y";

        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [HideInInspector]
        public bool canMove = true;

        void Start()
        {
            characterController = GetComponent<CharacterController>();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis(verticalInputAxis) : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis(horizontalInputAxis) : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton(jumpInput) && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                // Get right stick input and account for drift when stick is neutral:
                var rightStickY = Input.GetAxis(rightStickYInput);
                var rightStickX = Input.GetAxis(rightStickXInput);
                if (Mathf.Abs(rightStickY) < 0.1f) rightStickY = 0;
                if (Mathf.Abs(rightStickX) < 0.1f) rightStickX = 0;
                var lookY = (-Input.GetAxis(mouseYInput) * lookSpeed) + (-rightStickY * rightStickLookSpeed);
                var lookX = (Input.GetAxis(mouseXInput) * lookSpeed) + (rightStickX * rightStickLookSpeed);
                rotationX += lookY;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, lookX, 0);
            }
        }

        #region IPlayerController Implementation

        public void DetectedByGuard()
        {
            //[TODO]
        }

        public void Die()
        {
            //[TODO]
        }

        #endregion
    }
}