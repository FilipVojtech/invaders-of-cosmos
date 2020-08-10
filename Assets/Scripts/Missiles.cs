using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        if (transform.position.y > 7.5f) Destroy(gameObject);
    }
}