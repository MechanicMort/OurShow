using UnityEngine;

namespace WPM.Connect.Core
{
    [CreateAssetMenu(fileName = "AudioCollectionEpisode_", menuName = "ScriptableObjects/CreateAudioCollection", order = 2)]

    public class AudioCollectionSO : ScriptableObject
    {
        [SerializeField] private AudioClip[] m_audioWords;

        public AudioClip[] GetAudioWords()
        {
            return m_audioWords;
        }
    }
}
