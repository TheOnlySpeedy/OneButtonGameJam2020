using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotate : MonoBehaviour
{
    private Vector3 _speed;

    private void Awake()
    {
        _speed = new Vector3(0, 0, 90f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_speed * Time.deltaTime);
    }
}
