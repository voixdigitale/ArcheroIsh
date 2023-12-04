using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable() {
        Health.OnHit += OnHit_Counter;
    }

    private void OnDisable() {
        Health.OnHit -= OnHit_Counter;
    }


    private void Start() {
        _score = 0;
        UpdateScore();
    }

    private void UpdateScore() {
        string fmt = "00000";
        _scoreText.SetText("Score: " + _score.ToString(fmt));
    }

    private void OnHit_Counter(Health sender) {
        _score++;
        UpdateScore();
    }
}
