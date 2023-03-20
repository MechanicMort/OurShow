using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePlayer : MonoBehaviour
{
    AudioSource m_myAudioSource;
    [SerializeField] AudioClip[] m_correctAnswerCollection;

    int m_currentClipIndex;

    private void Awake()
    {
        m_myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayPhraseVoice()
    {
        //int l_randomCollection = Random.Range(0, m_correctAnswerCollection.Length);
        //m_myAudioSource.clip = m_correctAnswerCollection[l_randomCollection];
        m_myAudioSource.clip = m_correctAnswerCollection[m_currentClipIndex];
        m_myAudioSource.Play();
        m_currentClipIndex++;
    }

    public void SetCurrentClipIndex(int _clipIndex)
    {
        m_currentClipIndex = _clipIndex;
    }
}