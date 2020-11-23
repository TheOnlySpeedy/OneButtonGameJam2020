using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputMaster _controls;

        private bool _isTapped;
        private bool _isHolding;

        // Double Tap Action
        private float _tapTime;
        private float _doubleTapWait = 0;
        private const float DoubleTapWaitThreshold = 0.3f;
        private bool _isDoubleTaping;

        // Hold Action
        [SerializeField] private float _heldTime;
        [SerializeField] private const float HeldTimeThreshold = 0.4f;

        private Player _player;

        private void Awake()
        {
            _controls = new InputMaster();
            _player = GetComponent<Player>();

            _controls.Player.TapAction.performed += _ =>
            {
                _isTapped = true;
                _isHolding = true;
            };

            _controls.Player.TapAction.canceled += _ =>
            {
                _isTapped = false;
                _isHolding = false;
                _heldTime = 0;
            };
        }

        public void CheckInput()
        {
            if (_player.PerformingAction)
            {
                return;
            }

            if (_isHolding)
            {
                _heldTime += Time.deltaTime;
                if (_heldTime >= HeldTimeThreshold)
                {
                    _doubleTapWait = 0;
                    HoldAction();
                }
            }

            if (_isTapped)
            {
                if (Time.time - _tapTime < DoubleTapWaitThreshold)
                {
                    _isDoubleTaping = true;
                    _doubleTapWait = 0;
                    DoubleAction();
                }

                _tapTime = Time.time;

                if (!_isDoubleTaping)
                {
                    _doubleTapWait += Time.deltaTime;
                }
            }

            if (_doubleTapWait > 0 && !_isHolding)
            {
                _doubleTapWait += Time.deltaTime;
                if (_doubleTapWait >= DoubleTapWaitThreshold)
                {
                    _doubleTapWait = 0;
                    _isDoubleTaping = false;
                    SingleAction();
                }
            }

            _isTapped = false;
            _isDoubleTaping = false;
        }

        private void SingleAction()
        {
            if (_player.PerformingAction)
            {
                return;
            }

            _player.Rotate(-90f);
        }

        private void DoubleAction()
        {
            if (_player.PerformingAction)
            {
                return;
            }

            _player.Rotate(90f);
        }

        private void HoldAction()
        {
            if (_player.PerformingAction)
            {
                return;
            }

            if (_player.CanMoveForward)
            {
                _player.PerformingAction = true;
                _player.MoveForward();

                return;
            }

            if (_player.CanAttack)
            {
                _player.PerformingAction = true;
                _player.Attack();
            }
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public bool CanTeleport()
        {
            return true;
        }
    }
}