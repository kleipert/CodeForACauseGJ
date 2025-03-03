using InputSystem;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class ShootMachineGun : MonoBehaviour
    {
        // Machine Gun variables
        [SerializeField] private Transform _projectileTarget;
        [SerializeField] private float _baseCooldown = 0.2f;
        private float _activeCooldown;

        private InputSettingsInput _input;
        private GameObject _mainCamera;
        
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _input = GetComponent<InputSettingsInput>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _activeCooldown = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.GetCurrentMovementType() == MovementType.Grapple)
            {
                if (_input.shot && _activeCooldown <= 0f)
                {
                    _activeCooldown = _baseCooldown;
                    RaycastHit rc_hit;
                    if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out rc_hit))
                    {
                        // Shoot VFX from projectile target
                        // Impact VFX
                        // Damage player
                        if (rc_hit.transform.CompareTag("Enemy"))
                        {
                            // Do damage
                        }
                    }
                }
                _activeCooldown -= Time.deltaTime;
            }
        }
    }
}
