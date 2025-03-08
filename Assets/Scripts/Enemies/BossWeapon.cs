using System;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class BossWeapon : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                PlayerManager.Instance.ReceiveDamagePlayer(20f);
        }
    }
}
