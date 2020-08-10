using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScene : MonoBehaviour
{
    private bool _isPlaying;
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        _isPlaying = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level");
        }

        PauseMenuHandler();
    }

    private void PauseMenuHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) _isPlaying = !_isPlaying;

        if (!_isPlaying)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0.1f;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SetIsPlaying(bool toWhat)
    {
        _isPlaying = toWhat;
    }

    public bool GetIsPlaying()
    {
        return _isPlaying;
    }
}