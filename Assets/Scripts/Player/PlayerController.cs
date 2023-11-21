using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Movement _movement;

    public void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

     private void Update() {
        GatherInput();
        Movement();
    }

    private void GatherInput() {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {
        _movement.SetCurrentDirection(_frameInput.Move.x, _frameInput.Move.y);
    }
}
