using System.Collections;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private float _health = 250f;
        [SerializeField] private float waitTime = 6f;
        private EnemyAnimations _enemyAnimations;
        private bool _isInvincible = false;
        [SerializeField] private bool skipAnimation = false;
        [SerializeField] private bool _isBoss = false;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _enemyAnimations = GetComponent<EnemyAnimations>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if (_health <= 0 && !_isBoss)
            {
                if (!skipAnimation)
                    _enemyAnimations.DeathAnimation();
                StartCoroutine(nameof(DestroyObject));
            }
        }
    
        public void ReceiveDamageEnemy(float damage)
        {
            if (!_isInvincible)
                _health -= damage * PlayerManager.Instance.GetPlayerVelocity();
        }

        IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(waitTime);
            Destroy(gameObject);
        }
        
        public float GetHealth() => _health;

        public void SetIsInvincible(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
    }
}
