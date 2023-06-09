using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnlineGameTest {
    public class GroundSensor : MonoBehaviour {
        [SerializeField] private GameObject _model;
        [SerializeField] private Animator _animator;


        public CapsuleCollider capsuleCollider;
        private Vector3 _capColTopControlPoint;
        private Vector3 _capColBottomControlPoint;
        private float _capColRadius;
        [SerializeField] private float _colliderOffset = -1f;


        private bool _isGrounded;

        public bool IsGrounded {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        private void Start() {
            _animator = _model.GetComponent<Animator>();

            _capColRadius = capsuleCollider.radius;
        }

        private void FixedUpdate() {
            // Sensor - Ground
            _capColTopControlPoint = transform.position + transform.up * capsuleCollider.height - transform.up *
                (_capColRadius + _colliderOffset);
            _capColBottomControlPoint = transform.position + transform.up * (_capColRadius + _colliderOffset);
            Collider[] outputCols = Physics.OverlapCapsule(
                point0: _capColTopControlPoint,
                point1: _capColBottomControlPoint,
                radius: _capColRadius,
                layerMask: LayerMask.GetMask("Ground")
            );

            IsGrounded = outputCols.Length != 0;
            _animator.SetBool(CharacterAnimationString.IsGrounded, IsGrounded);
        }
    }
}