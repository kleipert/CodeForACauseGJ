using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth = 250;
        [SerializeField] private float _playerVelocity = 0f;
        [SerializeField] private GameObject _uiSystem;
        private FirstPersonController _fpsController;
        private Vector3 _playerPositonVector;
        private float _damageCooldown = 0.5f;
        private float _currentDamageCooldown = 0.5f;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _fpsController = GetComponent<FirstPersonController>(); 
            _playerPositonVector = transform.position;
            _currentHealth = _maxHealth;
            SetMaxHealthInUI();
            UpdateHealthBar();
        }

        // Update is called once per frame
        void Update()
        {
            _playerVelocity = Mathf.Abs((transform.position - _playerPositonVector).magnitude);
            _playerPositonVector = transform.position;
            if (_currentHealth <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _currentDamageCooldown -= Time.deltaTime;
        }

        public float GetPlayerVelocity()
        {
            return _playerVelocity;
        }

        public void ReceiveDamagePlayer(float damage)
        {
            if (_currentDamageCooldown <= 0f)
            {
                _currentHealth -= (int) damage;
                UpdateHealthBar();
                _currentDamageCooldown = _damageCooldown;
            }
        }

        private void SetMaxHealthInUI()
        {
            _uiSystem.GetComponent<HealthBar>().SetMaxHealth(_maxHealth);
        }

        private void UpdateHealthBar()
        {
            _uiSystem.GetComponent<HealthBar>().SetNewHealthByPercent(_currentHealth);
        }

        /*private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
            Debug.Log("Player Hit");
    }*/
    }
}
