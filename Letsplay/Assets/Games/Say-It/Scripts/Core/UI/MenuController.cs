using UnityEngine;
using UnityEngine.UI;
using WPM.UI.Effects;
using UnityEngine.SceneManagement;
using TMPro;

namespace WPM.SayIt.Core
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] GameController m_myGameController;
        [SerializeField] AudioSource m_myMusicPlayer;

        [SerializeField] GameObject m_myMainMenu;
        [SerializeField] GameObject m_myPauseMenu;
        [SerializeField] GameObject m_myScoreScreen;
        [SerializeField] GameObject m_myTutorialScreen;

        [SerializeField] Button m_menuButton;
        [SerializeField] GameObject m_hintButton;
        [SerializeField] GameObject m_progressBar;
        [SerializeField] GameObject m_coiner;
        [SerializeField] TextMeshProUGUI m_scoreScreenTotalCoins;


        private PopUpEffect m_mainMenuPopUpComponent;
        private PopUpEffect m_menuPopUpComponent;
        private PopUpEffect m_scoreScreenPopUpComponent;
        private PopUpEffect m_tutorialScreenPopUpComponent;

        private PopUpEffect m_menuButtonPopUpEffect;
        private PopUpEffect m_hintButtonPopUpEffect;
        private PopUpEffect m_progressBarPopUpEffect;
        private PopUpEffect m_coinerPopUpEffect;


        void Start()
        {
            m_mainMenuPopUpComponent = m_myMainMenu.GetComponentInChildren<PopUpEffect>();
            m_menuPopUpComponent = m_myPauseMenu.GetComponentInChildren<PopUpEffect>();
            m_scoreScreenPopUpComponent = m_myScoreScreen.GetComponentInChildren<PopUpEffect>();
            m_tutorialScreenPopUpComponent = m_myTutorialScreen.GetComponentInChildren<PopUpEffect>();

            m_menuButtonPopUpEffect = m_menuButton.GetComponent<PopUpEffect>();
            m_hintButtonPopUpEffect = m_hintButton.GetComponent<PopUpEffect>();
            m_progressBarPopUpEffect = m_progressBar.GetComponent<PopUpEffect>();
            m_coinerPopUpEffect = m_coiner.GetComponent<PopUpEffect>();
            CloseMainMenuScreen();
            ShowInterface();
            StartGame();
        }

        public void ShowInterface()
        {
            m_menuButtonPopUpEffect.MaximiseWindow();
            m_hintButtonPopUpEffect.MaximiseWindow();
            m_progressBarPopUpEffect.MaximiseWindow();
            m_coinerPopUpEffect.MaximiseWindow();
        }

        public void OpenPauseMenu()
        {
            m_myPauseMenu.SetActive(true);
            m_menuButton.enabled = false;
            m_menuPopUpComponent.MaximiseWindow();
        }

        public void ClosePauseMenu()
        {
            m_menuButton.enabled = true;
            m_menuPopUpComponent.MinimiseWindow();
            Invoke("DelayPauseMenu", 0.5f);
        }

        private void DelayPauseMenu()
        {
            m_myPauseMenu.SetActive(false);
        }

        public void OpenMainMenuScreen()
        {
            m_myMainMenu.SetActive(true);
        }

        public void CloseMainMenuScreen()
        {
            m_mainMenuPopUpComponent.MinimiseWindow();
            Invoke("DisableMainMenu", 1.0f);
        }

        private void DisableMainMenu()
        {
            m_myMainMenu.SetActive(false);
        }

        public void OpenScoreScreen()
        {
            m_myScoreScreen.SetActive(true);
            m_scoreScreenTotalCoins.text = m_coiner.GetComponentInChildren<CoinCounter>().coinCount.ToString();
            m_scoreScreenPopUpComponent.MaximiseWindow();
        
        }

        public void CloseScoreScreen()
        {
            m_scoreScreenPopUpComponent.MinimiseWindow();
            Invoke("DisableScoreScreen", 1.0f);
        }

        private void DisableScoreScreen()
        {
            m_myScoreScreen.SetActive(false);
        }

        public void OpenTutorialScreen()
        {
            m_myTutorialScreen.SetActive(true);
            m_tutorialScreenPopUpComponent.MaximiseWindow();
        }

        public void CloseTutorialScreen()
        {
            m_tutorialScreenPopUpComponent.MinimiseWindow();
            Invoke("DisableTutorialScreen", 1.0f);
        }

        private void DisableTutorialScreen()
        {
            m_myScoreScreen.SetActive(false);
        }

        public void StartGame()
        {
            m_myGameController.StartGame();
        }

        public void PlayLevelAgain()
        {
            CloseScoreScreen();
            m_myGameController.RestartLevel();
        }

        public void ShowHintButton()
        {
            m_hintButton.SetActive(true);
            m_hintButton.GetComponentInChildren<PopUpEffect>().MaximiseWindow();
        }

        public void HideHintButton()
        {
            if (!m_hintButton.activeSelf) return;

            m_hintButton.GetComponentInChildren<PopUpEffect>().MinimiseWindow();
            Invoke("DelayDisabling", 1.0f);
        }

        public bool IsHintActive()
        {
            return m_hintButton.activeSelf;
        }

        private void DelayDisabling()
        {
            m_hintButton.SetActive(false);
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void UpdateProgressBar(float _currentPhrase)
        {
            m_progressBar.GetComponent<Slider>().value = _currentPhrase;
        }

        public void QuitGame()
        {
            Application.Quit();
            Application.OpenURL("https://wordplay.media/Game");
        }

        public void SwitchMusic()
        {
            if (m_myMusicPlayer.isPlaying)
            {
                m_myMusicPlayer.Stop();
            }
            else
            {
                m_myMusicPlayer.Play();
            }
        }
    }
}