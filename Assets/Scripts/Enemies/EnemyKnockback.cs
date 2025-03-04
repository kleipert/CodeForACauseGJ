using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyKnockback : MonoBehaviour
    {

        [SerializeField] private float _upMultiplier;
        [SerializeField] private float _timeToActivateNav = 1.5f;
        private Rigidbody _rb;
        private NavMeshAgent _navAgent;
        private Vector3 _hitDir;

        private bool _wasHit = false;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _navAgent = GetComponent<NavMeshAgent>();
            _upMultiplier = 1f;
        }
        private void FixedUpdate()
        {
            if (!_wasHit) return;

            _navAgent.enabled = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.AddForce(_hitDir.normalized * _upMultiplier, ForceMode.Impulse);
            _wasHit = false;
            StartCoroutine(EnableNavAgent());
        }

        private IEnumerator EnableNavAgent()
        {
            yield return new WaitForSeconds(_timeToActivateNav);
            _navAgent.enabled = true;
        }

        public void GotHit(Vector3 dir, float multiplier)
        {
            _upMultiplier = multiplier;
            _hitDir = dir;
            _wasHit = true;
        }
    }
}
