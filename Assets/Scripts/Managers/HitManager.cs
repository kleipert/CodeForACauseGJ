using Enemies;
using UnityEngine;

namespace Managers
{
    public class HitManager : MonoBehaviour
    {
        private SphereCollider _sphereCollider;
        private EnemyType _type;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyBase enemyBaseScript = other.GetComponent<EnemyBase>();
                _type = enemyBaseScript.GetEnemyType();
                switch (_type)
                {
                    case EnemyType.ZombieMelee:
                        enemyBaseScript.PerformMeleeAttack();
                        break;
                    case EnemyType.Dog:
                        break;
                    case EnemyType.Other:
                        break;
                }
                
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            if(other.gameObject.GetComponent<EnemyBase>().GetCanAttack())
                PlayerManager.Instance.ReceiveDamagePlayer(50f);
            //Debug.Log("Player got hit!");
        }
                
    }

}
