using UnityEngine;

namespace WPM.SayIt.Core
{
    public class AudioPlayer : MonoBehaviour
    {
        private static AudioPlayer instance = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
