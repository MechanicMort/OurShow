using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Words : MonoBehaviour
{
    public TextAsset Wep1;
    public TextAsset wordChoice;
    public TextAsset senChoice;
    public TextAsset Wep2;
    public TextAsset Wep3;
    public TextAsset Sep1;
    public TextAsset Sep2;
    public TextAsset Sep3;
    public TextAsset dum;

    public TextAsset[] assetHolder;

    public string[] words;
    public string[] sentances;
    public string[] dums;
    public void Start()
    {
        dums = new string[dum.text.Split(':').Length];

        //asset holder is so they can be loaded by integer 
        //should read all files in location and dynamically add them
        //use a string at the start of the text files to locate correct file
        words = new string[Wep1.text.Split(':').Length];
        assetHolder = new TextAsset[6];
        assetHolder[0] = Wep1;
        assetHolder[1] = Wep2;  
        assetHolder[2] = Wep3;
        assetHolder[3] = Sep1;
        assetHolder[4] = Sep2;
        assetHolder[5] = Sep3;
        //use player prefs to load correct one here once thats set up
        // PlayerPrefs.GetFloat("Episode");
        wordChoice = assetHolder[PlayerPrefs.GetInt("Episode")];
        senChoice = assetHolder[PlayerPrefs.GetInt("Episode") + 3];
        sentances = new string[senChoice.text.Split(':').Length];

        if (true)// this will eventually use prefs to load correct file from 
        {
            for (int x = 0; x < wordChoice.text.Split(':').Length; x++)
            {
                words[x] = wordChoice.text.Split(':')[x];
            }
            //Split into a words and sentance holder
            for (int i = 0; i < senChoice.text.Split(':').Length; i++)
            {
                sentances[i] = senChoice.text.Split(':')[i];
         
            }
        }

        for (int i = 0; i < dum.text.Split(':').Length; i++)
        {
            dums[i] = dum.text.Split(':')[i];

        }

    }
}
