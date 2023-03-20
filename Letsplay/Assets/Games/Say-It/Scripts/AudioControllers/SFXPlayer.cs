using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource m_myAudioSource;
    [SerializeField] AudioClip m_wrongSlot;
    [SerializeField] AudioClip m_correctSlotAudio;
    [SerializeField] AudioClip m_phraseCompleted;

    private void Awake()
    {
        m_myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayWrongSlotSound()
    {
        m_myAudioSource.clip = m_wrongSlot;
        m_myAudioSource.Play();
    }

    public void PlayCorrectSlotSound()
    {
        m_myAudioSource.clip = m_correctSlotAudio;
        m_myAudioSource.Play();
    }

    public void PlayPhraseCompletedSound()
    {
        m_myAudioSource.clip = m_phraseCompleted;
        m_myAudioSource.Play();
    }
}
