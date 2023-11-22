using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Action<Health> OnDeath;

    [SerializeField] private int _startingHealth = 100;
    [SerializeField] private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void ReduceHealth(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
        }
    }
}
