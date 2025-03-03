using UnityEngine;

namespace Player
{
    public class PlayerImpactHandler : MonoBehaviour
    {
        Vector3 impact = Vector3.zero;
        private CharacterController character;
        // Use this for initialization
        void Start () 
        {
            character = GetComponent<CharacterController>();
        }
        void Update () 
        {
            // apply the impact force:
            if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
        }
        
        
        public void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
            impact += dir.normalized * force;
        }
    }
}
