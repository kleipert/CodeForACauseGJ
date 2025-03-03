using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class ShotgunProjectile : MonoBehaviour
    {
        [SerializeField] private float _maxRotation = 3f;
        [SerializeField] private float _bulletSpeed = 100f;
        [SerializeField] private float _maxLifetime = 0.1f;
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

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * (Time.deltaTime * _bulletSpeed);
            //Debug.DrawRay(transform.position, transform.forward, Color.red, 120);
            _remainingLifetime -= Time.deltaTime;
            if(_remainingLifetime <= 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                //Debug.Log($"BULLET PREFAB COLLISION WITH: {other.gameObject.name}");
                
            }
        }
    }
}
