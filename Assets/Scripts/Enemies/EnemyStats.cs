using Managers;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float _health = 250f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(_health <= 0)
            Destroy(gameObject);
    }
    
    public void ReceiveDamageEnemy(float damage)
    {
        _health -= damage * PlayerManager.Instance.GetPlayerVelocity();
    }
}
