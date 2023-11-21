using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    private PlayerInputActions _playerInputActions;
    private InputAction _move;


    private void Awake() {
        _playerInputActions = new PlayerInputActions();

        _move = _playerInputActions.Player.Move;
    }

    private void OnEnable() {
        _playerInputActions.Enable();
    }

    private void OnDisable() {
        _playerInputActions.Disable();
    }

    private void Update() {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput() {
        return new FrameInput {
            Move = _move.ReadValue<Vector2>(),
        };
    }
}

public struct FrameInput {
    public Vector2 Move;
}