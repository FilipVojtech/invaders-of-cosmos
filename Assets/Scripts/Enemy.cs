using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private float _hardSpeed;

    private int _livesComponent;
    private GameObject _player;

    private AudioSource _destroyAudioSource;
    private AudioSource _hitAudioSource;

    private void Start()
    {
        _destroyAudioSource = GameObject.Find("Easy Enemy Destroy Audio Source").GetComponent<AudioSource>();
        _hitAudioSource = GameObject.Find("Enemy Hit Audio Source").GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        if (!_player.gameObject.GetComponent<Player>().GetEasy())
        {
            speed *= 1.25f;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        OutOfSceneCheck();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) Destroy(other.gameObject);

        if (other.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<Player>().Damage();
            _hitAudioSource.Play();
            _destroyAudioSource.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Missile") || other.gameObject.CompareTag("Explosion"))
        {
            Destroy(other.gameObject);
            _player.GetComponent<Player>().AddKills();
            _hitAudioSource.Play();
            _destroyAudioSource.Play();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("SuperMissile"))
        {
            _hitAudioSource.Play();
            other.gameObject.GetComponent<Supercharge>().SuperBoom();
        }
    }

    private void OutOfSceneCheck()
    {
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(-8.1f, 8.1f), 7, transform.position.z);
        }
    }

/*
    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    */
}