using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.SayIt.Core
{
    public class SpeechBubbleTail : MonoBehaviour
    {

        [SerializeField] Transform[] m_startPosition;
        [SerializeField] Transform[] m_endPosition;

        private Vector3 InitialScale;

        private void Start()
        {
            InitialScale = transform.localScale;
        }

        public void UpdateTailForCharacter(int _characterNumber)
        {
            float distance = Vector3.Distance(m_startPosition[_characterNumber].transform.position, m_endPosition[_characterNumber].transform.position);

            transform.localScale = new Vector3(InitialScale.x, distance/10f, InitialScale.z);

            Vector3 middlePoint = (m_startPosition[_characterNumber].transform.position + m_endPosition[_characterNumber].transform.position)/2;
            transform.position = middlePoint;

            Vector3 rotationDirection = (m_endPosition[_characterNumber].transform.position - m_startPosition[_characterNumber].transform.position);

            transform.rotation = Quaternion.FromToRotation(Vector3.up, m_startPosition[_characterNumber].transform.position - m_endPosition[_characterNumber].transform.position);
        }
    }
}
