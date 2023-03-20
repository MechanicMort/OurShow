using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace WPM.SayIt.Core
{
    public class WordController : MonoBehaviour
    {
        [SerializeField] WordPreparer m_myWordPreparer;

        [SerializeField] private WordContainer m_myWordContainer;
        [SerializeField] public SpeechBubbleController m_mySpeechBubbleController;
        private SpeechBubble m_myActiveSpeechBubble;

        [SerializeField] GameObject m_wordContainerSlotPrefab;
        [SerializeField] GameObject m_bubbleSlotPrefab;

        private string[] m_sentence;
        public string[] m_shuffledSentence;

        private Characters m_characterName;

        private bool m_gameReady = false;

        public void Reset()
        {
            m_myWordPreparer.Reset();
            m_mySpeechBubbleController.Reset();
        }

        public void PreloadWordsForEpisode(int _currentEpisode)
        {
            m_myWordPreparer.PrepareSentence(_currentEpisode - 1);
        }

        public int GetCurrentCharacterName()
        {
            return (int)m_characterName;
        }

        public bool IsGameReady()
        {
            return m_gameReady;
        }

        public SpeechBubble GetActiveSpeechBubble()
        {
            return m_myActiveSpeechBubble;
        }

        /// <summary>
        /// Populate scrambled words container and speech bubble with words.
        /// </summary>
        public void PrepareWords(int _currentPhraseIndex)
        {
            m_gameReady = false;
            DestroyWordContainerSlots();
            m_myActiveSpeechBubble = m_mySpeechBubbleController.GetActiveSpeechBubble().GetComponent<SpeechBubble>();

            // Get phrase from the collection of current episodes
            //PhraseObject l_phraseObject = m_myTextReader.m_phraseCollection[_currentPhraseIndex];
            //m_characterName = l_phraseObject.name;

            PhraseObject l_phraseObject = m_myWordPreparer.m_phraseCollection[_currentPhraseIndex];
            m_characterName = l_phraseObject.name;

            m_sentence = l_phraseObject.singleWords;

            // Determine punctuational mark in the sentence
            string t_lastWord = m_sentence[m_sentence.Length - 1];
            char m_punctuactionMark = t_lastWord[t_lastWord.Length - 1];

            AdjustWords();

            // Prepare container words
            for (int i = 0; i < m_sentence.Length; i++)
            {
                var word = Instantiate(m_wordContainerSlotPrefab, new Vector3(0, 5 + (i*3), 0), Quaternion.identity) as GameObject;
                m_myWordContainer.m_wordSlots.Add(word);
                m_myWordContainer.m_wordSlots[i].transform.SetParent(m_myWordContainer.transform, false);
                m_myWordContainer.m_wordSlots[i].GetComponentInChildren<WordObject>().SetupContainerSlot(i, m_sentence[i]);
            }

            // Shuffle container words
            for (int i = 0; i < m_myWordContainer.m_wordSlots.Count; i++)
            {
                int l_randomNumber;
                do
                {
                    l_randomNumber = UnityEngine.Random.Range(0, m_myWordContainer.m_wordSlots.Count);
                }
                while (l_randomNumber == i);

                //Swap words
                Vector3 t_tempWord;
                t_tempWord = m_myWordContainer.m_wordSlots[l_randomNumber].transform.position;
                m_myWordContainer.m_wordSlots[l_randomNumber].transform.position = m_myWordContainer.m_wordSlots[i].transform.position;
                m_myWordContainer.m_wordSlots[i].transform.position = t_tempWord;
            }

            // Prepare speech bubble slots
            for (int i = 0; i < m_sentence.Length+1; i++)
            {
                var word = Instantiate(m_bubbleSlotPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                m_myActiveSpeechBubble.m_wordSlots.Add(word);
                m_myActiveSpeechBubble.m_wordSlots[i].transform.SetParent(m_myActiveSpeechBubble.transform, false);
                m_myActiveSpeechBubble.m_wordSlots[i].GetComponent<WordObject>().SetupSpeechBubbleSlot(i);
                m_myActiveSpeechBubble.m_wordSlots[i].GetComponent<Rigidbody2D>().isKinematic = true;
                m_myActiveSpeechBubble.m_wordSlots[i].GetComponent<BoxCollider2D>().enabled = false;

                //Check if it's final slot in the bubble and set this one as punctuational mark.
                if (i == m_sentence.Length)
                {
                    m_myActiveSpeechBubble.m_wordSlots[i].GetComponent<WordObject>().SetText(m_punctuactionMark.ToString());
                    m_myActiveSpeechBubble.m_wordSlots[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                    m_myActiveSpeechBubble.m_wordSlots[i].GetComponentInChildren<TextMeshPro>().fontSize = 6.0f;
                }
            }

            StartCoroutine("SetContainerWordsKinematic");

            m_myActiveSpeechBubble.UpdateSlots();
        }


        private IEnumerator SetContainerWordsKinematic()
        {
            yield return new WaitForSeconds(4.0f);

            foreach ( GameObject slot in m_myWordContainer.m_wordSlots)
            {
                slot.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            m_gameReady = true;
        }

 
        void AdjustWords()
        {
            TrimPunctuations();
            for (int i = 0; i < m_sentence.Length; i++)
            {
                m_sentence[i] = ChangeToLowercase(m_sentence[i]);
            }
        }

        /// <summary>
        /// Trim all of the punctuations from the end of the words
        /// </summary>
        private void TrimPunctuations()
        {
            char[] listOfCharsToTrim = { ',', '.', '?', '!' };
            for (int i=0; i< m_sentence.Length; i++)
            {
                m_sentence[i] = m_sentence[i].TrimEnd(listOfCharsToTrim);
            }
            m_sentence[m_sentence.Length - 1] = m_sentence[m_sentence.Length - 1].TrimEnd(listOfCharsToTrim);
        }

        /// <summary>
        /// Apply exception to the adjusted words
        /// If there is a word mentioned in collection, this word will not be adjusted to lowercase when it's first in the phrase
        /// </summary>
        private string ChangeToLowercase(string _word)
        {
            string[] l_lowerCaseExcemptionWords = { "I", "I'm", "I'am" ,"George", "Harry" };
            bool isException = false;
            foreach (string word in l_lowerCaseExcemptionWords)
            {
                if (_word.Equals(word))
                {
                    isException = true;
                }
            }

            if (!isException)
            {
                return _word.ToLower();
            } else
            {
                return _word;
            }
        }

        /// <summary>
        /// Destroy all objects after sentence check.
        /// </summary>
        public void DestroyWordContainerSlots()
        {
            if (m_myWordContainer != null)
            {
                m_myWordContainer.DestroyList();
            }
        }

        /// <summary>
        /// Destroy all objects after sentence check.
        /// </summary>
        public void DestroySpeechBubbleSlots()
        {
            if (m_myActiveSpeechBubble != null)
            {
                m_myActiveSpeechBubble.DestroyList();
            }
        }

        public void ShowHint()
        {
            int l_hintWordIndex = m_myActiveSpeechBubble.GetActiveSlotIndex();
            m_myWordContainer.m_wordSlots[l_hintWordIndex].GetComponentInChildren<PulseEffect>().StartPulseEffect();
        }

        public void DisableHint()
        {
            int l_hintWordIndex = m_myActiveSpeechBubble.GetActiveSlotIndex();
            m_myWordContainer.m_wordSlots[l_hintWordIndex].GetComponentInChildren<PulseEffect>().StopPulseEffect();
        }

        /*
        public void ResetAttributes()
        {
            m_activeSlotNumber = 0;
        }
        */
    }
}