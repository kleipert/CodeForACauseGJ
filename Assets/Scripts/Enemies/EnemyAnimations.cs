using System.Collections;
using Managers;
using Player;
using Unity.VisualScripting;
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
        private WalkToPlayer _walkToPlayer;
        private float _baseDamageCooldown = 5f;
        private float _currentDamageCooldown;
        private Vector3 _playerPosition;
        private Rigidbody _rb;
        private Vector3 _meleeVector;
        [SerializeField] private float waitattack = 0.5f;
        [SerializeField] private float attackpower = 5f;
        [SerializeField] private float jumpheigth = 5f;


        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _base = GetComponent<EnemyBase>();
            _type = _base.GetEnemyType();
            _rb = GetComponent<Rigidbody>();
            _walkToPlayer = GetComponent<WalkToPlayer>();
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
                    //_walkToPlayer.FollowPlayer = false;
                    if (_navAgent.enabled == true)
                        _navAgent.enabled = false;
                    _anim.SetBool("CanHit", true);
                    StartCoroutine(nameof(CheckMeleeRange)); 
                    StartCoroutine(nameof(WaitAndResetAnimations));
                    StartCoroutine(nameof(StopAttack));
                    _currentDamageCooldown = _baseDamageCooldown;
                }
                
            }
                
        }
        
        public void PlayHitAnimation() => _hitAnimation = true;
        public void PlayMeleeAttackAnimation() => _meleeAtkAnimation = true;
        
        
        private IEnumerator WaitAndResetAnimations()
        {
            yield return new WaitForSeconds(5f);
            ResetBools();
            _walkToPlayer.FollowPlayer = true;
            
            /*if(_navAgent.enabled)
                _navAgent.isStopped = false;*/
            if(_navAgent.enabled == false)
                _navAgent.enabled = true;
            _playerPosition = PlayerManager.Instance.GetPlayerPosition();
            Vector3.RotateTowards(transform.forward, _meleeVector, 360f, 0.0f);
            _currentDamageCooldown = _baseDamageCooldown;
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
            Vector3.RotateTowards(transform.forward, _meleeVector, 360f, 0.0f);
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
        
        private void ResetBools()
        {
            _hitAnimation = false;
            _anim.SetBool("GotHit", false);

            _meleeAtkAnimation = false;
            _anim.SetBool("CanHit", false);
        }
    }
}
