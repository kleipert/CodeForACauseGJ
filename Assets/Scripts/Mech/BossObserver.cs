using System.Collections;
using Enemies;
using UnityEngine;
using Managers;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Mech
{
    public class BossObserver : MonoBehaviour
    {
        private Animator animator;
        private EnemyStats stats;
        private NavMeshAgent navMeshAgent;
        [SerializeField] private GameObject shield;
        [SerializeField] private float deathTime;
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
                PhasenManager.Instance.StartPhase2();
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
                PhasenManager.Instance.StartPhase3();
            }

            if (stats.GetHealth() <= 0)
            {
                animator.SetTrigger("IsDead");
                StartCoroutine(nameof(DestroyBoss));
            }
        }

        public void EndPhase2()
        {
            parkourPhase = false;
            navMeshAgent.enabled = true;
            animator.SetBool("IsIdle", false);
            shield.SetActive(false);
            stats.SetIsInvincible(false);
        }

        public void EndPhase3()
        {
            parkourPhase = false;
            navMeshAgent.enabled = true;
            animator.SetBool("IsIdle", false);
            shield.SetActive(false);
            stats.SetIsInvincible(false);
        }

        IEnumerator DestroyBoss()
        {
            yield return new WaitForSeconds(deathTime);
            Destroy(gameObject);
            SceneManager.LoadScene("EndScreen");
        }
    }
}


