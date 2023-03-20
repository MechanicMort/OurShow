using UnityEngine;

namespace WPM.Connect.Core
{
    [CreateAssetMenu(fileName = "WordsCollectionEpisode_", menuName = "ScriptableObjects/CreateWordCollection", order = 1)]

    public class WordsCollectionSO : ScriptableObject
    {
        [SerializeField] private string[] m_languageWords;

        public string[] GetLanguageWords()
        {
            return m_languageWords;
        }
    }
}
