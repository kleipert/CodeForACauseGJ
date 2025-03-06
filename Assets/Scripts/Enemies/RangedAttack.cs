using System.Collections;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class RangedAttack : MonoBehaviour
    {
        Vector3 targetPosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            targetPosition = ((PlayerManager.Instance.GetPlayerPosition()-transform.position) * 2) + transform.position;
            transform.up = -targetPosition;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.1f);
            if (transform.position == targetPosition)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                return;
            if(other.gameObject.CompareTag("Player"))
                PlayerManager.Instance.ReceiveDamagePlayer(20f);
            Destroy(gameObject);
        }
        
    }
}
