using System;
using UnityEngine;

namespace MTProject.PlayerController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;

        private static int inputXHash = Animator.StringToHash("InputX");
        private static int inputYHash = Animator.StringToHash("InputY");
        private static int inputMagnitudeHash = Animator.StringToHash("InputMagnitude");

        private Vector3 _currentBlendInput = Vector3.zero;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        public void UpdateAnimationState()
        {
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MoveInput*1.5f : _playerLocomotionInput.MoveInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);
            
            _animator.SetFloat(inputXHash,_currentBlendInput.x);
            _animator.SetFloat(inputYHash,_currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash,_currentBlendInput.magnitude);
        }
    }
}
