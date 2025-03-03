using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class MissileProjectile : MonoBehaviour
    {

        [SerializeField] LayerMask _player;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _explosionForce = 100f;
        private Vector3 _target;
        private bool _isFired;


        public void SetTarget(Vector3 target) => _target = target;
        public void SetFired(bool isFired) => _isFired = isFired;


        private void Awake()
        {
            _isFired = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isFired)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    _target, _speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                Transform _finalTransform = transform;
                Destroy(gameObject);
                Collider[] colliders = Physics.OverlapSphere(_finalTransform.position, _explosionRadius);
                // Trigger VFX + Sound?
                foreach (Collider coll in colliders)
                {
                    if (coll.CompareTag(TagHandle.GetExistingTag("Player")))
                    {
                        Vector3 dir = coll.gameObject.transform.position - _finalTransform.position;
                        dir.y += 1.5f;
                        PlayerManager.Instance.MovePlayer(dir, _explosionForce);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(transform.position, _explosionRadius);
        }
    }
}
