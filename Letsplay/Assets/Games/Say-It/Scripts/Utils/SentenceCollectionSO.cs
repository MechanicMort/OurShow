using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Harry,
    George
}

[CreateAssetMenu(fileName = "SentenceCollectionEpisode_", menuName = "ScriptableObjects/CreateSentenceCollection", order = 1)]

public class SentenceCollectionSO : ScriptableObject
{
    public int m_charactersCount = 0;
    public Characters[] m_characterName;
    public string[] m_sentence;
    public int[] m_imageSwitchNumber;

    public int GetSentenceCount()
    {
        return m_sentence.Length;
    }

    public Characters GetCharacterName(int _sentenceNumber)
    {
        return m_characterName[_sentenceNumber];
    }

    public string GetSentence(int _sentenceNumber)
    {
        return m_sentence[_sentenceNumber];
    }
}