using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource m_myAudioSource;
    [SerializeField] AudioClip[] m_correctAnswerAudios; 

    private void Awake()
    {
        m_myAudioSource = this.GetComponent<AudioSource>();
    }

    public void PlayCorrectAnswer()
    {
        StartCoroutine("PlaySound");
    }

    IEnumerator PlaySound()
    {
        int l_firstClipIndex = Random.Range(0, m_correctAnswerAudios.Length);

        int l_secondClipIndex = 0 + l_firstClipIndex;

        m_myAudioSource.clip = m_correctAnswerAudios[l_firstClipIndex];
        m_myAudioSource.Play();
        yield return new WaitForSeconds(m_myAudioSource.clip.length);
        m_myAudioSource.clip = m_correctAnswerAudios[l_secondClipIndex];
        m_myAudioSource.Play();
    }
}