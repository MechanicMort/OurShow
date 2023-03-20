using System.Collections;
using UnityEngine;
using WPM.SayIt.Core;

public class GameFlowController : MonoBehaviour
{
    private AudioSource m_myAudioSource;
    private WordContainer m_myWordContainer;
    private MenuController m_myMenuController;
    private Coiner m_myCoiner;

    [SerializeField] private AudioClip m_currentWordClip;
    [SerializeField] private AudioClip m_collectSFX;
    [SerializeField] private AudioClip m_loseGameSFX;

    int m_currentCoins = 0;

    int m_letterCoins = 1;
    int m_wordCoins = 5;
    int m_bonusCoins = 3;

    private void Awake()
    {
        m_myWordContainer = FindObjectOfType<WordContainer>();
        m_myMenuController = FindObjectOfType<MenuController>();
        m_myCoiner = FindObjectOfType<Coiner>();
        m_myAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_currentCoins = GameData.GetTotalCoins();
    }

    public void WordCollected(char _letter)
    {
        if (m_myWordContainer.CheckLetter(_letter))
        {
            if (m_myWordContainer.isWordComplete())
            {
                m_myCoiner.AddCoins(m_wordCoins);
                m_currentCoins += m_wordCoins;
                GameData.SetTotalCoins(m_currentCoins);
                StartCoroutine("OpenWinScreen");
            } else
            {
                m_myCoiner.AddCoins(m_letterCoins);
                m_currentCoins += m_letterCoins;
                PlayCollectSound();
            }
        }
        else
        {
            StartCoroutine("OpenLoseScreen");
            PlayLoseSound();
        }
    }

    public void BonusCollected()
    {
        m_myCoiner.AddCoins(m_bonusCoins);
        m_currentCoins += m_bonusCoins;
        PlayCollectSound();
    }

    private IEnumerator OpenWinScreen()
    {
        yield return new WaitForSeconds(0.5f);
        PlayWord();
        yield return new WaitForSeconds(2.8f);
        m_myMenuController.OpenWinScreen();
        yield return null;
    }

    private IEnumerator OpenLoseScreen()
    {
        yield return new WaitForSeconds(1.5f);
        m_myMenuController.OpenLoseScreen();
        yield return null;
    }

    private void PlayWord()
    {
        m_myAudioSource.clip = m_currentWordClip;
        m_myAudioSource.Play();
    }

    private void PlayCollectSound()
    {
        m_myAudioSource.clip = m_collectSFX;
        m_myAudioSource.Play();
    }

    private void PlayLoseSound()
    {
        m_myAudioSource.clip = m_loseGameSFX;
        m_myAudioSource.Play();
    }
}