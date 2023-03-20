using UnityEngine;

namespace WPM.Connect.Core
{
    public class SFXController : MonoBehaviour
    {
        AudioSource m_myAudioSource;

        [SerializeField] AudioClip m_correctAnswerClip;
        [SerializeField] AudioClip m_wrongAnswerClip;
        [SerializeField] AudioClip[] m_bugClickClip;

        private void Awake()
        {
            m_myAudioSource = this.GetComponent<AudioSource>();
        }

        public void PlayCorrectAnswer()
        {
            m_myAudioSource.PlayOneShot(m_correctAnswerClip);
        }

        public void PlayWrongAnswer()
        {
            m_myAudioSource.PlayOneShot(m_wrongAnswerClip);
        }

        public void PlayWordClick()
        {
            m_myAudioSource.PlayOneShot(m_bugClickClip[Random.Range(0, m_bugClickClip.Length)]);
        }
    }
}