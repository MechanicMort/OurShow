using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WPM.SayIt.Core
{
    public class SpeechBubble : MonoBehaviour
    {
        SpriteRenderer m_myBackgroundSprite;

        public List<GameObject> m_wordSlots;
        private Vector2 m_bubbleSize;

        [SerializeField] float m_speechBubblePadding = 1.0f;
        [SerializeField] float m_spaceBetweenWords = 0.3f;
        [SerializeField] private float m_phraseOffset = 0.4f;
        private float[] m_singleWordOffset;
        private float m_phraseMiddlePointOffset;
        private float[] m_slotWidth;
        private float m_phraseWidth = 0.0f;

        private int m_activeSlotIndex = 0;

        [SerializeField] private UnityEvent m_phraseComplete;

        private void Awake()
        {
            m_myBackgroundSprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            CalculatePhrasePosition();
            UpdatePhrasePosition();
            CalculateSpeechBubbleSize();
            UpdateSpeechBubbleSize();
        }

        public void Reset()
        {
            m_activeSlotIndex = 0;
        }

        /// <summary>
        /// Calculate width of the speech bubble
        /// </summary>
        public void CalculateSpeechBubbleSize()
        {
            m_bubbleSize.x = 0;

            foreach (Transform wordElement in transform)
            {
                if (wordElement.GetComponent<BoxTextFitter>() == null) return;

                BoxTextFitter childTransform = wordElement.GetComponent<BoxTextFitter>();
                m_bubbleSize.x += childTransform.m_textBoxSize.x + m_spaceBetweenWords;
            }
        }

        /// <summary>
        /// Update speech bubble width with every frame
        /// </summary>
        public void UpdateSpeechBubbleSize()
        {
            if (m_wordSlots.Count == 0)
            {
                m_myBackgroundSprite.size = new Vector2(m_bubbleSize.x, m_myBackgroundSprite.size.y);
            }
            else
            {
                m_myBackgroundSprite.size = new Vector2(m_bubbleSize.x + m_speechBubblePadding, m_myBackgroundSprite.size.y);
            }
        }

        /// <summary>
        /// Set default position for each word in the phrase
        /// </summary>
        private void CalculatePhrasePosition()
        {
            m_phraseWidth = 0;
            m_singleWordOffset = new float[m_wordSlots.Count];
            m_slotWidth = new float[m_wordSlots.Count];
            for (int i = 0; i < m_wordSlots.Count; i++)
            {
                m_slotWidth[i] = m_wordSlots[i].GetComponent<BoxTextFitter>().m_textBoxSize.x;
                m_phraseWidth += m_slotWidth[i] + m_spaceBetweenWords;
                float l_prevWordsWidth = 0;

                for (int j = 0; j < i; j++)
                {
                    l_prevWordsWidth += m_slotWidth[j] + m_spaceBetweenWords;
                }

                //Check if it's final slot. If it's final slot it's punctuational mark, and should be moved closer to the last word in the sentence.
                if (i == m_wordSlots.Count-1)
                {
                    m_singleWordOffset[i] = l_prevWordsWidth;
                } else
                {
                    m_singleWordOffset[i] = (m_slotWidth[i] / 2) + l_prevWordsWidth;
                }
            }
            m_phraseWidth -= m_spaceBetweenWords;

            // Move the entire phrase to the middle of the speech bubble
            m_phraseMiddlePointOffset = m_phraseWidth / 2;
        }

        /// <summary>
        /// Update position for each word with every frame
        /// </summary>
        private void UpdatePhrasePosition()
        {
            for (int i = 0; i < m_wordSlots.Count; i++)
            {
                float t_slotPositionX = 0.0f;
                t_slotPositionX = m_singleWordOffset[i] * transform.localScale.x;
                t_slotPositionX -= m_phraseMiddlePointOffset * transform.localScale.x;
                t_slotPositionX += m_phraseOffset * transform.localScale.x;

                t_slotPositionX += transform.position.x; //Fit position to parent

                m_wordSlots[i].transform.position = new Vector3(t_slotPositionX, m_wordSlots[i].transform.position.y, m_wordSlots[i].transform.position.z);
            }
        }

        /// <summary>
        /// Update currently active slot
        /// Return true if the slot is updated.
        /// Return false when there is no more slots to update and the phrase is complete
        /// </summary>
        public bool UpdateSlots()
        {
            // Check if last word was placed. Total number of slots was decreased by one due to fact that the last one in punctuational mark
            if (m_activeSlotIndex < m_wordSlots.Count-1)
            {
                // Disable pulse effect for previous slot
                if (!(m_activeSlotIndex < 1))
                {
                    m_wordSlots[m_activeSlotIndex - 1].GetComponent<BoxCollider2D>().enabled = false;
                    m_wordSlots[m_activeSlotIndex - 1].GetComponentInChildren<PulseEffect>().StopPulseEffect();
                }

                // Start pulse effect for current slot
                if (!(m_activeSlotIndex < 0))
                {
                    m_wordSlots[m_activeSlotIndex].GetComponent<BoxCollider2D>().enabled = true;
                    m_wordSlots[m_activeSlotIndex].GetComponentInChildren<PulseEffect>().StartPulseEffect();
                }
                m_activeSlotIndex++;
                return true;
            }
            else
            {
                // Stop pulse effect when there is no more slots
                m_wordSlots[m_activeSlotIndex - 1].GetComponent<BoxCollider2D>().enabled = false;
                m_wordSlots[m_activeSlotIndex - 1].GetComponentInChildren<PulseEffect>().StopPulseEffect();

                m_activeSlotIndex = 0;
                m_phraseComplete.Invoke();
                return false;
                // TODO phrase complete
            }
        }

        /// <summary>
        /// Return index for current word/slot
        /// </summary>
        public int GetActiveSlotIndex()
        {
            return m_activeSlotIndex - 1;
        }

        public void DestroyList()
        {        
            if (m_wordSlots != null)
            {
                foreach (GameObject go in m_wordSlots)
                {
                    Destroy(go);
                }
            }

            m_wordSlots.Clear();        
        }
    }
}
