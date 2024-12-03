using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    public event Action OnChargeStart;
    public event Action OnChargeEnd;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.SpaceBar.started += ctx => OnChargeStart?.Invoke();
        _inputActions.Player.SpaceBar.canceled += ctx => OnChargeEnd?.Invoke();
    }

    private void OnDisable()
    {
        if (_inputActions != null)
        {
            _inputActions.Player.SpaceBar.started -= ctx => OnChargeStart?.Invoke();
            _inputActions.Player.SpaceBar.canceled -= ctx => OnChargeEnd?.Invoke();
            _inputActions.Disable();
        }
    }

}
