using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.Connect.Core
{
    public class LinesController : MonoBehaviour
    {
        private Line[] m_lines;

        [SerializeField] int m_numberOfLines = 7;

        [SerializeField] float m_topLineOffset = -4.4f;
        [SerializeField] float m_spawnPointXPos = -7.0f;
        [SerializeField] float m_spaceBetweenLines = 1.25f;

        [SerializeField] private float m_baseLineSpeed = 0.25f;
        [SerializeField] private float m_minRangeModifier = 0.5f;
        [SerializeField] private float m_maxRangeModifier = 2.5f;

        int[] m_previousLinesIndex;
        int m_oldestIndex = 0;    

        private void Awake()
        {
            m_lines = new Line[m_numberOfLines];
            m_previousLinesIndex = new int[4];

            for (int i=0; i<m_lines.Length; i++)
            {
                m_lines[i].initialSpeed = GetRandomSpeed();
                m_lines[i].currentSpeed = m_lines[i].initialSpeed;
                m_lines[i].spawnPosition = new Vector3(m_spawnPointXPos, m_topLineOffset + (m_spaceBetweenLines * i), 0.0f);
            }
        }

        /// <summary>
        /// Return position for the new line. Algorithm is doing check to discard results for few previous lines that's been used.
        /// </summary>
        public int GetRandomLineIndex()
        {
            int l_newLineIndex = 0;
            bool isCorrectLineIndex;
            do
            {
                isCorrectLineIndex = true;
                l_newLineIndex = Random.Range(0, m_lines.Length);
                foreach (int index in m_previousLinesIndex)
                {
                    if (l_newLineIndex == index)
                    {
                        isCorrectLineIndex = false;
                    }
                }

            } while (!isCorrectLineIndex);

            m_previousLinesIndex[m_oldestIndex] = l_newLineIndex;
            m_oldestIndex++;
            if (m_oldestIndex == m_previousLinesIndex.Length)
            {
                m_oldestIndex = 0;
            }

            return l_newLineIndex;
        }

        /// <summary>
        /// Return line position for passed index
        /// </summary>
        public Vector3 GetLinePosition(int _lineIndex)
        {
            return m_lines[_lineIndex].spawnPosition;
        }

        /// <summary>
        /// Return line speed for passed index
        /// </summary>
        public float GetLineSpeed(int _lineIndex)
        {
            return m_lines[_lineIndex].currentSpeed;
        }

        /// <summary>
        /// Return random speed for the single line.
        /// </summary>
        private float GetRandomSpeed()
        {
            float t_randomSpeed = Random.Range(m_baseLineSpeed * m_minRangeModifier, m_baseLineSpeed * m_maxRangeModifier);
            return t_randomSpeed;
        }

        /// <summary>
        /// Increase speed for all lines byt 0.25.
        /// </summary>
        public void IncreaseLinesSpeed()
        {
            for (int i=0; i<m_lines.Length; i++)
            {
                m_lines[i].currentSpeed += 0.25f;
            }
        }
    }
}