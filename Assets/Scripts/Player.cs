using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    //Player Statistics
    [SerializeField] private float speed;
    [SerializeField] private int pointsToSuper;
    [SerializeField] private int lives = 3;
    private int _kills;
    private bool _easy;
    private bool _isPlaying;

    //Shooting
    [SerializeField] private float fireRate;
    private float _nextFire;
    private int _currentMag;
    private int _ammoToLoad;
    [SerializeField] private int magSize;
    [SerializeField] private int ammoStash;
    [SerializeField] private int maxAmmoStash;
    [SerializeField] private float waitForReload;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform rightSpawner;
    [SerializeField] private Transform leftSpawner;
    [SerializeField] private GameObject superPrefab;
    [SerializeField] private Transform superSpawner;

    private int _superChargeCharge;
    private int _superChargeValue;
    [SerializeField] private Slider superSlider;

    //Enemy Config
    [SerializeField] private GameObject enemyPrefabEasy;
    [SerializeField] private GameObject enemyPrefabHard;
    [SerializeField] private int killsToMakeHarder;
    [SerializeField] private float timeToSpawnEnemyEasy;
    [SerializeField] private float timeToSpawnEnemyHard;
    private float _originalTtsEasy;
    private float _originalTtsHard;
    private Vector3 _randomPosition;
    private bool _rightTurret = false;

    //GUI handling
    [SerializeField] private Slider hitPoints;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private TextMeshProUGUI superCount;
    [SerializeField] private GameObject gameOverScreen;
    
    //Sounds
    [SerializeField] private AudioSource audioSomething;

    /// Start
    private void Start()
    {
        Time.timeScale = 1;
        hitPoints.maxValue = lives + 1;
        _currentMag = magSize;
        _easy = true;
        _originalTtsEasy = timeToSpawnEnemyEasy;
        _originalTtsHard = timeToSpawnEnemyHard;
        superSlider.maxValue = pointsToSuper;
        gameObject.transform.position = new Vector2(0f, -4f);
    }

    private void Update()
    {
        _isPlaying = GameObject.Find("Main Camera").GetComponent<RestartScene>().GetIsPlaying();

        if (_isPlaying)
        {
            PlayerMovement();
            Shoot();
        }

        CheckSuperCharge();
        SpawnEnemy();
        CheckLives();
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(0, horizontalInput, 0);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.1f, 8.1f), transform.position.y, 0);
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _currentMag > 0)
        {
            _nextFire = Time.time + fireRate;
            if (_rightTurret)
            {
                _currentMag--;
                Instantiate(missilePrefab, leftSpawner.position, Quaternion.identity);
                _rightTurret = false;
            }
            else
            {
                _currentMag--;
                Instantiate(missilePrefab, rightSpawner.position, Quaternion.identity);
                _rightTurret = true;
            }
            audioSomething.Play();
        }

        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
        {
            _ammoToLoad = magSize - _currentMag;

            if (ammoStash > _ammoToLoad)
            {
                ammoStash -= _ammoToLoad;
                _currentMag += _ammoToLoad;
            }
            else
            {
                _currentMag += ammoStash;
                ammoStash = 0;
            }
        }

        ammoStash = ammoStash > maxAmmoStash ? maxAmmoStash : ammoStash;
        ammoCount.text = _currentMag + "/" + ammoStash;
    }

    private void SpawnEnemy()
    {
        timeToSpawnEnemyEasy -= 1 * Time.deltaTime;
        if (timeToSpawnEnemyEasy <= 0)
        {
            _randomPosition = new Vector3(Random.Range(-8.1f, 8.1f), 7, 0);
            Instantiate(enemyPrefabEasy, _randomPosition, Quaternion.Euler(-180, 0, 0));
            timeToSpawnEnemyEasy = _originalTtsEasy;
        }

        timeToSpawnEnemyHard -= 1 * Time.deltaTime;
        if (!_easy && timeToSpawnEnemyHard <= 0)
        {
            _randomPosition = new Vector3(Random.Range(-8.1f, 8.1f), 7, 0);
            Instantiate(enemyPrefabHard, _randomPosition, Quaternion.Euler(-180, 0, 0));
            timeToSpawnEnemyHard = _originalTtsHard;
        }

        if (_kills >= killsToMakeHarder && _easy)
        {
            _originalTtsEasy *= 0.5f;
            _easy = false;
        }
    }

    private void CheckSuperCharge()
    {
        if (_superChargeCharge >= pointsToSuper)
        {
            _superChargeValue++;
            _superChargeCharge -= pointsToSuper;
        }

        superSlider.value = _superChargeCharge;
        superCount.text = _superChargeValue.ToString();
        if (Input.GetKeyDown(KeyCode.LeftAlt) && _superChargeValue >= 1)
        {
            _superChargeValue--;
            Instantiate(superPrefab, superSpawner.position, Quaternion.identity);

            if (lives < 4) lives++;
        }
    }

    private void CheckLives()
    {
        hitPoints.value = lives;

        if (lives <= 0)
        {
            gameObject.SetActive(false);
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    ///Public methods///
    public void Damage()
    {
        lives--;
    }

    public void AddKills()
    {
        _kills++;
        _superChargeCharge++;
        ammoStash += 2;

        killCount.text = _kills.ToString();
    }

    public void AddKills(int superCharge)
    {
        _kills++;
        _superChargeCharge += superCharge;

        switch (superCharge)
        {
            case 2:
            {
                ammoStash += 4;
                break;
            }
        }

        killCount.text = _kills.ToString();
    }

    public bool GetEasy()
    {
        return _easy;
    }
}