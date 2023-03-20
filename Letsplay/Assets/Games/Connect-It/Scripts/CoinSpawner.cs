using UnityEngine;

namespace WPM.SayIt.Core
{
    /// <summary>
    /// Class responsible for spawning new coins, determines spawning point, and destination point.
    /// </summary>
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] AudioSource m_myAudioSource;

        [SerializeField] AudioClip[] m_coinAudio;  

        [SerializeField] GameObject m_coinPrefab;

        [SerializeField] Transform m_spawningPoint;
        [SerializeField] GameObject m_destinationPoint;

        [SerializeField] int m_coinsToSpawn = 5;
        [SerializeField] int m_spawnedCoins = 0;

        [SerializeField] float m_minSpawnFrequency = 0.1f;
        [SerializeField] float m_maxSpawnFrequency = 0.3f;
        [SerializeField] float m_backDelayTime = 0.5f;

        float m_spawnTimer = 1.0f;

        bool m_isSpawningCoins;

        float m_spawnRandomFactor = 15.0f;
        float m_spawnForce = 200.0f;

        void Start()
        {
            m_spawnTimer = m_minSpawnFrequency;
            m_isSpawningCoins = false;
        }    

        void Update()
        {
            if (m_isSpawningCoins)
            {
                if (m_spawnedCoins < m_coinsToSpawn)
                {
                    if (m_spawnTimer <= 0)
                    {
                        m_spawnedCoins++;
                        SpawnCoin();
                        float t_randomTime = Random.Range(m_minSpawnFrequency, m_maxSpawnFrequency);
                        m_spawnTimer = t_randomTime;
                    } else
                    {
                        m_spawnTimer -= Time.deltaTime;
                    }
                } else
                {
                    m_spawnedCoins = 0;
                    m_isSpawningCoins = false;
                }
            }
        }

        /// <summary>
        /// Public function that can be called on event
        /// </summary>
        public void AddCoins(int _count)
        {
            m_coinsToSpawn = _count;
            m_isSpawningCoins = true;
        }

        /// <summary>
        /// Spawn coin, set starting position, target position, delay time for coin following back to target position, creating random direction for initial jump
        /// </summary>
        private void SpawnCoin()
        {
            GameObject t_coinObject = Instantiate(m_coinPrefab, m_spawningPoint.position, Quaternion.identity);
            t_coinObject.GetComponent<Coin>().targetPosition = m_destinationPoint.transform.position;

            float randomDirection = Random.Range(-m_spawnRandomFactor, m_spawnRandomFactor);
            Vector2 t_coinForce = new Vector2(randomDirection, m_spawnForce);
            t_coinObject.GetComponent<Rigidbody2D>().AddForce(t_coinForce);

            t_coinObject.GetComponent<Coin>().delayTime = m_backDelayTime;

            if (m_coinAudio.Length != 0) m_myAudioSource.clip = m_coinAudio[Random.Range(0, m_coinAudio.Length)];
            m_myAudioSource.Play();
        }
    }
}
