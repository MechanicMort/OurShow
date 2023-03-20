using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int _currentLevel = 0;
    private int _currentWord = 0;

    private List<string> wordDatas;
    private Dictionary<int, List<string>> _words = new Dictionary<int, List<string>>();


    public Words words;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task LoadLevelData()
    {
        int i = _currentLevel;
    
        string[] MyWords = words.words;
        wordDatas = new List<string>(MyWords);
        _words.Add(_currentLevel - 1, wordDatas);
        await Task.Yield();
        
    }   

    public string GetCurrentWordData()
    {
        return _currentWord < _words[_currentLevel - 1].Count ? _words[_currentLevel - 1][_currentWord] : null;
    }

    public string GetRandomWordData()
    {
        int row = UnityEngine.Random.Range(0, _words.Count);
        int column = UnityEngine.Random.Range(0, _words[row].Count);
        return _words[row][column];
    }

    public int GetCurrentWordIndex()
    {
        return _currentWord;
    }

    public bool IsAllWordSpellCorrectlyForCurrentRound()
    {
        return _currentWord == _words[_currentLevel - 1].Count - 1;
    }

    public bool IsAllRoundFinished()
    {
        return _currentLevel == _words.Count;
    }

    public void MoveToNextWord()
    {
        _currentWord++;
    }

    public void MoveToNextRound()
    {
        _currentWord = 0;
        _currentLevel++;
    }

    //public void CheckWordFinishItsGenerateTimes(string word)
    //{
    //    if (word.NumberOfTimeWordGenerate == 3)
    //    {
    //        _words[word.RoundIndex].Remove(word);
    //    }

    //    if (_words[word.RoundIndex].Count == 0)
    //    {
    //        _words.Remove(word.RoundIndex);
    //    }
    //}

    public bool IsAllRandomWordsFinished()
    {
        return _words.Count == 0;
    }
}