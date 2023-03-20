using System;
using System.Collections.Generic;
using UnityEngine;
using WPM.UI.Effects;

namespace WPM.Connect.Core
{
    public class WordSpawner : MonoBehaviour
    {
        [SerializeField] AudioCollectionSO[] m_myAudioWordCollection;
        [SerializeField] WordsCollectionSO[] m_myEnglishWordCollection;
        [SerializeField] WordsCollectionSO[] m_myNativeWordCollection;
        [SerializeField] LinesController m_myLinesController;

        private int m_nativeLanguageIndex = 1;

        [SerializeField] private GameObject m_wordPrefabLH;
        [SerializeField] private GameObject[] m_wordPrefabRH;

        [SerializeField] private Transform m_spawnPointRH;

        [SerializeField] public AudioClip[] m_myAudioCollection;
        public string[] m_myWordCollectionLH;
        public string[] m_myWordCollectionRH;

        public GameObject[] m_preparedWordsLH;
        public GameObject[] m_preparedWordsRH;

        List<GameObject> m_spawnedWordsLH;
        public List<GameObject> m_spawnedWordsRH;

        [SerializeField] private bool m_isSpawnerOn = false;

        [SerializeField] float m_spawnFrequencyLH = 4.0f;

        [SerializeField] float m_spawnFrequencyRH = 2.0f;
        [SerializeField] private float m_wordSpeedRH = 0.5f;

        float m_leftTimeRemaining = 0;
        float m_rightTimeRemaining = 0;

        int m_wordsGroupSize = 4;    

        int m_currentSpawnIndexLH = 0;
        int m_cycledWordIndex = 0;

        [SerializeField] int m_cycledCollectionSizeLH = 5;
        int m_addNextIndexLH = 0;

        [SerializeField] int m_cycledCollectionSize = 8;
        int m_addNextIndexRH = 0;

        public void SetNativeLanguageIndex(int _nativeIndex)
        {
            m_nativeLanguageIndex = _nativeIndex;
        }

        /// <summary>
        /// Reset all attributes before starting game
        /// </summary>
        public void Reset()
        {
            SetSpawnerActive(false);
            if (m_spawnedWordsLH != null && m_spawnedWordsRH != null)
            {
                foreach (GameObject o in m_spawnedWordsLH)
                {
                    Destroy(o);
                }

                foreach (GameObject o in m_spawnedWordsRH)
                {
                    Destroy(o);
                }

                foreach (GameObject o in m_preparedWordsLH)
                {
                    Destroy(o);
                }

                foreach (GameObject o in m_preparedWordsRH)
                {
                    Destroy(o);
                }

                Array.Clear(m_preparedWordsLH, 0, m_preparedWordsLH.Length);
                Array.Clear(m_preparedWordsRH, 0, m_preparedWordsRH.Length);

                m_spawnedWordsLH.Clear();
                m_spawnedWordsRH.Clear();
            }

            m_currentSpawnIndexLH = 0;
            m_cycledWordIndex = 0;
            m_addNextIndexLH = 0;
            m_addNextIndexRH = 0;
        }

        /// <summary>
        /// Prepare words for new game
        /// </summary>
        public void PrepareWordSpawner(int _currentEpisode, int _nativeLanguageIndex)
        {
            m_preparedWordsLH = new GameObject[m_myEnglishWordCollection[_currentEpisode].GetLanguageWords().Length];
            m_preparedWordsRH = new GameObject[m_myNativeWordCollection[m_nativeLanguageIndex].GetLanguageWords().Length];
            //m_preparedWordsRH = new GameObject[m_myNativeWordCollection[_currentEpisode].GetLanguageWords().Length];

            m_myAudioCollection = m_myAudioWordCollection[_currentEpisode].GetAudioWords();
            m_myWordCollectionLH = m_myEnglishWordCollection[_currentEpisode].GetLanguageWords();
            m_myWordCollectionRH = m_myNativeWordCollection[m_nativeLanguageIndex].GetLanguageWords();
            //m_myWordCollectionRH = m_myNativeWordCollection[_currentEpisode].GetLanguageWords();

            m_spawnedWordsLH = new List<GameObject>();
            m_spawnedWordsRH = new List<GameObject>();

            m_leftTimeRemaining = m_spawnFrequencyLH;
            m_rightTimeRemaining = m_spawnFrequencyRH;

            m_preparedWordsLH = PrepareAllWords(m_myWordCollectionLH, m_preparedWordsLH, true);
            m_preparedWordsRH = PrepareAllWords(m_myWordCollectionRH, m_preparedWordsRH, false);

            // Prepare objects for cycle
            while (m_addNextIndexRH < m_cycledCollectionSize)
            {
                m_preparedWordsRH[m_addNextIndexRH].transform.position = new Vector3(7.5f, 20 + m_addNextIndexRH, 0f);
                m_spawnedWordsRH.Add(m_preparedWordsRH[m_addNextIndexRH]);
                m_addNextIndexRH++;
            }

            // Prepare objects for cycle
            while (m_addNextIndexLH < m_cycledCollectionSizeLH)
            {
                m_preparedWordsLH[m_addNextIndexLH].transform.position = new Vector3(-7.5f, 20 + m_addNextIndexLH, 0f);
                m_spawnedWordsLH.Add(m_preparedWordsLH[m_addNextIndexLH]);
                m_addNextIndexLH++;
            }
        }

