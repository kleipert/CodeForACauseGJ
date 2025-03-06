using UnityEngine;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class WalkToPlayerRanged : MonoBehaviour
    {
        private GameObject _player;
        private NavMeshAgent _navAgent;
        public bool FollowPlayer = true;
        private EnemyAnimations _enemyAnimations;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player");
            _enemyAnimations = GetComponent<EnemyAnimations>();
            _enemyAnimations.SetIdleAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            if (_navAgent.enabled)
            {
                if (FollowPlayer && Vector3.Distance(_player.transform.position, transform.position) < 40f)
                {
                    _navAgent.SetDestination(_player.transform.position);
                    _enemyAnimations.ResetBools();
                }
                else if (FollowPlayer && Vector3.Distance(_player.transform.position, transform.position) > 40f)
                {
                    _navAgent.SetDestination(transform.position);
                    //_enemyAnimation.SetReadyforAttack();
                }
                else
                {
                    _navAgent.SetDestination(transform.position);
                    _enemyAnimations.SetIdleAnimation();
                }
            }
        }
    }
}
