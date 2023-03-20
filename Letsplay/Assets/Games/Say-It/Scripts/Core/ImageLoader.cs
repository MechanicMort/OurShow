using UnityEngine;
using WPM.SayIt.Core;

namespace WPM.UI.Effects
{
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] ImageCollection m_imageCollection;
        //[SerializeField] Sprite[] m_subjectImageCollection;
        [SerializeField] SpriteRenderer m_mySpriteRenderer;

        private void Awake()
        {
            SwitchOffThisObject();
        }

        public void LoadImage(int _imageNumber)
        {
            int t_imageIndex = _imageNumber;

            if (_imageNumber >= m_imageCollection.m_subjectImageCollection.Length)
            {
                t_imageIndex = 0;
                Debug.Log("Not enough images");
            } else
            {
                t_imageIndex = _imageNumber;
            }

            m_mySpriteRenderer.sprite = m_imageCollection.m_subjectImageCollection[t_imageIndex];
        }

        public void SwitchOffThisObject()
        {
            gameObject.SetActive(false);
        }

        public void SwitchOnThisObject()
        {
            gameObject.SetActive(true);
        }
    }
}