        void Update()
        {
            if (!m_isSpawnerOn) return;

            if (m_leftTimeRemaining > 0)
            {
                m_leftTimeRemaining -= Time.deltaTime;
            } else
            {
                SpawnWordLH();
                m_leftTimeRemaining = m_spawnFrequencyLH;
            }

            if (m_rightTimeRemaining > 0)
            {
                m_rightTimeRemaining -= Time.deltaTime;
            }
            else
            {
                SpawnWordRH();
                m_rightTimeRemaining = m_spawnFrequencyRH;
            }
        }

        /// <summary>
        /// Prepare all word objects before spawning them on the lines
        /// </summary>
        private GameObject[] PrepareAllWords(string[] _languageWords, GameObject[] _wordObjectList, bool _isLeftHand)
        {
            float Xpos = 2.5f;
            if (_isLeftHand)
            {
                Xpos = -2.5f;
            }

            GameObject prefab;
            if (_isLeftHand)
            {
                prefab = m_wordPrefabLH;
            }
            else
            {
                prefab = m_wordPrefabRH[m_nativeLanguageIndex];
            }

            // Create words instances
            for (int k = 0; k < m_myWordCollectionLH.Length; k++)
            {
                GameObject wordObject = Instantiate(prefab, new Vector3(Xpos, 20 + k, 20), Quaternion.identity);
                wordObject.SetActive(true);
                wordObject.GetComponent<Word>().wordID = k;
                wordObject.GetComponent<Word>().text = _languageWords[k];
                wordObject.GetComponent<Word>().audioClip = m_myAudioCollection[k]; // TODO work around this
                wordObject.GetComponent<Word>().UpdateWordText();
                wordObject.GetComponent<Word>().isLeftHand = _isLeftHand;
                wordObject.GetComponentInChildren<Animator>().enabled = false;
                wordObject.transform.SetParent(this.transform);
                _wordObjectList[k] = wordObject;
            }

            if (_isLeftHand)
            {            
                // Shuffle words
                int i = 0;
                int minRange = 0;
                int maxRange = m_wordsGroupSize;

                while (i < _wordObjectList.Length)
                {
                    for (int j = minRange; j < maxRange; j++)
                    {
                        int randomIndex = UnityEngine.Random.Range(minRange, maxRange - 1);
                        GameObject tempObject = _wordObjectList[j];
                        _wordObjectList[j] = _wordObjectList[randomIndex];
                        _wordObjectList[randomIndex] = tempObject;
                    }

                    i += m_wordsGroupSize;

                    minRange += m_wordsGroupSize;
                    maxRange += m_wordsGroupSize;
                }
            }
        
            return _wordObjectList;
        }

        /// <summary>
        /// Prepare extra word, when wrong answer is given.
        /// </summary>
        public void PrepareExtraWord(int _wordID, string _wordText, AudioClip _audioClip,bool _isLeftHand)
        {
            GameObject prefab;
            if (_isLeftHand)
            {
                prefab = m_wordPrefabLH;
            }
            else
            {
                prefab = m_wordPrefabRH[m_nativeLanguageIndex];
            }

            GameObject wordObject = Instantiate(prefab, new Vector3(12.5f, 20.0f, 20.0f), Quaternion.identity);
            wordObject.SetActive(true);
            wordObject.GetComponent<Word>().wordID = _wordID;
            wordObject.GetComponent<Word>().text = _wordText;
            wordObject.GetComponent<Word>().audioClip = _audioClip;
            wordObject.GetComponent<Word>().UpdateWordText();
            wordObject.GetComponent<Word>().isLeftHand = _isLeftHand;
            wordObject.GetComponent<Word>().isExtra = true;
            wordObject.GetComponent<Word>().GetComponentInChildren<Animator>().enabled = true;
            wordObject.GetComponentInChildren<SpriteRenderer>().color = Color.red; // TODO delete later
            wordObject.transform.SetParent(this.transform);

            if (_isLeftHand)
            {
                m_spawnedWordsLH.Add(wordObject);
            }
            else
            {
                m_spawnedWordsRH.Add(wordObject);
            }
        }

