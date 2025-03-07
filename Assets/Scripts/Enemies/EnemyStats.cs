using System.Collections;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private float _health = 250f;
        private EnemyAnimations _enemyAnimations;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _enemyAnimations = GetComponent<EnemyAnimations>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if (_health <= 0)
            {
                _enemyAnimations.DeathAnimation();
                StartCoroutine(nameof(DestroyObject));
            }
        }
    
        public void ReceiveDamageEnemy(float damage)
        {
            _health -= damage * PlayerManager.Instance.GetPlayerVelocity();
        }

        IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(6f);
            Destroy(gameObject);
        }
    }
}
