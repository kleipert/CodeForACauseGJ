using System;
using Player;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        [SerializeField] private GameObject _player;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
        }

        public void MovePlayer(Vector3 dir, float strength)
        {
            _player.GetComponent<PlayerImpactHandler>().AddImpact(dir, strength);
        }

        public void SetMovementAbility(MovementType movementType)
        {
            _player.GetComponent<FirstPersonController>().MovementAbility = (int)movementType;
        }

        public MovementType GetCurrentMovementType()
        {
            return (MovementType) _player.GetComponent<FirstPersonController>().MovementAbility;
        }

        public Transform GetPlayerTransform() => _player.transform;
        
        public float GetPlayerVelocity() => _player.GetComponent<PlayerStats>().GetPlayerVelocity();
        
        public Vector3 GetPlayerPosition()
        {
            return _player.transform.position;
        }

    }
}
