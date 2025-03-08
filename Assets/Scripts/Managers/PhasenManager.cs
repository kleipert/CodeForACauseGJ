using System.Collections;
using Mech;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class PhasenManager : MonoBehaviour
    {
        public static PhasenManager Instance;
        [SerializeField] private GameObject phase1;
        [SerializeField] private GameObject phase2;
        [SerializeField] private GameObject phase3;
        [SerializeField] private GameObject startBossFight;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject boss;
        [SerializeField] private GameObject crystal1;
        [SerializeField] private GameObject crystal2; 
        public bool endPhase1 { get; private set; }
        public bool endPhase2 { get; private set; }
        public bool endPhase3 { get; private set; }
        public bool betweenPhase { get; private set; }
        private BossObserver bossObserver;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            phase1.SetActive(true);
            phase2.SetActive(false);
            phase3.SetActive(false);
            bossObserver = boss.GetComponent<BossObserver>();
            betweenPhase = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(player.transform.position, startBossFight.transform.position) < 10f)
            {
                phase1.SetActive(false);
                endPhase1 = true;
                betweenPhase = false;
            }

            if (!crystal1 && !endPhase2)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = startBossFight.transform.position;
                player.GetComponent<CharacterController>().enabled = true;
                StartCoroutine(DeactivatePhaseTwo());
                endPhase2 = true;
                bossObserver.EndPhase2();
                betweenPhase = false;
            }

            if (!crystal2 && !endPhase3)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = startBossFight.transform.position;
                player.GetComponent<CharacterController>().enabled = true;
                StartCoroutine(DeactivatePhaseThree());
                endPhase3 = true;
                bossObserver.EndPhase3();
                betweenPhase = false;
            }
            
            if (Vector3.Distance(player.transform.position, startBossFight.transform.position) > 500f)
            {
                SceneManager.LoadScene("SpaceStation");
            }
        }

        private IEnumerator DeactivatePhaseTwo()
        {
            yield return new WaitForSeconds(.5f);
            phase2.SetActive(false);
        }
        
        private IEnumerator DeactivatePhaseThree()
        {
            yield return new WaitForSeconds(.5f);
            phase3.SetActive(false);
        }

        public void StartPhase2()
        {
            phase2.SetActive(true);
            betweenPhase = true;
        }

        public void StartPhase3()
        {
            phase3.SetActive(true);
            betweenPhase = true;
        }
    }
}

