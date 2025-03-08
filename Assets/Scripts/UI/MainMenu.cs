using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene("Medieval");

        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
