using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class ShotgunProjectile : MonoBehaviour
    {
        [SerializeField] private float _maxRotation = 3f;
        [SerializeField] private float _bulletSpeed = 5f;
        [SerializeField] private float _maxLifetime = 5f;
        private float _remainingLifetime;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            var angle = Random.Range(-_maxRotation, _maxRotation);
            transform.Rotate(Vector3.up, angle);
            angle = Random.Range(-_maxRotation, _maxRotation);
            transform.Rotate(Vector3.right, angle);
            
            _remainingLifetime = _maxLifetime;
        }
        
        void Update()
        {
            transform.position += transform.forward * (Time.deltaTime * _bulletSpeed);
            Debug.DrawRay(transform.position, transform.forward, Color.red, 120);
            _remainingLifetime -= Time.deltaTime;
            if(_remainingLifetime <= 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                if (other.CompareTag(TagHandle.GetExistingTag("Enemy")))
                {
                    Debug.Log("HIT ENEMY");
                    Vector3 enemyTransformPos = other.transform.position;
                    enemyTransformPos.y += other.bounds.size.y / 2;
                    Vector3 dir = enemyTransformPos - transform.position;
                    dir.y = 0;
                    other.GetComponent<EnemyKnockback>().GotHit(dir, 1);
                }
                
            }
        }
    }
}
