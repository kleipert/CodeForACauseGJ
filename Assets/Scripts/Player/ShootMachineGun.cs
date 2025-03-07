using System.Collections;
using Enemies;
using InputSystem;
using Managers;
using UnityEngine;

namespace Player
{
    public class ShootMachineGun : MonoBehaviour
    {
        // Machine Gun variables
        [SerializeField] private float _baseCooldown = 0.1f;
        [SerializeField] private float _bulletDamage = 2f;
        [SerializeField] private GameObject _shotVFX;
        private ParticleSystem _laserVFX;
        private float _activeCooldown;


        private InputSettingsInput _input;
        private GameObject _mainCamera;
        
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _input = GetComponent<InputSettingsInput>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _laserVFX = _shotVFX.GetComponent<ParticleSystem>();
            _activeCooldown = 0f;
            StopLaserBeam();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.GetCurrentMovementType() == MovementType.Grapple)
            {
                if (_input.shot)
                {
                    _shotVFX.transform.forward = _mainCamera.transform.forward;
                    ActivateLaserBeam();
                    if (_activeCooldown <= 0f)
                    {
                        _activeCooldown = _baseCooldown;
                        RaycastHit rc_hit;
                        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out rc_hit))
                        {
                        
                            if (rc_hit.transform.CompareTag("Enemy"))
                            {
                                rc_hit.transform.GetComponent<EnemyStats>().ReceiveDamageEnemy(_bulletDamage);
                            }
                        }
                    }
                }
                else
                    StopLaserBeam();
                
                _activeCooldown -= Time.deltaTime;
            }
        }
        
        private void ActivateLaserBeam()
        {
            _shotVFX.SetActive(true);
            if(!_laserVFX.isPlaying)
                _laserVFX.Play();
        }
        
        private void StopLaserBeam()
        {
            if(_laserVFX.isPlaying)
                _laserVFX.Stop();
            _shotVFX.SetActive(false);
        }
    }
}
