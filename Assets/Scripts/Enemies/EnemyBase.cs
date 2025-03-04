using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private EnemyType type;
        private EnemyAnimations _animations;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animations = GetComponent<EnemyAnimations>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && type == EnemyType.ZombieMelee)
            {
                PerformMeleeAttack();
                // Damage Player
            }
        }

        private void PerformMeleeAttack()
        {
            _animations.PlayMeleeAttackAnimation();
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
