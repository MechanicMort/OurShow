using UnityEngine;
using TMPro;

namespace WPM.Connect.Core
{
    public class Word : MonoBehaviour
    {
        TextMeshPro m_myTMPro;
        Animator m_myAnimator;

        public int wordID { get; set; }
        public string text { get; set; }
        public AudioClip audioClip { get; set; }

        private bool m_isMoving { get; set; }
        public int line { get; set; }
        public float speed { get; set; }

        public bool isClickable { get; set; }
        public bool isLeftHand { get; set; }
        public bool isExtra { get; set; }

        /// <summary>
        /// Reference assignment
        /// </summary>
        private void Awake()
        {
            m_myTMPro = GetComponentInChildren<TextMeshPro>();
            m_myAnimator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            isClickable = true;
            isExtra = false;
            m_isMoving = false;
        }

        /// <summary>
        /// Update movement, from left to right for LF hand collection, and from bottom to top for RH collection
        /// </summary>
        void Update()
        {
            if (!m_isMoving) return;

            if (isLeftHand)
            {
                transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
            } else
            {
                transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
            }
        }

        /// <summary>
        /// Update visible text with content of stored one in the object
        /// </summary>
        public void UpdateWordText()
        {
            m_myTMPro.text = text;
        }

        /// <summary>
        /// Set movement for object
        /// </summary>
        public void SetIsMoving(bool _isMoving)
        {
            m_isMoving = _isMoving;
        }

        /// <summary>
        /// Use force on this object, to make special effect
        /// </summary>
        public void BounceObject()
        {
            float t_randomDirection = Random.Range(-50.0f, 50.0f);
            float t_randomUpForce = Random.Range(150.0f, 250.0f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(t_randomDirection, t_randomUpForce));
        }

        /// <summary>
        /// Set animation framerate to fit movement speed
        /// </summary>
        public void AdjustAnimationSpeed()
        {
            m_myAnimator.speed = (speed * 3);
        }

        /// <summary>
        /// Play death animation
        /// </summary>
        public void PlayDeathAnimation()
        {
            m_myAnimator.Play("Worm_death");
        }
    }
}