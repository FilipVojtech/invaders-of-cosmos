using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Supercharge : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;


    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));

        if (transform.position.y >= 0)
        {
            SuperBoom();
        }
    }

    public void SuperBoom()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}