using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WPM.SayIt.Core
{
    public class WordContainer : MonoBehaviour
    {
        [SerializeField] GameObject m_blocker;

        public List<GameObject> m_wordSlots;

        SpriteRenderer m_mySpriteRenderer;
        [SerializeField] UnityEvent m_endOfFallingWordAnimation;

        float m_heightModifier = 0;
        float m_wordBiggestWidth = 0;

        [SerializeField] float m_widthPadding = 0.5f;
        [SerializeField] float m_heightPadding = 0.5f;

        private void Awake()
        {
            m_mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            FitContainerSize();
        }

        private void FitContainerSize()
        {
            m_heightModifier = 0;
            m_wordBiggestWidth = 0;

            // Determine the word with the biggest width 
            foreach (Transform child in transform)
            {
                m_heightModifier += child.transform.GetComponentInChildren<BoxCollider2D>().size.y;

                if (child.transform.GetComponentInChildren<BoxCollider2D>().size.x > m_wordBiggestWidth)
                {
                    m_wordBiggestWidth = child.transform.GetComponentInChildren<BoxCollider2D>().size.x;
                }            
            }

            if (m_wordSlots.Count == 0)
            {
                m_mySpriteRenderer.size = new Vector2(0, 0);
            } else
            {
                // Set minimum size for the word container
                if (m_wordBiggestWidth > 2.0f)
                {
                    m_mySpriteRenderer.size = new Vector2(m_wordBiggestWidth + m_widthPadding, m_heightModifier + m_heightPadding);
                } else
                {
                    m_mySpriteRenderer.size = new Vector2(2.0f + m_widthPadding, m_heightModifier + m_heightPadding);
                }
            }

            AdjustWordBlockerPosition();
        }

        private void AdjustWordBlockerPosition()
        {
            m_blocker.transform.localPosition = new Vector3(0, -0.5f - (0.50f * m_heightModifier), 0);
        }

        /// <summary>
        /// Destroy all objects after sentence check.
        /// </summary>
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