using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _health = 250f;
    [SerializeField] private float _playerVelocity = 0f;
    private FirstPersonController _fpsController;
    private Vector3 _playerPositonVector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fpsController = GetComponent<FirstPersonController>(); 
        _playerPositonVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _playerVelocity = Mathf.Abs((transform.position - _playerPositonVector).magnitude);
        _playerPositonVector = transform.position;
        if (_health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetPlayerVelocity()
    {
        return _playerVelocity;
    }

    public void ReceiveDamagePlayer(float damage)
    {
        _health -= damage;
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
            Debug.Log("Player Hit");
    }*/
}
