using UnityEngine;
using UnityEngine.SceneManagement;

namespace WPM.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void ExitGame()
        {
            //UNITY_WEBGL
            Application.OpenURL("about:blank");

            Application.Quit();
    #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            //Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
    #endif
    #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
    #elif (UNITY_STANDALONE)
        Application.Quit();
    #endif

        }
    }
}
