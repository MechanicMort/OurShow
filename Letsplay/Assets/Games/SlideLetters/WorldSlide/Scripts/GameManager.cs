using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WPM.Connect.Core;


//DESCLAIMER ON ALL THIS ITS TERRIBLE AS IT HAD TO UNDERGO A LARGE RECODE TO FIT A NEW MODEL OF WORD HANDLEING 
public class GameManager : MonoBehaviour
{
    [SerializeField] private float _intervalBetweenFallingWords;

    public enum GameType
    {
        Simple,
        Advance
    }

    public GameType GameMode;

    public static GameManager Instance;

    [SerializeField] private ParrotsController _parrotsController;
    [SerializeField] private GridController _gridController;

    public Sprite[] sprites;
    public GameObject backGround;
    public string wordData;
    public Words words;

    private bool _isGameOver;

    private List<string> _wordList;
    private string _word;

    private int currentLetter = 0;


    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    Task task =  StartGame();
    //}

    public async void StartGame()
    {
        backGround.GetComponent<Image>().sprite = sprites[1];
        LevelManager.Instance.MoveToNextRound();
        await LevelManager.Instance.LoadLevelData();

        string[] MyWords = words.words;
        _wordList = new List<string>(MyWords);

        if (GameMode == GameType.Simple)
        {
            MoveToNextWord();
        }
        else
        {
            GenerateWordRandomly();
        }
    }

    private async void MoveToNextWord()
    {

        _word = _wordList[currentLetter];
        currentLetter++;
        await _parrotsController.InitializeParrot(_word);
        _gridController.GenerateLetters(_word);
    }

    public async void SpellCorrect(string wordData)
    {
        await _parrotsController.SpellCorrect(wordData);

        if (GameMode != GameType.Simple)
        {
            if (LevelManager.Instance.IsAllRandomWordsFinished() &&
                _gridController.IsGridEmpty())
            {
                FinishGame();
            }

            return;
        }

        if (LevelManager.Instance.IsAllWordSpellCorrectlyForCurrentRound())
        {
            await _gridController.ClearGrid();
            if (LevelManager.Instance.IsAllRoundFinished())
            {
                GameMode = GameType.Advance;
                GenerateWordRandomly();
            }
            else
            {
                LevelManager.Instance.MoveToNextRound();
            }
        }
        else
        {
            LevelManager.Instance.MoveToNextWord();
        }
        MoveToNextWord();
    }

    private async void GenerateWordRandomly()
    {
        while (!LevelManager.Instance.IsAllRandomWordsFinished() && !_isGameOver)
        {
            wordData = LevelManager.Instance.GetRandomWordData();
          //  LevelManager.Instance.CheckWordFinishItsGenerateTimes(wordData);
            _gridController.GenerateLetters(wordData);
            await Task.Delay((int)(_intervalBetweenFallingWords * 1000));
        }
    }

    public async void GameOver()
    {
        print("GameOver");
        _isGameOver = true;
        await _gridController.ClearGrid();
    }

    private void FinishGame()
    {
        print("Game Finish");
    }
}