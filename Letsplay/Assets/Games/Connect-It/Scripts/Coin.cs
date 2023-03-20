using UnityEngine;

namespace WPM.SayIt.Core
{
    public class Coin : MonoBehaviour
    {
        public float delayTime { get; set;}
        float m_delayTimer = 1.0f;

        public Vector2 targetPosition { get; set; }
        public float smoothTime = 0.25f;
        public float speed = 10.0f;
        Vector2 velocity;

        bool m_isFollowingBack;

        void Start()
        {
            delayTime = 0.5f;
            m_delayTimer = delayTime;
        }

        void Update()
        {
            if (!m_isFollowingBack)
            {
                if (m_delayTimer <= 0)
                {
                    m_isFollowingBack = true;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GetComponent<Collider2D>().enabled = true;
                } else
                {
                    m_delayTimer -= Time.deltaTime;
                }
            }
        
            if (m_isFollowingBack)
            {
                transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
            }
        
        }
    }
}
