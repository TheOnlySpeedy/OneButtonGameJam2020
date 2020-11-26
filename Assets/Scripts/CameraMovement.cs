using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    public float smoothTime = 0.3f;

    private Vector3 _offeset;

    private void Awake()
    {
        _offeset = new Vector3(-3.5f, 4.5f, 3.5f);

        if (!target)
        {
            target = GameObject.Find("Player");
        }
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        
        var rotation = target.transform.position;
        var p = rotation + _offeset;
        transform.position = Vector3.Lerp(
            transform.position,
            p,
            0.1f
        );
        transform.LookAt(target.transform);
    }
}
