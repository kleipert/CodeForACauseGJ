using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyAnimations : MonoBehaviour
    {
        private EnemyType _type;
        private bool _hitAnimation;
        private bool _meleeAtkAnimation;
        private Animator _anim;
        private EnemyBase _base;
        private Transform _hitzone;
        private NavMeshAgent _navAgent;
        private float _baseDamageCooldown = 2f;
        private float _currentDamageCooldown;
        
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _base = GetComponent<EnemyBase>();
            _type = _base.GetEnemyType();
            _hitAnimation = false;
            _meleeAtkAnimation = false;
            _currentDamageCooldown = _baseDamageCooldown;

            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.gameObject.name == "HitZone")
                {
                    _hitzone = child;
                }
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            switch (_type)
            {
                case EnemyType.ZombieMelee:
                    ZombieMeleeAnimations();
                    break;
                case EnemyType.Dog:
                    break;
                case EnemyType.Other:
                    break;
            }

            _currentDamageCooldown -= Time.deltaTime;
        }

        private void ZombieMeleeAnimations()
        {
            if (_hitAnimation)
            {
                _anim.SetBool("GotHit", true);
                StartCoroutine(nameof(WaitAndResetAnimations));
            }

            if (_meleeAtkAnimation)
            {
                if (_currentDamageCooldown <= 0)
                {
                    if(_navAgent.enabled)
                        _navAgent.isStopped = true;
                    _anim.SetBool("CanHit", true);
                    StartCoroutine(nameof(CheckMeleeRange));
                    StartCoroutine(nameof(WaitAndResetAnimations));
                    _currentDamageCooldown = _baseDamageCooldown;
                    if(_navAgent.enabled)
                        _navAgent.isStopped = false;
                }
                
            }
                
        }
        
        public void PlayHitAnimation() => _hitAnimation = true;
        public void PlayMeleeAttackAnimation() => _meleeAtkAnimation = true;
        
        
        private IEnumerator WaitAndResetAnimations()
        {
            yield return new WaitForSeconds(0.5f);
            ResetBools();
            WalkToPlayer.FollowPlayer = true;
            if(_navAgent.enabled)
                _navAgent.isStopped = false;
        }
        private IEnumerator CheckMeleeRange()
        {
            yield return new WaitForSeconds(0.5f);
            if (_hitzone.GetComponent<HitBox>().HasTarget() && _hitAnimation == false)
            {
                Debug.Log("PLAYER GOT HIT");
            }
        }
        private void ResetBools()
        {
            _hitAnimation = false;
            _anim.SetBool("GotHit", false);

            _meleeAtkAnimation = false;
            _anim.SetBool("CanHit", false);
        }
    }
}
