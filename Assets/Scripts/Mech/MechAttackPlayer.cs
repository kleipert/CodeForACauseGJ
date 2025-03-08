using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Mech
{
    public class AttackPlayer : MonoBehaviour
    {   
        private BoxCollider boxCollider;
        private Animator animator;
        private Rigidbody rigidbody;
        private NavMeshAgent navMeshAgent;
        [SerializeField] private float attackDuration;
        private bool attacking = false;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player" && !attacking)
            {
                if (navMeshAgent.enabled)
                    navMeshAgent.enabled = false;
                transform.LookAt(PlayerManager.Instance.GetPlayerPosition());
                animator.SetBool("CanHit", true);
                StartCoroutine(nameof(AttackDelay));
            }
        }

        IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(attackDuration);
            animator.SetBool("CanHit", false);
            navMeshAgent.enabled = true;
        }
    }
}


