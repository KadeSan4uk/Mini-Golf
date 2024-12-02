using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private float _maxEntrySpeed = 2.0f;
    public delegate void BallInHoleHandler();
    public static event BallInHoleHandler OnBallInHole;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = other.GetComponent<Rigidbody>();
            if (ballRigidbody != null && ballRigidbody.velocity.magnitude <= _maxEntrySpeed)
            {
                Debug.Log("Goal!");
                OnBallInHole?.Invoke();
                Destroy(other.gameObject); 
            }
        }
    }
}
