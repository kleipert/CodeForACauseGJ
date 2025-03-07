using Managers;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class WalkToPlayerMech : MonoBehaviour
    {
        private GameObject _player;
        private NavMeshAgent _navAgent;
        public bool FollowPlayer = false;
        private Animator _animator;
        private Vector3 _startPosition;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player");
            _animator = GetComponent<Animator>();
            _navAgent.SetDestination(transform.position);
            _startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (_navAgent.enabled && PhasenManager.Instance.betweenPhase)
            {
                _navAgent.SetDestination(_player.transform.position);
                _animator.SetBool("IsIdle", true);
            }
            else if (_navAgent.enabled && !PhasenManager.Instance.betweenPhase)
            {
                _navAgent.SetDestination(transform.position);
                _animator.SetBool("IsIdle", false);
            }
                
        }
    }
}