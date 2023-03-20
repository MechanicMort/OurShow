using System.Collections;
using UnityEngine;
using TMPro;

namespace WPM.Connect.Core
{
    public class Fader : MonoBehaviour
    {
        TextMeshPro m_myTextMeshPro;

        Color m_textColor;

        [SerializeField] float m_textSpeed = 0.5f;
        [SerializeField] float m_timeToFade = 1.0f;
        [SerializeField] float m_fadeSpeed = 0.5f;

        [SerializeField] bool m_destroyAfterFade = true;

        void Start()
        {
            m_myTextMeshPro = GetComponent<TextMeshPro>();
            m_textColor = m_myTextMeshPro.color;
            StartCoroutine(FadeTextToZeroAlpha());

            if (m_destroyAfterFade)
            {
                Destroy(this.gameObject, m_timeToFade + (2.0f - m_fadeSpeed));
            }
        }

        void Update()
        {
            transform.position += new Vector3(0, m_textSpeed * Time.deltaTime, 0);
        }

        IEnumerator FadeTextToFullAlpha()
        {
            yield return new WaitForSeconds(2.0f);

            m_textColor = new Color(m_textColor.r, m_textColor.g, m_textColor.b, 0);
            while (m_textColor.a < 1.0f)
            {
                m_textColor = new Color(m_textColor.r, m_textColor.g, m_textColor.b, m_textColor.a + (Time.deltaTime / m_fadeSpeed));
                yield return null;
            }
        }
        
        public IEnumerator FadeTextToZeroAlpha()
        {
            yield return new WaitForSeconds(m_timeToFade);

            m_textColor = new Color(m_textColor.r, m_textColor.g, m_textColor.b, 255);
            while (m_textColor.a > 0.0f)
            {
                float modifier = Time.deltaTime * m_fadeSpeed;
                m_myTextMeshPro.color = new Color(m_myTextMeshPro.color.r, m_myTextMeshPro.color.g, m_myTextMeshPro.color.b, m_myTextMeshPro.color.a - modifier);
                yield return null;
            }        
        }
    }
}