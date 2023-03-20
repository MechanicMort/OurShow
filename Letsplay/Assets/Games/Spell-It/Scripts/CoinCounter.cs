using UnityEngine;
using TMPro;

namespace WPM.SayIt.Core
{
    /// <summary>
    /// Class responsible for scrapping coins and updating TMPro counte
    /// </summary>
    public class CoinCounter : MonoBehaviour
    {
        AudioSource m_myAudioSource;
        [SerializeField] TextMeshPro m_myTMProCounter;

        [SerializeField] AudioClip[] m_coinAudio;

        public int coinCount { get; set; }

        private void Awake()
        {
            m_myAudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            coinCount = GameData.GetTotalCoins();
            m_myTMProCounter.text = coinCount.ToString();
        }

        private void OnTriggerEnter2D(Collider2D _collision)
        {
            if (_collision.gameObject.tag == "Coin")
            {
                AddCoin(_collision);
            }
        }

        /// <summary>
        /// Update score on every coin added to the counter
        /// </summary>
        private void AddCoin(Collider2D _collision)
        {
            m_myAudioSource.clip = m_coinAudio[Random.Range(0, m_coinAudio.Length)];
            m_myAudioSource.Play();

            Destroy(_collision.gameObject);
            coinCount++;
            m_myTMProCounter.text = coinCount.ToString();
        }

        public void Reset()
        {
            coinCount = 0;
            m_myTMProCounter.text = coinCount.ToString();
        }
    }
}