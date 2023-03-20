using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    private Vector3 m_initialScale;
    private bool m_isActive = false;

    private bool m_isGrowing = true;

    [SerializeField] private float m_pulseFrequency = 0.4f;
    [SerializeField] private float m_pusleTopLimit = 1.2f;
    [SerializeField] private float m_pusleBotLimit = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        CallPulseEffect();
    }

    private void CallPulseEffect()
    {
        if (m_isActive)
        {
            if (m_isGrowing)
            {
                transform.localScale += new Vector3(m_pulseFrequency, m_pulseFrequency, m_pulseFrequency) * Time.deltaTime;
                if (transform.localScale.x! > m_pusleTopLimit)
                {
                    m_isGrowing = false;
                }
            }
            else
            {
                transform.localScale -= new Vector3(m_pulseFrequency, m_pulseFrequency, m_pulseFrequency) * Time.deltaTime;
                if (transform.localScale.x! < m_pusleBotLimit)
                {
                    m_isGrowing = true;
                }
            }
        }
    }

    public void StartPulseEffect()
    {
        m_isActive = true;
    }

    public void StopPulseEffect()
    {
        m_isActive = false;
        transform.localScale = m_initialScale;
    }
}
