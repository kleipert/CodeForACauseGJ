using System;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ShootProjectile : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileTarget;
        [SerializeField] private float _baseCooldown = 3f;
        
        private InputSettingsInput _input;
        private GameObject _mainCamera;
        private bool _isLoaded;
        private GameObject _loadedProjectile;
        private float _activeCooldown;
        
        
    
        void Start()
        {
            _input = GetComponent<InputSettingsInput>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _loadedProjectile = Instantiate(_projectilePrefab, _projectileTarget.transform.position, _projectileTarget.transform.rotation, _projectileTarget);
            _isLoaded = true;
            _activeCooldown = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isLoaded && _input.shot && _activeCooldown <= 0f)
            {
                _activeCooldown = _baseCooldown;
                _isLoaded = false;
                RaycastHit rc_hit;
                if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out rc_hit))
                {
                    //Debug.DrawLine(_mainCamera.transform.position, rc_hit.point, Color.magenta, 120);
                    _loadedProjectile.transform.SetParent(null);
                    _loadedProjectile.GetComponent<Projectile>().SetTarget(rc_hit.point);
                    _loadedProjectile.GetComponent<Projectile>().SetFired(true);
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
