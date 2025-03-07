using Managers;
using UnityEditor.Experimental.GraphView;
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
        private Vector3 _rangedVector;
        private int layerMask;

    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player");
            _enemyAnimations = GetComponent<EnemyAnimations>();
            //_enemyAnimations.SetIdleAnimation();

        }

        // Update is called once per frame
        void Update()
        {
            if (_navAgent.enabled)
            {
                if (FollowPlayer && Vector3.Distance(_player.transform.position, transform.position) < 40f)
                {
                    _rangedVector = PlayerManager.Instance.GetPlayerPosition() - transform.position;
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, _rangedVector, out hit, Mathf.Infinity) && hit.collider.CompareTag("Player"))
                    {
                        _navAgent.SetDestination(transform.position);
                    }
                    else
                    {
                        _navAgent.SetDestination(PlayerManager.Instance.GetPlayerPosition());
                        _enemyAnimations.ResetBools();
                    }
                    
                }
                else if (FollowPlayer && Vector3.Distance(_player.transform.position, transform.position) > 40f)
                {
                    _navAgent.SetDestination(PlayerManager.Instance.GetPlayerPosition());
                    _enemyAnimations.ResetBools();
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
