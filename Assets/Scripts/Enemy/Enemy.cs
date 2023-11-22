using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _moveSpeed;

    private Rigidbody2D _rigidBody;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        //Rotate();
        Move();
    }
    private void Move() {
        Vector3 dir = _target.position - transform.position;
        Vector3 movement = new Vector3(dir.normalized.x * _moveSpeed, dir.normalized.y * _moveSpeed, 0f);
        _rigidBody.velocity = movement;
    }

    private void Rotate() {
        Vector3 relative = transform.InverseTransformPoint(_target.position);
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }

    public static int SortByDistance(Enemy e1, Enemy e2) {
        Transform p = GameObject.FindWithTag("Player").transform;
        if (Mathf.Abs((p.position - e1.transform.position).magnitude) > Mathf.Abs((p.position - e2.transform.position).magnitude))
            return 1;

        if (Mathf.Abs((p.position - e1.transform.position).magnitude) < Mathf.Abs((p.position - e2.transform.position).magnitude))
            return -1;

        return 0;
    }

}
