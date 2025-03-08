using UnityEngine;

namespace Enemies
{
    public class HitBox : MonoBehaviour
    {
        private bool _playerInside;

        public bool HasTarget() => _playerInside;
        private void OnTriggerStay(Collider other) => _playerInside = other.gameObject.CompareTag("Player");
        private void OnTriggerExit(Collider other) =>  _playerInside = false;
    }
}
