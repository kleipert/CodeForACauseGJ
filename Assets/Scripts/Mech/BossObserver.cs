using Enemies;
using UnityEngine;
using Managers;
using UnityEngine.AI;

namespace Mech
{
    public class BossObserver : MonoBehaviour
    {
        private Animator animator;
        private EnemyStats stats;
        private NavMeshAgent navMeshAgent;
        [SerializeField] private GameObject shield;
        public bool parkourPhase { get; set; }
        private bool startPhase2 = false;
        private bool startPhase3 = false;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            stats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            shield.SetActive(false);
            parkourPhase = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (stats.GetHealth() <= 10000 && !startPhase2)
            {
                parkourPhase = true;
                if(navMeshAgent.enabled)
                    navMeshAgent.enabled = false;
                animator.SetBool("IsIdle", true);
                shield.SetActive(true);
                stats.SetIsInvincible(true);
                startPhase2 = true;
            }
            
            if (stats.GetHealth() <= 5000 && !startPhase3)
            {
                parkourPhase = true;
                if(navMeshAgent.enabled)
                    navMeshAgent.enabled = false;
                animator.SetBool("IsIdle", true);
                shield.SetActive(true);
                stats.SetIsInvincible(true);
                startPhase3 = true;
            }
        }

        public void EndPhase2()
        {
            parkourPhase = false;
            navMeshAgent.enabled = true;
            animator.SetBool("IsIdle", false);
            shield.SetActive(false);
            stats.SetIsInvincible(false);
            startPhase2 = false;
        }

        public void EndPhase3()
        {
            parkourPhase = false;
            navMeshAgent.enabled = true;
            animator.SetBool("IsIdle", false);
            shield.SetActive(false);
            stats.SetIsInvincible(false);
            startPhase2 = false;
        }
    }
}


