using UnityEngine;
using UnityEngine.Events;

namespace WPM.Connect.Core
{
    public class DangerZone : MonoBehaviour
    {
        [SerializeField] WordSpawner m_myWordSpawner;
        [SerializeField] UnityEvent m_wordInDangerZone;

        float m_destroyDelayTime = 2.0f;

        private void Awake()
        {
            m_myWordSpawner = FindObjectOfType<WordSpawner>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<Word>()) return;
            if (!collision.GetComponent<Word>().isLeftHand) return;

            m_wordInDangerZone.Invoke();

            Word m_wordToDestroy = collision.gameObject.GetComponent<Word>();
            int t_cycledIndex = m_myWordSpawner.GetCycledIndex(m_wordToDestroy.wordID);

            m_wordToDestroy.PlayDeathAnimation();
            m_wordToDestroy.SetIsMoving(false);

            Destroy(m_myWordSpawner.m_spawnedWordsRH[t_cycledIndex].gameObject, m_destroyDelayTime);
            Destroy(collision.gameObject, m_destroyDelayTime);

            m_myWordSpawner.AddWordToCycledRH(t_cycledIndex);

            m_myWordSpawner.m_preparedWordsLH[m_wordToDestroy.wordID] = null;
            m_myWordSpawner.m_preparedWordsRH[m_wordToDestroy.wordID] = null;
        }
    }
}