using System;
using UnityEngine;

namespace MTProject.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;
        
        [Header("Move Settings")]
        public float runAcceleration;
        public float runSpeed;
        public float drag = .1f;
        
        [Header("CameraSettings")]
        public float lookSensivityH = .1f;
        public float lookSensivityV = 0.1f;
        public float lookLimitV = 60f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private Vector2 _cameraRotation = Vector2.zero;//mevcut kamera ve oyuncu konumu
        private Vector2 _playerTargetRotation = Vector2.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            Vector3 cameraForwardXZ =
                new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z);
            Vector3 cameraRightXZ = 
                new Vector3(_playerCamera.transform.right.x, 0, _playerCamera.transform.right.z);

            Vector3 moveDirection = cameraForwardXZ * _playerLocomotionInput.MoveInput.y +
                                    cameraRightXZ * _playerLocomotionInput.MoveInput.x; // karakterin yönü

            Vector3 moveDelta = moveDirection * runAcceleration * Time.deltaTime; //saniye başı karakterin ivmelenmesi
            Vector3 newVelocity = _characterController.velocity + moveDelta; //karakterin yeni hızı

            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime; // sürtünme
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;//sürtünmeyle birlikte yeni hızımız
            newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);//yeni hızı runspeed ile çarptık

            _characterController.Move(newVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraRotation.x += lookSensivityH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y += Mathf.Clamp(_cameraRotation.y - lookSensivityV * _playerLocomotionInput.LookInput.y,
                -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSensivityH * _playerLocomotionInput.LookInput.x;//oyuncunun kamera ile aynı yöne dönmesi
            transform.rotation = Quaternion.Euler(0, _playerTargetRotation.x, 0);
            
            _playerCamera.transform.rotation=Quaternion.Euler(_cameraRotation.y,_cameraRotation.x,0);
        }
    }
}

