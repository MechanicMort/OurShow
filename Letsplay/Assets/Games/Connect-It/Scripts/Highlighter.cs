using UnityEngine;

namespace WPM.Connect.Core
{
    public class Highlighter : MonoBehaviour
    {
        SpriteRenderer m_mySpriteRenderer;
        Color m_baseColour;

        Color m_currentColour;
        [SerializeField] Color m_pickedColour;
        Color m_correctColour;
        Color m_wrongColour;


        [SerializeField] float pulsingFrequency = 0.5f;
        [SerializeField] float maxBrightest = 0.8f;
        [SerializeField] float minBrightest = 0.3f;

        bool isGettingBright = true;

        private void Awake()
        {
            m_mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            m_baseColour = m_mySpriteRenderer.color;
            SetBase();
            m_pickedColour = Color.white;
            m_correctColour = Color.green;
            m_wrongColour = Color.red;
        }

        private void Update()
        {
            if (isGettingBright)
            {
                m_mySpriteRenderer.color = new Color(m_currentColour.r, m_currentColour.g, m_currentColour.b, m_mySpriteRenderer.color.a + (Time.deltaTime * pulsingFrequency));
                if (m_mySpriteRenderer.color.a > maxBrightest)
                {
                    isGettingBright = false;
                }

            } else
            {
                m_mySpriteRenderer.color = new Color(m_currentColour.r, m_currentColour.g, m_currentColour.b, m_mySpriteRenderer.color.a - (Time.deltaTime * pulsingFrequency));
                if (m_mySpriteRenderer.color.a < minBrightest)
                {
                    isGettingBright = true;
                }
            }
        }

        public void SetBase()
        {
            m_currentColour = m_baseColour;
            maxBrightest = 0.5f;
            minBrightest = 0.2f;
        }

        public void SetPicked()
        {
            m_currentColour = m_pickedColour;
            maxBrightest = 1.0f;
            minBrightest = 0.6f;
        }

        public void SetCorrect()
        {
            m_currentColour = m_correctColour;
            maxBrightest = 1.0f;
            minBrightest = 0.6f;
        }

        public void SetWrong()
        {
            m_currentColour = m_wrongColour;
            maxBrightest = 1.0f;
            minBrightest = 0.6f;
        }
    }
}