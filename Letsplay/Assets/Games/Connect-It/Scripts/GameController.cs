using System.Collections;
using UnityEngine;
using WPM.SayIt.Core;

namespace WPM.Connect.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] WordSpawner m_myWordSpawner;
        [SerializeField] CoinCounter m_myCoinCounter;
        [SerializeField] LifePointsController m_myLifePointController;
        [SerializeField] MenuController m_myMenuController;
        [SerializeField] BonusController m_myBonusController;

        int m_currentEpisode = 0;
        int m_nativeLanguageIndex = 0;

        int m_currentLifePoints = 3;
        int m_initialLifePoints = 3;

        public void Start()
        {
            Reset();
            m_myWordSpawner.PrepareWordSpawner(m_currentEpisode, m_nativeLanguageIndex);
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            Invoke("DelayedStart", 2);
        }

        private void DelayedStart()
        {
            m_myWordSpawner.SetSpawnerActive(true);
        }

        public void RestartGame()
        {
            Reset();
            m_myWordSpawner.PrepareWordSpawner(m_currentEpisode, m_nativeLanguageIndex);
            Invoke("DelayedStart", 2);
            Time.timeScale = 1;
        }

        public void SpawnerSwitch()
        {
            if (m_myWordSpawner.GetIsActive())
            {
                m_myWordSpawner.SetSpawnerActive(false);
            } else
            {
                m_myWordSpawner.SetSpawnerActive(true);
            }
        }

        public void DecreaseLifePoints()
        {
            m_currentLifePoints--;
            m_myLifePointController.UpdateLifePoints(m_currentLifePoints);

            if (m_currentLifePoints == 0)
            {
                StartCoroutine("DelayGameOverScreen");
            }
        }

        IEnumerator DelayGameOverScreen()
        {
            yield return new WaitForSeconds(2.0f);
            Time.timeScale = 0;
            m_myMenuController.ShowGameOverScreen();
        }

        private void Reset()
        {
            m_myWordSpawner.Reset();
            m_myCoinCounter.Reset();
            m_myBonusController.Reset();
            m_currentLifePoints = m_initialLifePoints;
            m_myLifePointController.UpdateLifePoints(m_currentLifePoints);            
        }

        public void SetEpisode(int _episode)
        {
            m_currentEpisode = _episode - 1;
            Debug.Log("Current episode set to: " + m_currentEpisode);
        }

        public void SetNativeLanguage(int _language)
        {
            m_nativeLanguageIndex = _language;
            m_myWordSpawner.SetNativeLanguageIndex(_language);
            Debug.Log("Current episode set to: " + m_nativeLanguageIndex);
        }
    }
}