using System.Collections;
using UnityEngine;
using WPM.UI.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPM.SayIt.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private WordController m_myWordController;
        [SerializeField] private GameObject m_myWordContainer;
        [SerializeField] private MenuController m_myMenuController;
        [SerializeField] private SpeechBubbleController m_mySpeechBubbleController;
        [SerializeField] private SpeechBubbleTail m_mySpeechBubbleTail;
        [SerializeField] private WordPreparer m_myWordPreparer;
        [SerializeField] private VoicePlayer m_myVoicePlayer;
        [SerializeField] private CoinCounter m_myCoinCounter;
        [SerializeField] private Coiner m_myCoiner;

        public int[] m_imageSwitchNumber;
        int m_imageSwitchCounter = 0;
        int m_currentImage = 0;

        [SerializeField] ImageLoader m_myImageLoader;

        [SerializeField] private GameObject[] m_characterList;

        //public int m_currentLevelScore = 0;
        //public int m_totalScore = 0;

        private int m_currentEpisode = 1;
        private int m_currentPhraseIndex = 0;
        private int m_phrasesCount;
        public int m_totalNumberOfPhrases = 0;
        public int m_currentNumberOfPhrases = 0;

        //private string m_currentCharacter;
        private int m_currentCharacterID;

        [SerializeField] private float m_startGameDelay = 2.0f;

        private void Start()
        {

        }

        /// <summary>
        /// Start game
        /// </summary>
        public void StartGame()
        {
            //SetEpisode(m_currentEpisode);
            m_myWordController.PreloadWordsForEpisode(m_currentEpisode);
            m_phrasesCount = m_myWordPreparer.GetNumberOfPhrases(m_currentEpisode);
            m_imageSwitchNumber = m_myWordPreparer.GetImagesSwitcher(m_currentEpisode);
            m_totalNumberOfPhrases = m_myWordPreparer.GetNumberOfPhrases(m_currentEpisode);

            GoNextGame();
            //StartCoroutine("LateStart");
        }

        /// <summary>
        /// Restart current phrase counter, and load new phrase to start level over again
        /// </summary>
        public void RestartLevel()
        {
            m_imageSwitchCounter = 0;
            m_currentImage = 0;
            m_currentPhraseIndex = 0;
            m_myVoicePlayer.SetCurrentClipIndex(m_currentPhraseIndex);
            m_myCoinCounter.Reset();
            m_myWordController.Reset();            
            m_myWordController.DestroyWordContainerSlots();
            SetEpisode(m_currentEpisode);
        }

        public void SetEpisode(int _episode)
        {
            if (_episode <= 0)
            {
                Debug.LogWarning("Episode number is lower then 0, value has been set to 0");
                m_currentEpisode = 0;
            }

            m_currentEpisode = _episode;
            Debug.Log("Words preloaded for episode with index: " + m_currentEpisode);
        }

        /*
        /// <summary>
        /// Delay function to start the game. Other functions can be loaded before starting new game.
        /// </summary>
        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(m_startGameDelay);

            m_phrasesCount = m_myWordPreparer.GetNumberOfPhrases(m_currentEpisode);
            m_imageSwitchNumber = m_myWordPreparer.GetImagesSwitcher(m_currentEpisode);
            m_totalNumberOfPhrases = m_myWordPreparer.GetNumberOfPhrases(m_currentEpisode);

            GoNextGame();
        }
        */

        /// <summary>
        /// Call functions that needs to be used before next sentence
        /// </summary>
        public void GoNextGame()
        {
            print("next game started");
            if (m_myMenuController.IsHintActive())
            {
                m_myMenuController.HideHintButton();
            }                

            CalculateAndUpdateProgressBar();
            m_myWordController.m_mySpeechBubbleController.ChangeBubbleToActive();

            //m_myWordController.ResetAttributes();
            // Check if it's end of level
            if (m_currentPhraseIndex != m_phrasesCount)
            {
                // Get current character
                m_myWordController.PrepareWords(m_currentPhraseIndex);
            } else
            {
                PlayBigHappyReaction();
                StartCoroutine("DelayOpenScoreScreen");
            }

            m_currentPhraseIndex++;

            m_currentCharacterID = m_myWordController.GetCurrentCharacterName();
            /*
            // Set currently speaking character
            if (m_myWordController.GetCurrentCharacterName().Equals("Harry"))
            {
                m_currentCharacterID = 0;
            }
            else if (m_myWordController.GetCurrentCharacterName().Equals("George"))
            {
                m_currentCharacterID = 1;
            }
            */

            m_mySpeechBubbleTail.UpdateTailForCharacter(m_currentCharacterID);

            // Switch subject image to appropriate one
            m_myImageLoader.SwitchOnThisObject();
            m_myImageLoader.LoadImage(m_currentImage);
            m_currentImage++;
            /*
            if (m_imageSwitchCounter == m_imageSwitchNumber[m_currentImage])
            {
                Debug.Log("Load new image");
                m_imageSwitchCounter = 1;
            }
            else
            {
                Debug.Log("Add switch counter");
                m_imageSwitchCounter++;
            }
            */
        }

        private IEnumerator DelayOpenScoreScreen()
        {
            yield return new WaitForSeconds(3.0f);
            m_myMenuController.OpenScoreScreen();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateAndUpdateProgressBar()
        {
            float l_progress = 0.0f;

            l_progress = (float)m_currentPhraseIndex/(float)m_phrasesCount;

            if (m_currentPhraseIndex == (m_phrasesCount))
            {

                PlayHighFiveReaction();
                StartCoroutine("DelayOpenScoreScreen");
            } else
            {
                m_myMenuController.UpdateProgressBar(l_progress);
            }
            if (l_progress >0)
            {
                m_myCoiner.AddCoins(3);
            }

        }



        public void PlayHighFiveReaction()
        {
            foreach (GameObject character in m_characterList)
            {
                character.GetComponent<CharacterAnimation>().PlayHighFive();
            }
        }
        
        public void PlaySadReaction()
        {
            m_characterList[m_currentCharacterID].GetComponent<CharacterAnimation>().PlaySad();
        }

        public void PlaySmallHappyReaction()
        {
            m_characterList[m_currentCharacterID].GetComponent<CharacterAnimation>().PlaySmallHappy();
        }

        public void PlayBigHappyReaction()
        {
            m_characterList[m_currentCharacterID].GetComponent<CharacterAnimation>().PlayBigHappy();
        }
        
    }
}