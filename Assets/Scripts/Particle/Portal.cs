using Interfaces;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isActive;
    public bool isEntrance;
    public bool hasExit;
    public Portal exit;
    public bool canTeleport;

    private GameObject _objectOnTeleporter;
    private ICanTeleportInterface _canTeleportInterface;
    private float _waitTime = 0;
    private float _waitTimeThreshold = 0.4f;

    private void Awake()
    {
        hasExit = exit != null;
        if (isEntrance && hasExit)
        {
            canTeleport = true;
        }
    }

    private void Update()
    {
        if (!isActive || !isEntrance || !hasExit || !canTeleport)
        {
            return;
        }
        
        if (_canTeleportInterface != null && _canTeleportInterface.CanTeleport() && _canTeleportInterface.CanTeleportThisFrame())
        {
            if (_waitTime > _waitTimeThreshold)
            {
                canTeleport = false;
                TeleportTarget();
            }
            _waitTime += Time.deltaTime;
        }
        else
        {
            _waitTime = 0;
        }
    }

    public void EnableTeleport()
    {
        canTeleport = true;
    }

    public void DisableTeleport()
    {
        canTeleport = false;
    }

    private void TeleportTarget()
    {
        exit.DisableTeleport();
        _canTeleportInterface.Teleport(exit);
        EnableTeleport();
    }

    private void OnTriggerEnter(Collider other)
    {
        var teleportInterface = other.GetComponentInParent<ICanTeleportInterface>();
        if (teleportInterface == null)
        {
            return;
        }

        var parent = other.transform.parent.gameObject;
        _objectOnTeleporter = parent.gameObject;
        _canTeleportInterface = teleportInterface;
    }

    private void OnTriggerExit(Collider other)
    {
        _objectOnTeleporter = null;
        _canTeleportInterface = null;
        EnableTeleport();
    }
}
