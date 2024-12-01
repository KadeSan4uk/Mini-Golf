using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    public delegate void ChargeStartEventHandler();
    public delegate void ChargeEndEventHandler();

    public event ChargeStartEventHandler OnChargeStart;
    public event ChargeEndEventHandler OnChargeEnd;

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
        _inputActions.Disable();
    }
}
