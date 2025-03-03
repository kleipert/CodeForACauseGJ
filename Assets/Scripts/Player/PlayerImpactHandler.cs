using UnityEngine;

namespace Player
{
    public class PlayerImpactHandler : MonoBehaviour
    {
        Vector3 _impact = Vector3.zero;
        private CharacterController _character;
        void Start () 
        {
            _character = GetComponent<CharacterController>();
        }
        void Update () 
        {
            if (_impact.magnitude > 0.2F) 
                _character.Move(_impact * Time.deltaTime);
            _impact = Vector3.Lerp(_impact, Vector3.zero, 5*Time.deltaTime);
        }
        
        
        public void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0) 
                dir.y = -dir.y; 
            _impact += dir.normalized * force;
        }
    }
}
