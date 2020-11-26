using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Player _player;

        private float _moveSpeed = 3f;
        private float _rotateSpeed = 180f; // rotation degree per second
        [SerializeField] private Vector3 _moveDestination;
        private Quaternion _rotationTarget;
        [SerializeField] private Vector3 _previousField;

        [SerializeField] private bool _moveForward = false;
        [SerializeField] private bool _rotate = false;

        // Start is called before the first frame update
        private void Awake()
        {
            _player = GetComponent<Player>();

            _moveDestination = transform.position;
            
            
        }

        // Update is called once per frame
        public void DoMovement()
        {
            if (_moveForward || _rotate)
            {
                _player.PerformingAction = true;
            }

            if (!_player.PerformingAction)
            {
                return;
            }

            if (_moveForward)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    _moveDestination,
                    _moveSpeed * Time.deltaTime
                );

                if (transform.position == _moveDestination)
                {
                    _moveForward = false;
                    _player.PerformingAction = false;
                }
            }

            if (_rotate)
            {
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    _rotationTarget,
                    _rotateSpeed * Time.deltaTime
                );

                if (transform.rotation == _rotationTarget)
                {
                    _rotate = false;
                    _player.PerformingAction = false;
                }
            }
        }

        public void SetRotate(float deg)
        {
            _rotate = true;
            _rotationTarget = Quaternion.Euler(
                0,
                transform.eulerAngles.y + deg,
                0
            );
        }

        public void MoveForward()
        {
            _moveForward = true;
            var forward = transform.forward;
            _previousField = transform.position;

            _moveDestination = new Vector3(
                (float) Math.Round(_moveDestination.x + forward.x, 1),
                0,
                (float) Math.Round(_moveDestination.z + forward.z, 1)
            );
        }

        public void ResetMoveDestination()
        {
            _moveDestination = transform.position;
        }

        public void UndoMovement()
        {
            _moveDestination = _previousField;
            _previousField = transform.position;
            _moveForward = true;
            var dir = _previousField - _moveDestination;
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;
        }
    }
}