        /// <summary>
        /// Spawning word on the left hand side
        /// </summary>
        private void SpawnWordLH()
        {
            if (m_currentSpawnIndexLH == m_spawnedWordsLH.Count) return;

            int t_randomLineIndex = m_myLinesController.GetRandomLineIndex();

            // Spawn word
            Vector3 t_startingPosition = m_myLinesController.GetLinePosition(t_randomLineIndex);
            m_spawnedWordsLH[m_currentSpawnIndexLH].transform.position = t_startingPosition;

            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<Word>().line = t_randomLineIndex;
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<PopUpEffect>().MaximiseWindow();
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<Word>().UpdateWordText();
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<Word>().SetIsMoving(true);
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<Word>().speed  = m_myLinesController.GetLineSpeed(t_randomLineIndex);
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponent<Word>().AdjustAnimationSpeed();
            m_spawnedWordsLH[m_currentSpawnIndexLH].GetComponentInChildren<Animator>().enabled = true;
        
            // Add another word from collection if there is no more words in cycled collection
            if (m_addNextIndexLH < m_myWordCollectionLH.Length)
            {
                m_preparedWordsLH[m_addNextIndexLH].transform.position = new Vector3(-7.5f, 20 + m_addNextIndexLH, 0f);
                m_spawnedWordsLH.Add(m_preparedWordsLH[m_addNextIndexLH]);
                m_addNextIndexLH++;
            }

            m_currentSpawnIndexLH++;
        }

        /// <summary>
        /// Spawning word on the right hand side
        /// </summary>
        private void SpawnWordRH()
        {
            if (m_spawnedWordsRH[m_cycledWordIndex] != null)
            {
                Vector3 t_startingPosition = m_spawnPointRH.position;

                m_spawnedWordsRH[m_cycledWordIndex].transform.position = t_startingPosition;

                m_spawnedWordsRH[m_cycledWordIndex].GetComponent<PopUpEffect>().MaximiseWindow();
                m_spawnedWordsRH[m_cycledWordIndex].GetComponent<Word>().UpdateWordText();
                m_spawnedWordsRH[m_cycledWordIndex].GetComponent<Word>().SetIsMoving(true);
                m_spawnedWordsRH[m_cycledWordIndex].GetComponent<Word>().speed = m_wordSpeedRH;
                m_spawnedWordsRH[m_cycledWordIndex].GetComponent<Word>().AdjustAnimationSpeed();
                m_spawnedWordsRH[m_cycledWordIndex].GetComponentInChildren<Animator>().enabled = true;
            }

            m_cycledWordIndex++;
            if (m_cycledWordIndex == m_spawnedWordsRH.Count)
            {
                m_cycledWordIndex = 0;
            }
        }

        /// <summary>
        /// Function is called when there is space for another word in RH collection.
        /// Called when word is removed after correct answer.
        /// Called when word is removed after meating danger zone gap.
        /// </summary>
        public void AddWordToCycledRH(int _removedObjectIndex)
        {
            if (m_addNextIndexRH < m_preparedWordsRH.Length)
            {
                int m_cycledIndex = GetCycledIndex(_removedObjectIndex);
                m_spawnedWordsRH[m_cycledIndex] = m_preparedWordsRH[m_addNextIndexRH];

                m_preparedWordsRH[m_addNextIndexRH].transform.position = new Vector3(7.5f, 20 + m_addNextIndexRH, 0f);
                m_addNextIndexRH++;
            }
        }

        public int GetCycledIndex(int _id)
        {
            bool l_cycledFound = false;

            int l_cycleIndex = 0;
            for (int i = 0; i < m_spawnedWordsRH.Count; i++)
            {
                if (!l_cycledFound && m_spawnedWordsRH[i] != null && m_spawnedWordsRH[i].GetComponent<Word>().wordID == _id)
                {
                    l_cycleIndex = i;
                    l_cycledFound = true;
                }
            }
            return l_cycleIndex;
        }

        public void SetSpawnerActive(bool _isActive)
        {
            m_isSpawnerOn = _isActive;
        }

        public bool GetIsActive()
        {
            return m_isSpawnerOn;
        }

        public void IncreaseSpeed()
        {
            if (m_spawnFrequencyLH > 2.0f)
            {
                m_spawnFrequencyLH -= 1.0f;
            }

            m_myLinesController.IncreaseLinesSpeed();

            foreach (GameObject worm in m_spawnedWordsLH)
            {
                if (worm != null)
                {
                    worm.GetComponent<Word>().speed += 0.25f;
                    worm.GetComponent<Word>().AdjustAnimationSpeed();
                }
            }
        }
    }
}