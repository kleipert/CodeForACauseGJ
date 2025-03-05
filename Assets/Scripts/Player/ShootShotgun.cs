using InputSystem;
using Managers;
using UnityEngine;

namespace Player
{
    public class ShootShotgun : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileTarget;
        [SerializeField] private float _baseCooldown = 1f;
        [SerializeField] private int _bulletsPerShot = 6;
        
        private InputSettingsInput _input;
        private float _activeCooldown;
        private AudioSource _audioSource;
        
        
    
        void Start()
        {
            _input = GetComponent<InputSettingsInput>();
            _audioSource = GetComponent<AudioSource>();
            _activeCooldown = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.GetCurrentMovementType() != MovementType.Dash) 
                return;
            
            if (_input.shot && _activeCooldown <= 0f)
            {
                _activeCooldown = _baseCooldown;
                SpawnBullets();
                _audioSource.Play();
                
            }
            _activeCooldown -= Time.deltaTime;
        }

        private void SpawnBullets()
        {
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                Instantiate(_projectilePrefab, _projectileTarget.transform.position, _projectileTarget.transform.rotation);
            }
        }
    }
}

