using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance = null;

    public static int m_totalCoins = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }

    }

    public static int GetTotalCoins()
    {
        return m_totalCoins;
    }

    public static void SetTotalCoins(int _coins)
    {
        m_totalCoins = _coins;
    }
}