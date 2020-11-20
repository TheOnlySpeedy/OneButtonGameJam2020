using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputMaster _controls;
    
    private bool _isTapped;
    private bool _isHolding;

    // Double Tap Action
    private float _tapTime;
    private const float DoubleTapTime = 0.3f;
    
    private float _doubleTapWait = 0;
    private const float DoubleTapWaitThreshold = 0.4f;
    private bool _isDoubleTaping;

    // Hold Action
    private float _heldTime;
    private const float HeldTimeThreshold = 0.4f;

    [SerializeField]
    private bool _performingAction;
    [SerializeField]
    private bool _moveForward = false;
    [SerializeField]
    private bool _rotate = false;

    private float _moveSpeed = 3f;
    private float _rotateSpeed = 180f; // rotation degree per second
    [SerializeField]
    private Vector3 _moveDestination;
    [SerializeField]
    private Quaternion _rotationTarget;

    private void Awake()
    {
        _controls = new InputMaster();

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

        _moveDestination = transform.position;
    }

    private void Update()
    {
        if (_performingAction)
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
            if (Time.time - _tapTime < DoubleTapTime)
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

    private void FixedUpdate()
    {
        if (_moveForward || _rotate)
        {
            _performingAction = true;
        }

        if (!_performingAction)
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
                _performingAction = false;
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
                _performingAction = false;
            }
        }
    }

    private void SingleAction()
    {
        if (_rotate)
        {
            return;
        }
        
        Debug.Log("SingleAction");
        _rotate = true;
        _rotationTarget = Quaternion.Euler(
            0, 
            transform.eulerAngles.y - 90f,
            0
        );
    }

    private void DoubleAction()
    {
        if (_rotate)
        {
            return;
        }

        Debug.Log("DoubleAction");
        _rotate = true;
        _rotationTarget = Quaternion.Euler(
            0, 
            transform.eulerAngles.y + 90f,
            0
        );
    }

    private void HoldAction()
    {
        if (_moveDestination == transform.position)
        {
            Debug.Log("HoldAction");
            _moveForward = true;
                   
            var forward = transform.forward;
                    
            _moveDestination = new Vector3(
                _moveDestination.x + forward.x,
                _moveDestination.y + forward.y,
                _moveDestination.z + forward.z
            );
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
}
