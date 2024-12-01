using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallController _ballController;
    [SerializeField] private InputHandler _inputHandler;

    private void OnEnable()
    {
        if (_ballController != null && _inputHandler != null)
        {
            _inputHandler.OnChargeStart += _ballController.StartCharging;
            _inputHandler.OnChargeEnd += _ballController.ReleaseCharging;
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null && _ballController != null)
        {
            _inputHandler.OnChargeStart -= _ballController.StartCharging;
            _inputHandler.OnChargeEnd -= _ballController.ReleaseCharging;
        }
    }
}
