using System;
using Interfaces;
using UnityEngine;

public class Player : MonoBehaviour, ICanTeleportInterface
{
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;

    [SerializeField] 
    private bool _performingAction;
    public bool PerformingAction
    {
        get => _performingAction;
        set => _performingAction = value;
    }

    public Vector3 RayCastCenter { get; set; }
    
    [SerializeField] 
    private bool _canMoveForward;
    public bool CanMoveForward
    {
        get => _canMoveForward;
        set => _canMoveForward = value;
    }

    [SerializeField] 
    private bool _canAttack;
    public bool CanAttack
    {
        get => _canAttack;
        set => _canAttack = value;
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        RayCastCenter = (Vector3.up / 2);
    }

    private void Update()
    {
        CheckInFront();
        Debug.DrawRay((transform.position + RayCastCenter), transform.forward, Color.yellow);
        _playerInput.CheckInput();
    }

    private void FixedUpdate()
    {
        _playerMovement.DoMovement();
    }

    public void CheckInFront()
    {
        if (Physics.Raycast(
            (transform.position + RayCastCenter),
            transform.forward, 
                out var hit,
                1f
        )) {
            // Debug.Log(hit.transform.gameObject.name);
            var checkTag = hit.transform.gameObject.tag;
            if (checkTag.Equals("Wall"))
            {
                CanMoveForward = false;
                CanAttack = false;
            }

            if (checkTag.Equals("Enemy"))
            {
                CanMoveForward = false;
                CanAttack = true;
            }
        }
        else
        {
            CanMoveForward = true;
            CanAttack = false;
        }
    }
    
    public bool CanTeleport()
    {
        return true;
    }

    public bool CanTeleportThisFrame()
    {
        return !(PerformingAction);
    }

    public void Teleport(Portal portal)
    {
        transform.position = portal.transform.position;
        portal.DisableTeleport();
        _playerMovement.ResetMoveDestination();
    }

    public void Rotate(float deg)
    {
        _playerMovement.SetRotate(deg);
    }

    public void MoveForward()
    {
        _playerMovement.MoveForward();
    }

    public void ResetMoveDestination()
    {
        _playerMovement.ResetMoveDestination();
    }
}
