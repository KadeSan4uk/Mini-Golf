using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallController _ballController;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private TextMeshProUGUI _ScoreUI;
    [SerializeField] private HoleCollider _holeCollider;

    private int _score = -15;

    private void OnEnable()
    {
        if (_inputHandler != null && _ballController != null)
        {
            _inputHandler.OnChargeStart += _ballController.StartCharging;
            _inputHandler.OnChargeEnd += _ballController.ReleaseCharging;
        }

        if (_holeCollider != null)
        {
            _holeCollider.OnBallInHole += RegisterHit;
        }

        if (_ballController != null)
        {
            _ballController.OnBallStopped += HandleBallStopped; 
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null && _ballController != null)
        {
            _inputHandler.OnChargeStart -= _ballController.StartCharging;
            _inputHandler.OnChargeEnd -= _ballController.ReleaseCharging;
        }

        if (_holeCollider != null)
        {
            _holeCollider.OnBallInHole -= RegisterHit;
        }

        if (_ballController != null)
        {
            _ballController.OnBallStopped -= HandleBallStopped; 
        }
    }

    private void Update()
    {
        CheckOnScore();
    }

    private void CheckOnScore()
    {
        _ScoreUI.text = "Score: " + _score.ToString();
    }

    public void RegisterHit(bool success)
    {
        if (!success)
        {
            _score += 5;
            CheckOnScore();
            if (_score > 0)
            {
                Debug.Log("You Lost!");
            }
        }
        else
        {
            _score -= 5;
            CheckOnScore();
            Debug.Log("Goal!");
        }
    }

    private void HandleBallStopped()
    {
        Debug.Log("Ball stopped but did not reach the hole.");
        _score += 5;
        CheckOnScore();
    }
}
