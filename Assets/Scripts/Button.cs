using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Renderer _renderer;
    private Color _green = new Color(0, 1f, 0);
    private void Start()
    {
        _renderer = transform.Find("Button").GetComponent<Renderer>();
    }

    private void ChangeColor()
    {
        _renderer.material.color = _green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.parent.CompareTag("Player"))
        {
            ChangeColor();
        }
    }
}
