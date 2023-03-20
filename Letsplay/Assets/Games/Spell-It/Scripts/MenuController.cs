using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject m_myMenu;
    [SerializeField] GameObject m_myWinScreen;
    [SerializeField] GameObject m_myLoseScreen;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OpenMenu()
    {
        m_myMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        m_myMenu.SetActive(false);
    }

    public void OpenWinScreen()
    {
        m_myWinScreen.SetActive(true);
    }

    public void OpenLoseScreen()
    {
        m_myLoseScreen.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayNext()
    {
        string[] t_sceneName = SceneManager.GetActiveScene().name.Split("_");
        int t_nextLevel = int.Parse(t_sceneName[1]) + 1;
        SceneManager.LoadScene(t_sceneName[0] + "_" + t_nextLevel);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
