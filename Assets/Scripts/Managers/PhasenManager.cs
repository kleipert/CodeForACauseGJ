using UnityEditor.Rendering;
using UnityEngine;

namespace Managers
{
    public class PhasenManager : MonoBehaviour
    {
        [SerializeField] private GameObject phase1;
        [SerializeField] private GameObject phase2;
        [SerializeField] private GameObject phase3;
        [SerializeField] private GameObject startBossFight;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject crystal1;
        [SerializeField] private GameObject crystal2;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            phase1.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(player.transform.position, startBossFight.transform.position) < 10f)
            {
                phase1.SetActive(false);
            }

            if (!crystal1)
            {
                player.transform.position = startBossFight.transform.position;
                phase2.SetActive(false);
            }

            if (!crystal2)
            {
                player.transform.position = startBossFight.transform.position;
                phase3.SetActive(false);
            }
        }

        public void StartPhase2()
        {
            phase2.SetActive(true);
        }

        public void StartPhase3()
        {
            phase3.SetActive(true);
        }
    }
}

