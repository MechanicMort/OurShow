using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordContainer : MonoBehaviour
{
    [SerializeField] string m_currentWord = "ALLOWED";
    GameObject[] m_letterSlotArray;

    [SerializeField] GameObject m_slotPrefab;

    int m_currentLetter = 0;

    void Start()
    {
        m_letterSlotArray = new GameObject[m_currentWord.Length];
        CreateNewSlots();
    }

    private void CreateNewSlots()
    {
        for (int i=0; i< m_currentWord.Length; i++)
        {
            Vector3 t_positionModifier = new Vector3(i * 125, 0, 0);
            GameObject t_newSlot = Instantiate(m_slotPrefab, transform.position + t_positionModifier, Quaternion.identity);
            t_newSlot.transform.SetParent(this.transform);
            m_letterSlotArray[i] = t_newSlot;
        }
    }

    public bool CheckLetter(char _letter)
    {
        if (m_currentWord[m_currentLetter].Equals(_letter))
        {
            m_letterSlotArray[m_currentLetter].GetComponentInChildren<TextMeshProUGUI>().text = _letter.ToString();
            m_currentLetter++;
            return true;
        } else
        {
            return false;
        }
    }

    public bool isWordComplete()
    {
        if (m_currentWord.Length == m_currentLetter)
        {
            Debug.Log("It was last letter");
            return true;
        } else
        {
            return false;
        }
    }
}