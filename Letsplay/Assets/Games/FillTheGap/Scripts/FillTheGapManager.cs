using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using SayIt;
using WPM.SayIt.Core;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FillTheGapManager : MonoBehaviour
{


    [Header("Sounds")]
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioSource audioPlayer;
    public Sounds sounds;
    public List<AudioClip> mySounds;
    public bool playedSound = false;

    [Header("Progress")]//self explanitory
    public  Image progressBar;
    public float progress;
    public float progressAmountPerCompletion;

    public GameObject ULostPanel;

    [Header("Timer")]//self explanitory
    public bool timerRunning;
    public bool timeStop;
    public Image timerBar;
    public float time; //Used for the UI not for the internal timer for the timer bar
    public float timeInterval; //the amount to increase time by every run of  
    public float maxTime = 100;

    [Header("Round")]
    public GameObject roundPanel;
    public float Round = 1;

    [Header("Settings")]
    private bool inOrder = true;
    private int lastWord = 0;
    public float difficulty = 4;

    [Header("Game")]

    public List<string>  dummyWords;

    public Words words;


    public List<string> myWords;
    public int noOfWords;
    public int noOfWordsReady;

    public bool isWords;
    public string[] individualWords;
    public bool isLetters;
    public GameObject MyItem;
    public GameObject kiteStartPos;
    public  Coiner Coiner;
    public GameObject playZone;
    public GameObject[] spawnZones;
    public List<GameObject> tempSpawnZones = new List<GameObject>();
   public List<GameObject> deleteObjects = new List<GameObject>();
    public string[] withGaps;
    public TextMeshProUGUI itemShelf;
    public TextMeshProUGUI wordNeededShelf;
    public string[] RandomWords;
    private string randomLetters = "abcdefghijklmnopqrstuvwxyz";

    public string testWord = "TEST WORD";
    public StringBuilder wordStr;
    public StringBuilder tempRandomLetters;
    private string[] tempArr;



    // Start is called before the first frame update
    void Start()
    {
        mySounds.AddRange(sounds.gameSounds);

        
        //myWords = new List<string>();
        //myWords.AddRange(words.words);
        //print(myWords.Count);
        //for (int i = 0; i < myWords.Count; i++)
        //{
        //    print(myWords[i]);
        //}
    }


    public void ThisLetterDone(bool endofRound)
    {
        noOfWordsReady += 1;
        if (noOfWordsReady == noOfWords)
        {
            //reset clock and start/restart it 
            time = 0;
            timeStop = false;
            
            if (!timerRunning && !endofRound)
            {
              
                StartCoroutine(Time());
            }

        }
    }

    void RoundComplete()
    {
        Round += 1;
        progress = 0;
        difficulty += 0.1f;
        maxTime -= 1;
        lastWord = 0;
        StopAllCoroutines();  
        myWords.Clear();
        myWords = new List<string>();
        playedSound = false;

        if (isLetters)
        {
            myWords.AddRange(words.words);
        }
        else
        {
            myWords.AddRange(words.sentances);
        }

        roundPanel.SetActive(true);
        roundPanel.GetComponentInChildren<TMP_Text>().text = "Round " + Round;
        Coiner.AddCoins();
        StartCoroutine(SimpleTimerForUI(1));
    }


    private IEnumerator SimpleTimerForUI(float time)
    {
        //simple timer to shjow round completeion
        //play round over sound
        yield return new WaitForSeconds(time);
        roundPanel.SetActive(false);
        noOfWordsReady = 0;
        for (int i = 0; i < deleteObjects.Count; i++)
        {
            if (deleteObjects[i].GetComponent<MyItem>())
            {
                deleteObjects[i].GetComponent<MyItem>().letterDone = false;
            }
        }
        StartCoroutine(ResetGame());
    }


    public void StartGame()
    {
        dummyWords.AddRange(words.dums);
        //create list of words to add to
        myWords = new List<string>();
        noOfWordsReady = 0;
        noOfWords = 0;
        tempSpawnZones.Clear();
        tempSpawnZones.AddRange(spawnZones);
        playedSound = false;

        //find new word or sentance then lower case it and add it to a string builder
        if (isLetters)
        {
            myWords.AddRange(words.words);
        }
        else
        {
            myWords.AddRange(words.sentances);
        }
        if (inOrder)
        {
            testWord = myWords[lastWord];
            lastWord += 1;
        }
        else
        {
            testWord = myWords[Random.Range(0, myWords.Count)];
        }
        //remove used words
        myWords.Remove(testWord);
        testWord = testWord.ToLower();
       //these two stringbuilders are used to seperate out characters from strings easier. wordStr is the word the player needs to rebuild and tempRandomLetters is used so no letters are repeated 
        wordStr = new StringBuilder(testWord);
        tempRandomLetters = new StringBuilder(randomLetters);
        MakeGaps();
 
    }



    private IEnumerator Time()
    {
        timerRunning = true;
        yield return new WaitForSeconds(0.01f);
        if (time <= maxTime)
        {
            time += timeInterval;
        }
        else
        {
            print("Game Lost");
            //game lost sound
            ULostPanel.SetActive(true);

        }

        if (!timeStop)
        {

            StartCoroutine(Time());
        }
        else
        {
            timerRunning = false;
        }
    }

    public IEnumerator ResetGame()
    {
        //resets only needed after first run put in here

        for (int i = 0; i < deleteObjects.Count; i++)
        {
            if (deleteObjects[i].GetComponent<MyItem>())
            {
                deleteObjects[i].GetComponent<MyItem>().RoundDone();
            }
        }

        yield return  new WaitForSeconds(0.1f);
        if (noOfWordsReady != noOfWords)
        {
            StartCoroutine(ResetGame());
        }
        else
        {
            deleteObjects.ForEach(Destroy);
            deleteObjects.Clear();
            StartGame();
        }
            
    }


    public void MakeGaps()
    {
        noOfWords = 0;
        if (isLetters)
        {
            tempArr = new string[testWord.Length];
           
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] = "_";
            }
   
            int randNum;
            for (int i = 0; i < Mathf.RoundToInt(testWord.Length * difficulty);)
            {

                randNum = Random.Range(0,testWord.Length);
                if (wordStr[randNum] != '_' && wordStr[randNum] != ' ')
                {
                    tempArr[randNum] = wordStr[randNum].ToString();
                    wordStr[randNum] = '_';
                    for (int x = 0; x < tempRandomLetters.Length; x++)
                    {
                        if (tempRandomLetters[x].ToString() == tempArr[randNum])
                        {
                            tempRandomLetters.Remove(x, 1);
                            break;
                        }
                    }
                    i++;
                    noOfWords++;
                }
            }
            for (int i = 0; i < tempArr.Length;i++)
            {
                if (tempArr[i] != "_")
                {
                    GameObject newItem = Instantiate(MyItem);
                    newItem.GetComponent<MyItem>().myItem = tempArr[i];
                    newItem.GetComponent<MyItem>().correctAnswer = true;
                    randNum = Random.Range(0, tempSpawnZones.Count);
                    newItem.transform.position = kiteStartPos.transform.position;
                    newItem.GetComponent<MyItem>().myPos = tempSpawnZones[randNum];
                    deleteObjects.Add(newItem);
                    tempSpawnZones.RemoveAt(randNum);
                }

            }
            CreateDummies();

        }
        else
        {
            individualWords = testWord.Split(' ' , System.StringSplitOptions.RemoveEmptyEntries); 
            int randNum;
            tempArr = new string[individualWords.Length];
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] = "_";
            }
            for (int i = 0; i < Mathf.RoundToInt(individualWords.Length * difficulty);)
            {

  
                randNum = Random.Range(0,individualWords.Length);
                if (individualWords[randNum] != "_")
                {
                    tempArr[randNum] = individualWords[randNum].ToString();
                    individualWords[randNum] = "_";
                    i++;
                    noOfWords++;
                }
            }
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (tempArr[i] != "_")
                {
                    print(tempArr[i]);
                    GameObject newItem = Instantiate(MyItem);
                    newItem.GetComponent<MyItem>().myItem = tempArr[i];
                    newItem.GetComponent<MyItem>().correctAnswer = true;
                    randNum = Random.Range(0, tempSpawnZones.Count);
                    newItem.transform.position = kiteStartPos.transform.position;
                    newItem.GetComponent<MyItem>().myPos = tempSpawnZones[randNum];
                    deleteObjects.Add(newItem);
                    tempSpawnZones.RemoveAt(randNum);
                }

            }

            wordStr.Clear();
            for (int i = 0; i < individualWords.Length; i++)
            {
                wordStr.Append(individualWords[i]);
                wordStr.Append(' ');
            }
            CreateDummies();
        }



    }


    public void IncorrectAnswer(float time)
    {
        time += 1;
        audioPlayer.PlayOneShot(loseSound);
    }


    public void CorrectAnswer(string answer)
    {
        if (isLetters)
        {
            if (testWord.Contains(answer))
            {
                for (int i = 0; i < tempArr.Length; i++)
                {
                    if (tempArr[i] == answer)
                    {
                        for (int c = 0; c < wordStr.Length; c++)
                        {
                            if (wordStr[c] == '_' && testWord[c] == answer[0])
                            {
                                wordStr[c] = answer[0];
                                break;
                            }
                        }
                        print("Found Slot");
                        tempArr[i] = "_";

                    }
                }
                //play correct sound here
                audioPlayer.PlayOneShot(winSound);
                print("Correct");
            }
        }
        else
        {
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (tempArr[i] == answer)
                {
                    individualWords[i] = tempArr[i];                
                    
                }
            }
            wordStr.Clear();
            for (int i = 0; i < individualWords.Length; i++)
            {
                wordStr.Append(individualWords[i]);
                wordStr.Append(' ');
            }
        }
        // if the wordStr does not contain the '_' Character then all missing chars have been found thus its complete this workds for both letters and words
        if (!wordStr.ToString().Contains('_'))
        {
            time = 0;
            timeStop =  true;
            print("WordComplete");
            progress += progressAmountPerCompletion;
            //check if that last completion pushed player over 100% done
            if (progress <= 100)
            {
                noOfWordsReady = 0;
                for (int i = 0; i < deleteObjects.Count; i++)
                {
                    if (deleteObjects[i].GetComponent<MyItem>())
                    {
                        deleteObjects[i].GetComponent<MyItem>().letterDone = false;
                        deleteObjects[i].GetComponent<MyItem>().endOfRound = true;
                    }
                }
                StartCoroutine(ResetGame());
            }
            else
            {
                print("Round Complete");
                RoundComplete();
            }
     
        }
    }


    public void CreateDummies()
    {
        if (isLetters)
        {
            for (int i = 0; i < difficulty * 10; i++)
            {
                noOfWords++;
                GameObject newItem = Instantiate(MyItem);
                newItem.GetComponent<MyItem>().myItem = tempRandomLetters[Random.Range(0, tempRandomLetters.Length)].ToString();
                newItem.GetComponent<MyItem>().correctAnswer = false;
                int rand = Random.Range(0, tempSpawnZones.Count);
                newItem.transform.position = kiteStartPos.transform.position;
                newItem.GetComponent<MyItem>().myPos = tempSpawnZones[rand];
                tempSpawnZones.RemoveAt(rand);
                deleteObjects.Add(newItem);
                
            }
        }
        else
        {
            for (int i = 0; i < difficulty * 10; i++)
            {
                noOfWords++;
                GameObject newItem = Instantiate(MyItem);
                print(dummyWords[Random.Range(0, dummyWords.Count)].ToString());
                newItem.GetComponent<MyItem>().myItem = dummyWords[Random.Range(0, dummyWords.Count)].ToString();
                newItem.GetComponent<MyItem>().correctAnswer = false;
                int rand = Random.Range(0, tempSpawnZones.Count);
                newItem.transform.position = kiteStartPos.transform.position;
                newItem.GetComponent<MyItem>().myPos = tempSpawnZones[rand];
                tempSpawnZones.RemoveAt(rand);
                deleteObjects.Add(newItem);

            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        itemShelf.text = wordStr.ToSafeString();
        wordNeededShelf.text = testWord.ToSafeString();
        timerBar.fillAmount = time * 0.01f;
        progressBar.fillAmount = progress * 0.01f;
        if (noOfWordsReady == noOfWords && !playedSound)
        {
            for (int i = 0; i < mySounds.Count; i++)
            {         
                if (mySounds[i].name.ToLower() == testWord)
                {
                    audioPlayer.PlayOneShot(mySounds[i]);
                    playedSound = true;
                }
            }
        }

    }
}
