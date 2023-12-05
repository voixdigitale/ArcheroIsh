using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] float _liveTime = 0.4f;

    private float _dropTimer;

    private void Start() {
        _dropTimer = Time.time + _liveTime / 4;
        StartCoroutine(Consume());
    }

    private IEnumerator Consume() {
        yield return new WaitForSeconds(_liveTime);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy" || Time.time > _dropTimer) {
            _sprite.color = Color.white;

            if (collision.gameObject.tag == "Enemy")
                collision.gameObject.GetComponent<Animator>().SetTrigger("IsHit");

            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth?.ReduceHealth(1);
        }   
    }

    public void SetReach(float reach) {
        _liveTime = reach;
    }
}
