using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSelect : MonoBehaviour
{
    public FillTheGapManager FillTheGapManager;
    public GameObject selectCanvas;
    public GameObject slotCanvas;


    public void SelectedWords()
    {

        FillTheGapManager.isLetters = false;
        FillTheGapManager.isWords = true;
        FillTheGapManager.StartGame();
        CloseMenu();
    }


    public void CloseMenu()
    {
        selectCanvas.SetActive(false);
        slotCanvas.SetActive(true);
    }

    public void SelectedLetters()
    {
        FillTheGapManager.isLetters = true;
        FillTheGapManager.isWords = false;
        FillTheGapManager.StartGame();
        CloseMenu();
    }
}
