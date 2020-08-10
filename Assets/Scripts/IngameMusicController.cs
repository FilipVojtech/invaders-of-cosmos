using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMusicController : MonoBehaviour
{
    private bool _isPlayerEasy;
    [SerializeField] private GameObject objekt;
    [SerializeField] private AudioClip audioEasy;
    [SerializeField] private AudioClip audioEasyCore;
    [SerializeField] private AudioClip audioHard;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = objekt.GetComponent<AudioSource>();
    }

    private void Update()
    {
        _isPlayerEasy = GameObject.Find("Player").gameObject.GetComponent<Player>().GetEasy();
        if (_audioSource.time >= audioEasy.length)
        {
            _audioSource.clip = audioEasyCore;
            _audioSource.Play();
            _audioSource.loop = true;
        }

        if ((_audioSource.clip.name == audioEasyCore.name || _audioSource.clip.name == audioEasy.name) && !_isPlayerEasy)
        {
            _audioSource.clip = audioHard;
            _audioSource.Play();
            _audioSource.loop = true;
        }
    }
}