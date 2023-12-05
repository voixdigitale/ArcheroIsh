using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Core Spawning")]
    [SerializeField] List<BoxCollider2D> _spawnZones;
    [SerializeField] Transform _furniture;

    [SerializeField] private bool _timedWave;
    [SerializeField] private int _maxTimeInSeconds;
    [SerializeField] private int _maxSpawnRate;
    [SerializeField] private int _maxSpawnNumber;
    [SerializeField] private AnimationCurve _enemyCurve;
    [SerializeField] private AnimationCurve _enemySpawnRate;

    [Header("Wave Spawning")]
    [SerializeField] private int _currentWave = 0;
    [SerializeField] private AnimationCurve _enemiesEachWave;
    [SerializeField, Range(0, 20)] private int _waveRecoveryBuffer = 5;

    [Header("References")]
    [SerializeField] Transform _enemyPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveDisplay;
    [SerializeField] private GameObject _countdownPrefab;

    private bool _allEnemiesSpawned;
    private bool _enemiesExist;
    private bool _canSpawn = true;
    private float _nextSpawnTimer;


    private void Start() {
        if (_timedWave) {
            _nextSpawnTimer = GetNextEnemySpawnTime();
        }
        
        StartCoroutine(BetweenWaveBuffer());
    }

    void Update()
    {
        if (_timedWave) {
            _nextSpawnTimer -= Time.deltaTime;
            if (_nextSpawnTimer <= 0 ) {
                _nextSpawnTimer = GetNextEnemySpawnTime();
                StartCoroutine(ManageEnemySpawning(GetEnemiesToSpawn()));
            }

        } else if (_allEnemiesSpawned && !_enemiesExist && _canSpawn) {
            StopAllCoroutines();
            StartCoroutine(BetweenWaveBuffer());
        }
    }

    public void NoEnemiesLeft() {
        _enemiesExist = false;
    }

    private void StartNewWave() {
        RandomizeFurniture();

        StartCoroutine(ManageEnemySpawning(GetEnemiesToSpawn()));
    }

    private void RandomizeFurniture() {
        foreach (Transform child in _furniture) {
            child.gameObject.SetActive(Random.value > 0.5f);
        }
    }

    private float GetNextEnemySpawnTime() {
        return _maxSpawnRate * _enemySpawnRate.Evaluate(Time.time / _maxTimeInSeconds);
    }

    private int GetEnemiesToSpawn() {
        int enemyQuantity;

        if (!_timedWave) {
            enemyQuantity = Mathf.RoundToInt(_enemiesEachWave.Evaluate(++GameManager.Instance.CurrentWave - 1));
        } else {
            enemyQuantity = 1 + (int) (_maxSpawnNumber * _enemyCurve.Evaluate(Time.time / _maxTimeInSeconds));
        }

        return enemyQuantity;
    }

    private IEnumerator BetweenWaveBuffer() {
        _allEnemiesSpawned = false;
        int seconds = _waveRecoveryBuffer / 1;
        for (int s = seconds; s >= 0; s--) {
            yield return new WaitForSeconds(1);
            _countdownPrefab.SetActive(true);
            if (_timedWave) {
                _countdownPrefab.GetComponent<Image>().enabled = (s == seconds ? false : true);
                _waveText.SetText(s == seconds ? $"Ready?" : s.ToString());
            } else {
                _waveText.SetText(s == seconds ? $"Prepare for wave {GameManager.Instance.CurrentWave + 1}" : s.ToString());
            }
        }
        _countdownPrefab.SetActive(false);
        _waveText.SetText(string.Empty);
        if (!_timedWave) {
            _waveDisplay.SetText($"Wave {GameManager.Instance.CurrentWave + 1}");
        }
        StartNewWave();
    }

    private IEnumerator ManageEnemySpawning(int numberInSpawn) {
        yield return new WaitForSeconds(.3f);

        int spawnedEnemies = 0;
        while (_canSpawn && spawnedEnemies < numberInSpawn) {
            Vector2 randomPos = GenerateRandomSpawnPoint();
            Transform enemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetTarget(_player);
            spawnedEnemies++;
            _enemiesExist = true;
        }
        _allEnemiesSpawned = true;
    }

    private Vector2 GenerateRandomSpawnPoint() {
        int zone = Random.Range(0, _spawnZones.Count);
        float posX = Random.Range(_spawnZones[zone].bounds.min.x, _spawnZones[zone].bounds.max.x);
        float posY = Random.Range(_spawnZones[zone].bounds.min.y, _spawnZones[zone].bounds.max.y);

        return new Vector2(posX, posY);
    }
}
