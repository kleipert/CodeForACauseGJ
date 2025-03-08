using System.Collections;
using Enemies;
using Managers;
using UnityEngine;

namespace Player
{
    public class MissileProjectile : MonoBehaviour
    {

        [SerializeField] LayerMask _player;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _explosionForcePlayer = 100f;
        [SerializeField] private float _explosionForceEnemy = 2f;
        [SerializeField] private Transform _explosionPoint;
        [SerializeField] private float _projectileLifetime = 3f;
        [SerializeField] private float _missleDamage = 10f;
        private float _currentProjectileLifetime;
        private Vector3 _target;
        private bool _isFired;
        private BoxCollider _collider;
        private AudioSource _audioSource;


        public void SetTarget(Vector3 target) => _target = target;
        public void SetFired(bool isFired) => _isFired = isFired;


        private void Awake()
        {
            _isFired = false;
            _collider = GetComponent<BoxCollider>();
            _collider.enabled = false;
            _currentProjectileLifetime = _projectileLifetime;
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isFired)
            {
                _collider.enabled = true;
                transform.position = Vector3.MoveTowards(transform.position,
                    _target, _speed * Time.deltaTime);
                _currentProjectileLifetime -= Time.deltaTime;

                if (_currentProjectileLifetime <= 0)
                {
                    _audioSource.Play();
                    StartCoroutine(nameof(WaitAudio));
                }
                    
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            if (!other.gameObject.CompareTag("Player"))
            {
                Transform _finalTransform = _explosionPoint;
                _audioSource.Play();
                StartCoroutine(nameof(WaitAudio));
                Collider[] colliders = Physics.OverlapSphere(_finalTransform.position, _explosionRadius);
                // Trigger VFX + Sound?
                foreach (Collider coll in colliders)
                {
                    if (coll.CompareTag(TagHandle.GetExistingTag("Player")))
                    {
                        Vector3 dir = coll.gameObject.transform.position - _finalTransform.position;
                        dir.y += 1.5f;
                        PlayerManager.Instance.MovePlayer(dir, _explosionForcePlayer);
                    }
                    
                    if (coll.CompareTag(TagHandle.GetExistingTag("Enemy")))
                    {
                        Vector3 enemyTransformPos = coll.gameObject.transform.position;
                        enemyTransformPos.y += coll.bounds.size.y / 2;
                        Vector3 dir = enemyTransformPos - _finalTransform.position;
                        if (dir.y <= 0)
                            dir.y = 0;
                        coll.GetComponent<EnemyKnockback>().GotHit(dir, _explosionForceEnemy);
                        coll.GetComponent<EnemyStats>().ReceiveDamageEnemy(_missleDamage);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(transform.position, _explosionRadius);
        }

        IEnumerator WaitAudio()
        {
            GetComponent<MeshRenderer>().enabled = false;
            //GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
    
}
