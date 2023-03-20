using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EpisodeSelect : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    // Start is called before the first frame update

    public void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("Episode");
        PlayerPrefs.SetInt("Episode", dropdown.value);
        print(PlayerPrefs.GetInt("Episode"));
    }

    public void EpisodeChanged()
    {
        PlayerPrefs.SetInt("Episode",dropdown.value)    ;
        print(PlayerPrefs.GetInt("Episode"));
    }

}
