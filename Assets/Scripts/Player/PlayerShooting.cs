using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _coolDown;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private float _attackRadius = 1.4f;
    [SerializeField] private float _projectileReach = .3f;

    private List<Enemy> _enemies = new List<Enemy>();
    private float _lastAttackTime;

    private void OnEnable() {
        Health.OnDeath += OnDeath_RefreshTargetList;
    }

    private void OnDisable() {
        Health.OnDeath -= OnDeath_RefreshTargetList;
    }
    private void Update() {
        RefreshTargetList();
        if (_enemies.Count > 0 && Time.time > _lastAttackTime && Vector2.Distance(_enemies.First().transform.position, transform.position) < _attackRadius)
            Shoot();
    }
    public float GetProjectileReach() {
        return _projectileReach;
    }

    public void SetProjectileReach(float reach) {
        _projectileReach = reach;
    }

    void Shoot() {
        _lastAttackTime = Time.time + _coolDown;
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetReach(_projectileReach);
        Vector2 relativePos = transform.InverseTransformPoint(_enemies.First().transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(relativePos * GameManager.Instance.CurrentBulletSpeed);
    }

    void RefreshTargetList() {
        _enemies.Clear();
        GameObject[] _enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject _enemyObject in _enemyObjects) {
            _enemies.Add(_enemyObject.GetComponent<Enemy>());
        }

        if (_enemies.Count > 0) {
            _enemies.Sort(Enemy.SortByDistance);
        } else {
            _spawner.NoEnemiesLeft();
        }
    }

    void OnDeath_RefreshTargetList(Health sender) {
        Enemy deadEnemy = sender.gameObject.GetComponent<Enemy>();
        if (_enemies.Contains(sender.gameObject.GetComponent<Enemy>())) {
            _enemies.Remove(deadEnemy);
            Destroy(deadEnemy.gameObject);
        }
    }
}
