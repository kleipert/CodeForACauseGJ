using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class WalkToPlayer : MonoBehaviour
    {
        private GameObject _player;
        private NavMeshAgent _navAgent;
        public bool FollowPlayer = true;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (_navAgent.enabled)
            {
                if(FollowPlayer)
                    _navAgent.SetDestination(_player.transform.position);
                else
                    _navAgent.SetDestination(transform.position);
            }
        }
    }
}
