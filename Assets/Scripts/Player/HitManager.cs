using System;
using Enemies;
using Managers;
using UnityEngine;

namespace Player
{
    public class HitManager : MonoBehaviour
    {
        private SphereCollider _sphereCollider;
        private EnemyType _type;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
        
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
