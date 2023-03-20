using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using WPM.UI.Effects;

namespace WPM.Connect.Core
{
    public class ObjectClicker : MonoBehaviour
    {
        [SerializeField] WordSpawner m_myWordSpawner;
        [SerializeField] AudioSource m_myAudioSource;

        bool m_isWordPicked = false;

        GameObject m_firstWordObject;
        GameObject m_secondWordObject;

        GameObject m_leftHandWordObject;
        GameObject m_rightHandWordObject;

        [SerializeField] UnityEvent m_wordClicked;
        [SerializeField] UnityEvent m_correctAnswer;
        [SerializeField] UnityEvent m_wrongAnswer;

        int m_extraWordsCount = 1;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickWord();
            }
        }

        /// <summary>
        /// Function determines what should be done when object is clicked
        /// </summary>
        private void PickWord()
        {
            RaycastHit2D l_draggedObjectHit;


            if (l_draggedObjectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.TransformDirection(Vector3.forward), Mathf.Infinity))
            {
                // Check if clicked object is word
                if (l_draggedObjectHit.collider.gameObject.tag == "Word")
                {
                    if (!l_draggedObjectHit.collider.gameObject.GetComponent<Word>().isClickable) return;

                    m_wordClicked.Invoke();

                    m_firstWordObject = m_secondWordObject;
                    m_secondWordObject = l_draggedObjectHit.collider.gameObject;                

                    // Check if another word is already picked
                    if (m_isWordPicked)
                    {
                        // Check if this same word was clicked second time
                        if (m_firstWordObject.Equals(m_secondWordObject))
                        {
                            UpdatePickedWord();
                        } else
                        {
                            // Check what kind of collection the word is
                            if (m_firstWordObject != null && m_firstWordObject.GetComponent<Word>().isLeftHand)
                            {
                                // if this same collection return
                                if (m_secondWordObject.GetComponent<Word>().isLeftHand)
                                {
                                    UpdatePickedWord();
                                } else
                                {
                                    SetWords();
                                    CheckWords();
                                }
                            } else
                            {
                                // if this same collection return
                                if (!m_secondWordObject.GetComponent<Word>().isLeftHand)
                                {
                                    UpdatePickedWord();
                                } else
                                {
                                    SetWords();
                                    CheckWords();
                                }
                            }
                        }
                    }
                    else
                    {
                        UpdatePickedWord();
                    }
                } else
                {
                    // if second object is not word
                    ResetWordsColour();
                    ResetWords();
                }
            } else
            {
                // if there is no object for raycast
                ResetWordsColour();
                ResetWords();
            }
        }

        private void SetWords()
        {
            if (m_secondWordObject.GetComponent<Word>().isLeftHand)
            {
                m_leftHandWordObject = m_secondWordObject;
                m_rightHandWordObject = m_firstWordObject;
            }
            else
            {
                m_leftHandWordObject = m_firstWordObject;
                m_rightHandWordObject = m_secondWordObject;
            }
        }

        /// <summary>
        /// Check if two picked words has this same meaning
        /// </summary>
        private void CheckWords()
        {
            if (IsSameID(m_firstWordObject.GetComponent<Word>(), m_secondWordObject.GetComponent<Word>()))
            {
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }

        /// <summary>
        /// Call this function if two words has this same meaning
        /// </summary>
        private void CorrectAnswer()
        {
            m_firstWordObject.GetComponentInChildren<Highlighter>().SetCorrect();
            m_secondWordObject.GetComponentInChildren<Highlighter>().SetCorrect();

            //m_leftHandWord.GetComponentInChildren<Animator>().SetBool("isCorrectAnswer", true);
            m_rightHandWordObject.GetComponentInChildren<Animator>().SetBool("isCorrectAnswer", true);

            //m_leftHandWord.GetComponent<AudioPlayer>().PlayCorrectAnswer();
            m_rightHandWordObject.GetComponentInChildren<AudioPlayer>().PlayCorrectAnswer();

            m_firstWordObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            m_secondWordObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            /* TODO set isClickable to false
            m_firstWordObject.GetComponent<BoxCollider2D>().enabled = false;
            m_secondWordObject.GetComponent<BoxCollider2D>().enabled = false;
            */
            m_firstWordObject.GetComponent<Word>().isClickable = false;
            m_secondWordObject.GetComponent<Word>().isClickable = false;

            m_firstWordObject.GetComponent<Word>().BounceObject();
            m_secondWordObject.GetComponent<Word>().BounceObject();

            m_myAudioSource.PlayOneShot(m_firstWordObject.GetComponent<Word>().audioClip);

            m_correctAnswer.Invoke();

            /*
            GameObject m_wordToPass;
            if (m_firstWordObject.GetComponent<Word>().isLeftHand)
            {
                m_wordToPass = m_firstWordObject;
            }
            else
            {
                m_wordToPass = m_secondWordObject;
            }

            m_myWordSpawner.AddWordToCycledRH(m_wordToPass.GetComponent<Word>().GetWordID());
            */

            m_myWordSpawner.AddWordToCycledRH(m_leftHandWordObject.GetComponent<Word>().wordID);

            ResetWords();
        }    

        /// <summary>
        /// Call this function if two words has not this same meaning
        /// </summary>
        private void WrongAnswer()
        {
            StartCoroutine("SetHighlightToRed");

            for (int i=0; i< m_extraWordsCount; i++)
            {
                GenerateExtraWords();
            }

            ResetWords();
            m_wrongAnswer.Invoke();
        }

        IEnumerator SetHighlightToRed()
        {
            GameObject firstWrongWord = m_firstWordObject;
            GameObject secondWrongWord = m_secondWordObject;
            firstWrongWord.GetComponentInChildren<Highlighter>().SetWrong();
            secondWrongWord.GetComponentInChildren<Highlighter>().SetWrong();
            yield return new WaitForSeconds(2);

            if (firstWrongWord == null || secondWrongWord == null)
            {
                yield break; // if for some reason object is destroyed, end function.
            }
            else {
                firstWrongWord.GetComponentInChildren<Highlighter>().SetBase();
                secondWrongWord.GetComponentInChildren<Highlighter>().SetBase();
            }
        }

        private void GenerateExtraWords()
        {
            if (m_firstWordObject.GetComponent<Word>().isLeftHand)
            {
                int t_wordID = m_firstWordObject.GetComponent<Word>().wordID;
                AudioClip t_audioClip = m_myWordSpawner.m_myAudioCollection[t_wordID];
                string t_textLH = m_myWordSpawner.m_myWordCollectionLH[t_wordID];
                string t_textRH = m_myWordSpawner.m_myWordCollectionRH[t_wordID];

                m_myWordSpawner.PrepareExtraWord(t_wordID, t_textLH, t_audioClip, true);
                m_myWordSpawner.PrepareExtraWord(t_wordID, t_textRH, t_audioClip, false);
            }
            else
            {
                int t_wordID = m_secondWordObject.GetComponent<Word>().wordID;
                AudioClip t_audioClip = m_myWordSpawner.m_myAudioCollection[t_wordID];
                string t_textLH = m_myWordSpawner.m_myWordCollectionLH[t_wordID];
                string t_textRH = m_myWordSpawner.m_myWordCollectionRH[t_wordID];

                m_myWordSpawner.PrepareExtraWord(t_wordID, t_textLH, t_audioClip, true);
                m_myWordSpawner.PrepareExtraWord(t_wordID, t_textRH, t_audioClip, false);
            }
        }

        private void UpdatePickedWord()
        {
            m_secondWordObject.GetComponent<PopUpEffect>().MaximiseWindow();
            if (m_firstWordObject != null) m_firstWordObject.GetComponentInChildren<Highlighter>().SetBase();
            m_secondWordObject.GetComponentInChildren<Highlighter>().SetPicked();   
            m_isWordPicked = true;
        }

        private void ResetWords()
        {
            m_isWordPicked = false;
        
            m_firstWordObject = null;
            m_secondWordObject = null;
        }

        private void ResetWordsColour()
        {
            if (m_firstWordObject != null) m_firstWordObject.GetComponentInChildren<Highlighter>().SetBase();
            if (m_secondWordObject != null) m_secondWordObject.GetComponentInChildren<Highlighter>().SetBase();
        }

        private bool IsSameID(Word _firstWord, Word _secondWord)
        {
            if (_firstWord.wordID == _secondWord.wordID)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}