using System;
using UnityEngine;

public class HoleCollider : MonoBehaviour
{
    [SerializeField] private float _maxEntrySpeed = 2.0f;
    [SerializeField] private Transform _initialBallPosition;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private BallController _ballController;

    public event Action<bool> OnBallInHole;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = other.GetComponent<Rigidbody>();
            if (ballRigidbody != null && ballRigidbody.linearVelocity.magnitude <= _maxEntrySpeed)
            {
                ResetBallPosition(other.transform);

                OnBallInHole?.Invoke(true);
            }            
        }
    }

    private void ResetBallPosition(Transform ballTransform)
    {
        if (_ballPrefab != null && _initialBallPosition != null)
        {
            ballTransform.gameObject.SetActive(false);

            GameObject newBall = Instantiate(_ballPrefab, _initialBallPosition.position, _initialBallPosition.rotation);
            Rigidbody newBallRigidbody = newBall.GetComponent<Rigidbody>();

            if (newBallRigidbody != null)
            {
                newBallRigidbody.linearVelocity = Vector3.zero;
                newBallRigidbody.angularVelocity = Vector3.zero;
            }

            newBall.SetActive(true);

            InputHandler newInputHandler = newBall.GetComponent<InputHandler>();
            if (newInputHandler != null)
            {
                newInputHandler.OnChargeStart += _ballController.StartCharging;
                newInputHandler.OnChargeEnd += _ballController.ReleaseCharging;

                Debug.Log("New InputHandler events are subscribed.");
            }
        }
    }
}

