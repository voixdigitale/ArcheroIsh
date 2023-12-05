using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _rewardsPanel;

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
        string fmt = "0000000";
        _scoreText.SetText(_score.ToString(fmt));
    }

    private void OnHit_Counter(Health sender) {
        _score++;
        UpdateScore();

        if (_score % 450 == 0) {
            Time.timeScale = 0;
            _rewardsPanel.SetActive(true);
        }
    }

    public void BoostWater() {
        PlayerShooting ps = GameManager.Instance.Player.GetComponent<PlayerShooting>();
        ps.SetProjectileReach(ps.GetProjectileReach() * 1.1f);

        RadiusRenderer r = GameManager.Instance.Player.GetComponent<RadiusRenderer>();
        r.xradius *= 1.1f;
        r.yradius *= 1.1f;
        r.CreatePoints();

        _rewardsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ScaleDownPlayer() {
        GameManager.Instance.Player.transform.localScale *= .9f;
        RadiusRenderer r = GameManager.Instance.Player.GetComponent<RadiusRenderer>();
        r.xradius *= 1.1f;
        r.yradius *= 1.1f;
        r.CreatePoints();

        _rewardsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
