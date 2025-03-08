using UnityEngine;

namespace Enemies
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private EnemyType type;
        [SerializeField] private float damage = 50f;
        [SerializeField] private float setAttackCooldown = 5f;
        [SerializeField] private bool canAttack;
        private EnemyAnimations _animations;
        private float _attackCooldown;
        public bool IsAttacking {get; set;} 
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animations = GetComponent<EnemyAnimations>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_attackCooldown > 0)
            {
                _attackCooldown -= Time.deltaTime;
                canAttack = false;
            }
            else
            {
                canAttack = true;
            }    
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && type == EnemyType.ZombieMelee)
            {
                PerformMeleeAttack();
            }
        }

        public void PerformMeleeAttack()
        {
            _animations.PlayMeleeAttackAnimation();
            IsAttacking = true;
        }

        public EnemyType GetEnemyType() => type;

        public bool GetCanAttack()
        {
            if (canAttack && IsAttacking)
            {
                _attackCooldown = setAttackCooldown;
                return canAttack;
            }
            return false;
        }
    }

    public enum EnemyType: int
    {
        ZombieMelee = 1,
        Dog = 2,
        Ranged = 3,
        Other = 4
    }
}
