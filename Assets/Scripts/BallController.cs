using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputHandler _inputHandler;

    [SerializeField] private float _minImpactForce = 1.0f;
    [SerializeField] private float _maxImpactForce = 10.0f;
    [SerializeField] private float maxChargeTime = 3.0f;

    private float _chargeTime = 0f;
    private bool _isCharging = false;

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

    private void ApplyImpactForce()
    {
        float impactForce = Mathf.Lerp(_minImpactForce, _maxImpactForce, _chargeTime);
        impactForce = Mathf.Clamp(impactForce, _minImpactForce, _maxImpactForce);
        _rigidbody.AddForce(Vector3.forward * impactForce, ForceMode.Impulse);
    }
}
