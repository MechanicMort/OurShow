using UnityEngine;

namespace WPM.Connect.Core
{
    public class WordScrapper : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }
    }
}