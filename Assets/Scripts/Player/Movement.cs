using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private float _moveY;
    private Vector3 _rotation;
    private bool _canMove = true;
    private Rigidbody2D _rigidBody;

    private void AllowMove() {
        _canMove = true;
    }
    private void PreventMove() {
        _canMove = false;
    }

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Move();
        //Rotate();
    }
    public void SetCurrentDirection(float currentXDirection, float currentYDirection) {
        _moveX = currentXDirection;
        _moveY = currentYDirection;
    }

    private void Move() {
        if (!_canMove) { return; }

        Vector3 movement = new Vector3(_moveX * _moveSpeed, _moveY * _moveSpeed, 0f);
        _rigidBody.velocity = movement;
    }

}
