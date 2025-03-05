using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private EnemyType type;
        [SerializeField] private float damage = 50f;
        private EnemyAnimations _animations;
        private PlayerStats _playerStats;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animations = GetComponent<EnemyAnimations>();
            _playerStats = GetComponent<PlayerStats>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && type == EnemyType.ZombieMelee)
            {
                PerformMeleeAttack();
                _playerStats.ReceiveDamagePlayer(damage);
            }
        }*/

        public void PerformMeleeAttack()
        {
            _animations.PlayMeleeAttackAnimation();
            //_playerStats.ReceiveDamagePlayer(damage);
        }

        public EnemyType GetEnemyType() => type;
    }

    public enum EnemyType: int
    {
        ZombieMelee = 1,
        Dog = 2,
        Other = 3
    }
}
