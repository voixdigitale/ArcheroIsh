using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    [Header("Wave Spawning")]
    [SerializeField] private int _currentWave = 0;
    [SerializeField] private AnimationCurve _enemiesEachWave;
    [SerializeField, Range(0, 20)] private int _waveRecoveryBuffer = 5;
    [SerializeField, Range(0, 20)] private float _spawnRadius = 3.0f;
    [SerializeField, Range(1, 20)] private float _spawnInnerRadius = 1.0f;

    [Header("References")]
    [SerializeField] Transform _enemyPrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private TextMeshProUGUI _waveText;

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
            _waveText.SetText(s == seconds ? "Prepare !" : s.ToString());
        }
        _waveText.SetText(string.Empty);
        StartNewWave();
    }

    private IEnumerator ManageEnemySpawning(int numberInSpawn) {
        Debug.Log("About to Spawn " + numberInSpawn);
        yield return new WaitForSeconds(.3f);

        int spawnedEnemies = 0;
        while (_canSpawn && spawnedEnemies < numberInSpawn) {
            Vector2 randomPos = Random.insideUnitCircle * _spawnRadius;
            randomPos.x += _spawnInnerRadius;
            randomPos.y += _spawnInnerRadius;
            Transform enemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetTarget(_player);
            spawnedEnemies++;
            _enemiesExist = true;
        }
        _allEnemiesSpawned = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector2.zero, _spawnRadius);
        Gizmos.DrawWireSphere(Vector2.zero, _spawnInnerRadius);
    }
}
