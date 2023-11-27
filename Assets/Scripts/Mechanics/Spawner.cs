using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Core Spawning")]
    [SerializeField] List<BoxCollider2D> _spawnZones;

    [Header("Wave Spawning")]
    [SerializeField] private int _currentWave = 0;
    [SerializeField] private AnimationCurve _enemiesEachWave;
    [SerializeField, Range(0, 20)] private int _waveRecoveryBuffer = 5;

    [Header("References")]
    [SerializeField] Transform _enemyPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveDisplay;

    private bool _allEnemiesSpawned;
    private bool _enemiesExist;
    private bool _canSpawn = true;


    private void Start() {
        StartCoroutine(BetweenWaveBuffer());
    }

    void Update()
    {
        if (_allEnemiesSpawned && !_enemiesExist && _canSpawn) {
            StopAllCoroutines();
            StartCoroutine(BetweenWaveBuffer());
        }
    }

    public void NoEnemiesLeft() {
        _enemiesExist = false;
    }

    private void StartNewWave() {
        int enemyQuantity = Mathf.RoundToInt(_enemiesEachWave.Evaluate(++GameManager.Instance._currentWave - 1));

        StartCoroutine(ManageEnemySpawning(enemyQuantity));
    }


    private IEnumerator BetweenWaveBuffer() {
        _allEnemiesSpawned = false;
        int seconds = _waveRecoveryBuffer / 1;
        for (int s = seconds; s > 0; s--) {
            yield return new WaitForSeconds(1);
            _waveText.SetText(s == seconds ? $"Prepare for wave {GameManager.Instance._currentWave + 1}" : s.ToString());
        }
        _waveText.SetText(string.Empty);
        _waveDisplay.SetText($"Wave {GameManager.Instance._currentWave + 1}");
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
