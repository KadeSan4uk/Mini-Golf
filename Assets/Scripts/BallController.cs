using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputHandler _inputHandler;

    [SerializeField] private float _minImpactForce = 1.0f;
    [SerializeField] private float _maxImpactForce = 100.0f;
    [SerializeField] private float maxChargeTime = 3.0f;
    private float _stopThreshold = 0.2f;
    private float _slowMotionDuration = 2.0f;
    private float _frictionFactor = 0.99f;

    public event System.Action OnBallStopped;

    private float _chargeTime = 0f;
    private bool _isCharging = false;
    private float _slowMotionTimer = 0f;

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnChargeStart += StartCharging;
            _inputHandler.OnChargeEnd += ReleaseCharging;
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnChargeStart -= StartCharging;
            _inputHandler.OnChargeEnd -= ReleaseCharging;
        }
    }

    public void StartCharging()
    {
        _isCharging = true;
        _chargeTime = 0f;
    }

    public void ReleaseCharging()
    {
        _isCharging = false;
        ApplyImpactForce();
    }

    private void Update()
    {
        if (_isCharging)
        {
            _chargeTime += Time.deltaTime / maxChargeTime;
            _chargeTime = Mathf.Clamp01(_chargeTime);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = _rigidbody.linearVelocity.magnitude;

        if (currentSpeed > _stopThreshold)
        {
            Vector3 newVelocity = _rigidbody.linearVelocity * _frictionFactor;
            _rigidbody.linearVelocity = newVelocity;

            _rigidbody.angularVelocity *= _frictionFactor;
        }
        else if (currentSpeed < _stopThreshold && _rigidbody.linearVelocity.magnitude > 0)
        {
            _slowMotionTimer += Time.fixedDeltaTime;

            if (_slowMotionTimer >= _slowMotionDuration)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;

                OnBallStopped?.Invoke();                
                _slowMotionTimer = 0f;
            }
        }
        else
        {
            _slowMotionTimer = 0f; 
        }
    }


    private void ApplyImpactForce()
    {
        _slowMotionTimer = 0f;

        float impactForce = Mathf.Lerp(_minImpactForce, _maxImpactForce, _chargeTime);
        impactForce = Mathf.Clamp(impactForce, _minImpactForce, _maxImpactForce);

        _rigidbody.AddForce(Vector3.forward * impactForce, ForceMode.Impulse);
    }
}
