using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;

    private Vector3 _offeset;

    private void Awake()
    {
        _offeset = new Vector3(-3.5f, 4.5f, -3.5f);
    }

    private void FixedUpdate()
    {
        var rotation = target.position;
        var p = rotation + _offeset;
        transform.position = Vector3.Lerp(
            transform.position,
            p,
            0.1f
        );
        transform.LookAt(target);
    }
}
