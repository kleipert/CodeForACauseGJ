using InputSystem;
using Managers;
using UnityEngine;

namespace Player
{
    public class ShootRocketMissile : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileTarget;
        [SerializeField] private float _baseCooldown = 3f;
        
        private InputSettingsInput _input;
        private GameObject _mainCamera;
        private bool _isLoaded;
        private GameObject _loadedProjectile;
        private float _activeCooldown;
        private LayerMask _player;
        
        
    
        void Start()
        {
            _input = GetComponent<InputSettingsInput>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _activeCooldown = 0f;
            _isLoaded = false;
            _player = LayerMask.GetMask("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.GetCurrentMovementType() != MovementType.JetPack) 
                return;
            
            if (_isLoaded && _input.shot && _activeCooldown <= 0f)
            {
                _activeCooldown = _baseCooldown;
                _isLoaded = false;
                RaycastHit rc_hit;
                if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out rc_hit, Mathf.Infinity, ~_player.value))
                {
                    //Debug.DrawLine(_mainCamera.transform.position, rc_hit.point, Color.magenta, 120);
                    _loadedProjectile.transform.SetParent(null);
                    _loadedProjectile.GetComponent<MissileProjectile>().SetTarget(rc_hit.point);
                    _loadedProjectile.GetComponent<MissileProjectile>().SetFired(true);
                }
            }

            if (_loadedProjectile == null && _activeCooldown <= 0f)
            {
                _loadedProjectile = Instantiate(_projectilePrefab, _projectileTarget.transform.position, _projectileTarget.transform.rotation, _projectileTarget);
                _isLoaded = true;
            }

            _activeCooldown -= Time.deltaTime;
        }
    }
}
