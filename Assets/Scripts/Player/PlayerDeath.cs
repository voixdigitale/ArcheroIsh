using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) { 
            Time.timeScale = 0;
            _gameOverPanel.SetActive(true);
        }
    }
}
