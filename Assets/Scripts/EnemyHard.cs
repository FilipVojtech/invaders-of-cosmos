using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHard : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int lives;
    [SerializeField] private int pointsForKill;

    private GameObject _player;
    private AudioSource _destroyAudioSource;
    private AudioSource _hitAudioSource;

    private void Start()
    {
        _destroyAudioSource = GameObject.Find("Hard Enemy Destroy Audio Source").GetComponent<AudioSource>();
        _hitAudioSource = GameObject.Find("Enemy Hit Audio Source").GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        OutOfSceneCheck();
        LivesCheck();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<Player>().Damage();
            lives--;
            _hitAudioSource.Play();
        }

        if (other.gameObject.CompareTag("Missile") || other.gameObject.CompareTag("Explosion"))
        {
            Destroy(other.gameObject);
            lives--;
            _hitAudioSource.Play();
        }

        if (other.gameObject.CompareTag("SuperMissile"))
        {
            lives--;
            other.gameObject.GetComponent<Supercharge>().SuperBoom();
            _hitAudioSource.Play();
        }
    }

    private void OutOfSceneCheck()
    {
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(-8.1f, 8.1f), 7, transform.position.z);
        }
    }

    private void LivesCheck()
    {
        if (lives <= 0)
        {
            _player.GetComponent<Player>().AddKills(pointsForKill);
            _destroyAudioSource.Play();
            Destroy(gameObject);
        }
    }
}