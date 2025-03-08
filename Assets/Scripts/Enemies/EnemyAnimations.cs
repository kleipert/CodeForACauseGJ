using System.Collections;
using System.Numerics;
using Managers;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Animator = UnityEngine.Animator;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


namespace Enemies
{
    public class EnemyAnimations : MonoBehaviour
    {
        private static readonly int CanHit = Animator.StringToHash("CanHit");
        private static readonly int GotHit = Animator.StringToHash("GotHit");
        private EnemyType _type; 
        private bool _hitAnimation;
        private bool _rangedAttackAnimation;
        [SerializeField] private bool _meleeAtkAnimation;
        private Animator _anim;
        private EnemyBase _base;
        private Transform _hitzone;
        private NavMeshAgent _navAgent;
        private WalkToPlayer _walkToPlayer;
        private WalkToPlayerRanged _walkToPlayerRanged;
        [SerializeField] private float _baseDamageCooldown = 1.5f;
        [SerializeField] private float _currentDamageCooldown;
        private Vector3 _playerPosition;
        private Rigidbody _rb;
        private Vector3 _meleeVector;
        private Vector3 _rangedVector;
        private bool _isattacking = false;
        [SerializeField] private LayerMask layerMask;
        private Vector3 newDirection;
        [SerializeField] private float waitattack = 0.5f;
        [SerializeField] private float attackpower = 5f;
        [SerializeField] private float jumpheigth = 5f;
        [SerializeField] private float durationanimation = 0.8f;
        [SerializeField] private GameObject _projectilePrefab;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
            _base = GetComponent<EnemyBase>();
            _type = _base.GetEnemyType();
            _rb = GetComponent<Rigidbody>();
            _walkToPlayer = GetComponent<WalkToPlayer>();
            _walkToPlayerRanged = GetComponent<WalkToPlayerRanged>();
            _hitAnimation = false;
            _meleeAtkAnimation = false;
            _rangedAttackAnimation = false;
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
                case EnemyType.Ranged:
                    RangedAttack();
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
                StartCoroutine(nameof(WaitAndResetAnimationsHit));
            }

            if (_meleeAtkAnimation)
            {
                if (_currentDamageCooldown <= 0 && !_isattacking)
                {
                    //_walkToPlayer.FollowPlayer = false;
                    _isattacking = true;
                    if (_navAgent.enabled == true)
                        _navAgent.enabled = false;
                    _anim.SetBool("CanHit", true);
                    StartCoroutine(nameof(CheckMeleeRange)); 
                    StartCoroutine(nameof(StopAttack));
                    StartCoroutine(nameof(WaitAndResetAnimations));
                    StartCoroutine(nameof(StopHitAnimation));
                    _currentDamageCooldown = _baseDamageCooldown;
                }
                
            }
                
        }
        
        public void PlayHitAnimation() => _hitAnimation = true;
        public void PlayMeleeAttackAnimation() => _meleeAtkAnimation = true;
        public void PlayRangedAttackAnimation() => _rangedAttackAnimation = true;


        private void SetWalkToPlayer(bool value)
        {
            switch (_type)
            {
                case EnemyType.ZombieMelee:
                    _walkToPlayer.FollowPlayer = value;
                    break;
                case EnemyType.Ranged:
                    _walkToPlayerRanged.FollowPlayer = value;
                    break;
                
            }
        }
        
        private IEnumerator WaitAndResetAnimations()
        {
            yield return new WaitForSeconds(5f);
            ResetBools();
            SetWalkToPlayer(true);
            
            /*if(_navAgent.enabled)
                _navAgent.isStopped = false;*/
            if(_navAgent.enabled == false)
                _navAgent.enabled = true;
            _playerPosition = PlayerManager.Instance.GetPlayerPosition();
            Vector3.RotateTowards(transform.forward, _meleeVector, 360f, 0.0f);
            _currentDamageCooldown = _baseDamageCooldown;
            _base.IsAttacking = false;
            _isattacking = false;
        }
        
        private IEnumerator WaitAndResetAnimationsHit()
        {
            yield return new WaitForSeconds(1f);
            SetWalkToPlayer(true);
            
            /*if(_navAgent.enabled)
                _navAgent.isStopped = false;*/
            if(_navAgent.enabled == false)
                _navAgent.enabled = true;
            _rb.constraints = RigidbodyConstraints.FreezePosition;
            _playerPosition = PlayerManager.Instance.GetPlayerPosition();
            transform.LookAt(PlayerManager.Instance.GetPlayerPosition());
            ResetBools();
        }
        /*private IEnumerator CheckMeleeRange()
        {
            yield return new WaitForSeconds(0.5f);
            if (_hitzone.GetComponent<HitBox>().HasTarget() && _hitAnimation == false)
            {
                Debug.Log("PLAYER GOT HIT");
            }
        }*/

        private IEnumerator CheckMeleeRange()
        {
            _playerPosition = PlayerManager.Instance.GetPlayerPosition();
            _meleeVector = _playerPosition - transform.position;
            transform.LookAt(PlayerManager.Instance.GetPlayerPosition());
            _meleeVector.y = 0;
            _meleeVector = _meleeVector.normalized;
            _meleeVector.y = jumpheigth;
            
            //_meleeVector = _meleeVector * -1;
            //_rb.constraints = RigidbodyConstraints.FreezePosition;
            yield return new WaitForSeconds(waitattack);
            //_rb.AddForce(_meleeVector * attackpower, ForceMode.Impulse);
            if (_navAgent.enabled == true)
                _navAgent.enabled = false;
            _rb.AddForce(_meleeVector * attackpower, ForceMode.Impulse);
        }

        private IEnumerator StopAttack()
        {
            yield return new WaitForSeconds(waitattack); 
            if(_rb.linearVelocity.magnitude >= 0.2f)
                _rb.AddForce(_meleeVector * -0.5f, ForceMode.Impulse);
        }

        private IEnumerator StopHitAnimation()
        {
            yield return new WaitForSeconds(durationanimation);
            ResetBools();
        }
        
        public void ResetBools()
        {
            _hitAnimation = false;
            _anim.SetBool("GotHit", false);

            _meleeAtkAnimation = false;
            _anim.SetBool("CanHit", false);
            
            _anim.SetBool("IsIdle", false);
        }
        
        public void SetIdleAnimation() => _anim.SetBool("IsIdle", true);

        public void DeathAnimation()
        {
            if(_navAgent.enabled == true)
                _navAgent.enabled = false;
            _anim.SetTrigger("IsDead");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void RangedAttack()
        {
            if (_hitAnimation)
            {
                _anim.SetBool("GotHit", true);
                StartCoroutine(nameof(WaitAndResetAnimationsHit));
            }

            //if (_rangedAttackAnimation)
            //{
                if (_currentDamageCooldown <= 0)
                {
                    RaycastHit hit;
                    _rangedVector = PlayerManager.Instance.GetPlayerPosition()-transform.position;
                    if(Physics.Raycast(transform.position, _rangedVector, out hit, Mathf.Infinity))
                    {
                        if (hit.collider != null && hit.collider.CompareTag("Player"))
                        {
                            _anim.SetBool("CanHit", true);
                            if (_navAgent.enabled)
                                _navAgent.enabled = false;
                            transform.LookAt(PlayerManager.Instance.GetPlayerPosition());
                            Instantiate(_projectilePrefab, transform.position + Vector3.up, Quaternion.identity);
                            _currentDamageCooldown = _baseDamageCooldown; 
                            if (!_navAgent.enabled)
                                _navAgent.enabled = true;
                        }
                    }
                }
            //}
        }
    }
}
