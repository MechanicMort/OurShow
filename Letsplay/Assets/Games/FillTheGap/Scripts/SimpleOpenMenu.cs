using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WPM.UI.Effects;


public class SimpleOpenMenu : MonoBehaviour
{
    public GameObject Menu;
    public PopUpEffect pop;
    public void OpenMenuPanel()
    {

        if (!Menu.activeInHierarchy)
        {
            Menu.SetActive(!Menu.activeInHierarchy);
            Menu.GetComponent<PopUpEffect>().MaximiseWindow();
        }
        else if (Menu.activeInHierarchy)
        {
            Menu.GetComponent<PopUpEffect>().MinimiseWindow();
        }

    }

}
