using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WPM.UI.Effects;

namespace WPM.Connect.Core
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] GameController m_myGameController;
        [SerializeField] AudioSource m_myMusicPlayer;

        [SerializeField] GameObject m_myMainMenuScreen;
        [SerializeField] GameObject m_myPauseScreen;
        [SerializeField] GameObject m_myVictoryScreen;
        [SerializeField] GameObject m_myDefeatScreen;

        [SerializeField] GameObject m_myMainMenuTitle;
        [SerializeField] Button m_myPlayButton;
        [SerializeField] Button m_mySettingsButton;
        [SerializeField] Button m_myExitButton;

        void Start()
        {
            //StartCoroutine("ShowMainMenu");
            PlayGame();
        }

        public void PlayGame()
        {
            StartCoroutine("HideMainMenu");
            m_myGameController.StartGame();
        }

        public void RestartGame()
        {
            m_myDefeatScreen.SetActive(false);
            m_myGameController.RestartGame();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ShowGameOverScreen()
        {
            m_myDefeatScreen.SetActive(true);
        }

        public void HideGameOverScreen()
        {
            m_myDefeatScreen.SetActive(false);
        }

        public void ShowPauseScreen()
        {
            Time.timeScale = 0;
            m_myPauseScreen.SetActive(true);
        }

        public void HidePauseScreen()
        {
            m_myPauseScreen.SetActive(false);
            Time.timeScale = 1;
        }

        public void SwitchMusic()
        {
            if (m_myMusicPlayer.isPlaying)
            {
                m_myMusicPlayer.Stop();
            } else
            {
                m_myMusicPlayer.Play();
            }
        }

        IEnumerator ShowMainMenu()
        {        
            m_myMainMenuScreen.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_myMainMenuTitle.GetComponent<PopUpEffect>().MaximiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_myPlayButton.GetComponent<PopUpEffect>().MaximiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_mySettingsButton.GetComponent<PopUpEffect>().MaximiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_myExitButton.GetComponent<PopUpEffect>().MaximiseWindow();
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator HideMainMenu()
        {
            yield return new WaitForSeconds(0.5f);
            m_myPlayButton.GetComponent<PopUpEffect>().MinimiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_mySettingsButton.GetComponent<PopUpEffect>().MinimiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_myExitButton.GetComponent<PopUpEffect>().MinimiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_myMainMenuTitle.GetComponent<PopUpEffect>().MinimiseWindow();
            yield return new WaitForSeconds(0.5f);
            m_myMainMenuScreen.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_myGameController.StartGame();
        }
    }
}