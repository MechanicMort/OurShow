using UnityEngine;

namespace WPM.SayIt.Core
{
    public class CharacterAnimation : MonoBehaviour
    {
        private Animator m_myAnimator;

        private float m_rareBehaviourTimer = 3;
        private float m_frequentBehaviourTimer = 3;

        private void Start()
        {
            m_myAnimator = GetComponent<Animator>();
        }

        void Update()
        {
            RareBehaviourTimer();
            FrequentBehaviourTimer();
        }

        private void RareBehaviourTimer()
        {
            if (m_rareBehaviourTimer > 0)
            {
                m_rareBehaviourTimer -= Time.deltaTime;
            }
            else
            {
                m_rareBehaviourTimer = Random.Range(10.0f, 15.0f);
                m_myAnimator.SetBool("isRareBehaviour", true);
            }
        }

        private void RareBehaviourEnded()
        {
            m_myAnimator.SetBool("isRareBehaviour", false);
        }

        private void FrequentBehaviourTimer()
        {
            if (m_frequentBehaviourTimer > 0)
            {
                m_frequentBehaviourTimer -= Time.deltaTime;
            }
            else
            {
                m_frequentBehaviourTimer = Random.Range(4.0f, 6.0f);
                m_myAnimator.SetBool("isFrequentBehaviour", true);
            }
        }

        private void FrequentBehaviourEnded()
        {
            m_myAnimator.SetBool("isFrequentBehaviour", false);
        }

        public void PlayBigHappy()
        {
            m_myAnimator.Play("Big_Happy_Reaction");
        }

        public void PlaySmallHappy()
        {
            m_myAnimator.Play("Small_Happy_Reaction");
        }

        public void PlaySad()
        {
            m_myAnimator.Play("Sad_Reaction");
        }

        public void PlayHighFive()
        {
            m_myAnimator.Play("High_Five");
        }
    }
}