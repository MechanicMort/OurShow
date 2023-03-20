using System.Collections.Generic;
using UnityEngine;

namespace WPM.SayIt.Core
{
    public class WordPreparer : MonoBehaviour
    {
        [SerializeField] SentenceCollectionSO[] m_sentenceCollection;
        public List<PhraseObject> m_phraseCollection = new List<PhraseObject>();

        public void PrepareSentence(int _currentEpisode)
        {
            for (int i=0; i< m_sentenceCollection[_currentEpisode].GetSentenceCount(); i++)
            {
                PhraseObject levelWord = new PhraseObject(m_sentenceCollection[_currentEpisode].GetCharacterName(i), m_sentenceCollection[_currentEpisode].GetSentence(i));
                m_phraseCollection.Add(levelWord);
            }
        }

        public int[] GetImagesSwitcher(int _currentEpisode)
        {
            return m_sentenceCollection[_currentEpisode].m_imageSwitchNumber;
        }

        public int GetNumberOfPhrases(int _currentEpisode)
        {
            return m_sentenceCollection[_currentEpisode].GetSentenceCount();
        }

        public void Reset()
        {
            m_phraseCollection.Clear();
        }
    }
}