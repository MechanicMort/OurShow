using UnityEngine;

namespace WPM.SayIt.Core
{
    public class Coiner : MonoBehaviour
    {
        [SerializeField] CoinSpawner m_myCoinSpawner;
        [SerializeField] CoinCounter m_myCoinCounter;

        [SerializeField] int m_totalCoins = 0;
        [SerializeField] int m_defaultCoinsToSpawn = 5;
        [SerializeField] int m_actualCoinsToSpawn = 5;

        void Start()
        {
            m_actualCoinsToSpawn = m_defaultCoinsToSpawn;
        }

        public void AddCoins()
        {
            m_myCoinSpawner.AddCoins(m_actualCoinsToSpawn);
            m_actualCoinsToSpawn = m_defaultCoinsToSpawn;
            m_totalCoins += m_actualCoinsToSpawn;
        }

        public void AddCoins(int _numberOfCoins)
        {
            m_myCoinSpawner.AddCoins(_numberOfCoins);
            m_actualCoinsToSpawn = m_defaultCoinsToSpawn;
            m_totalCoins += _numberOfCoins;
        }

        public void DecreaseCoinsCount()
        {
            if (m_actualCoinsToSpawn > 1)
            {
                m_actualCoinsToSpawn--;
            }
        }
    }
}