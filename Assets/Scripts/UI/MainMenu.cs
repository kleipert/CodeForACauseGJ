using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadNextScene()
        {
            SceneManager.LoadScene("Medieval");

        }
        
        public void ExitGame()
        {
            SceneManager.LoadScene("Medieval");

        }
    }
}
