using System.Collections;
using UnityEngine;
using WPM.UI.Effects;
using UnityEngine.Events;

namespace WPM.SayIt.Core
{
    public class SpeechBubbleController : MonoBehaviour
    {
        [SerializeField] GameObject[] m_speechBubble;
        [SerializeField] GameObject m_speechBubbleTail;

        Vector3 m_activeBubblePosition;
        Vector3 m_incavtiveBubblePosition;

        int m_activeIndex = 0;
        int m_inactiveIndex = 1;

        [SerializeField] UnityEvent m_mainBubbleIsInactive;

        [SerializeField] float m_bubbleSpeechTime = 2.0f;

        void Start()
        {
            m_speechBubbleTail.GetComponent<SpriteRenderer>().enabled = false;
            m_activeBubblePosition = m_speechBubble[0].transform.position;
            m_incavtiveBubblePosition = m_speechBubble[1].transform.position;

            //m_speechBubble[1].GetComponent<SpriteRenderer>().enabled = false;
        }

        public void Reset()
        {
            m_speechBubble[m_activeIndex].GetComponent<SpeechBubble>().Reset();
            m_speechBubbleTail.GetComponent<SpriteRenderer>().enabled = false;
            m_activeIndex = 0;
            m_inactiveIndex = 1;
            m_activeBubblePosition = m_speechBubble[0].transform.position;
            m_incavtiveBubblePosition = m_speechBubble[1].transform.position;
        }

        public void DelayedMoveActiveBubble()
        {
            StartCoroutine("MoveActiveBubble");
        }

        public IEnumerator MoveActiveBubble()
        {
            yield return new WaitForSeconds(m_bubbleSpeechTime);
            m_speechBubbleTail.GetComponent<SpriteRenderer>().enabled = false;
            m_speechBubble[m_inactiveIndex].GetComponent<PopUpEffect>().MinimiseWindowSpriteDisabled();
            StartCoroutine("ChangeBubbleToInactive");
        }

        IEnumerator ChangeBubbleToInactive()
        {
            while (0.1f < Vector3.Distance(m_speechBubble[m_activeIndex].transform.position, m_incavtiveBubblePosition))
            {
                m_speechBubble[m_activeIndex].transform.position = Vector3.Lerp(m_speechBubble[m_activeIndex].transform.position, m_incavtiveBubblePosition, 0.05f);

                m_speechBubble[m_activeIndex].transform.localScale -= new Vector3(0.005f,0.005f,0.005f);
                yield return null;
            }

            m_mainBubbleIsInactive.Invoke();
        }

        public void ChangeBubbleToActive()
        {
            m_speechBubble[m_inactiveIndex].GetComponent<SpeechBubble>().DestroyList();
            int tempIndex = m_activeIndex;
            m_activeIndex = m_inactiveIndex;
            m_inactiveIndex = tempIndex;

            StartCoroutine("MoveBubbleToFirstPosition");
        }

        IEnumerator MoveBubbleToFirstPosition()
        {
            yield return new WaitForSeconds(1.0f);
            m_speechBubble[m_activeIndex].transform.position = m_activeBubblePosition;
            m_speechBubble[m_activeIndex].GetComponent<PopUpEffect>().MaximiseWindow();
            m_speechBubbleTail.GetComponent<SpriteRenderer>().enabled = true;
        }

        public GameObject GetActiveSpeechBubble()
        {
            return m_speechBubble[m_activeIndex];
        }
    }
}