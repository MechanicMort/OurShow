using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.SayIt.Core
{
    [CreateAssetMenu(fileName = "imageCollection_", menuName = "ScriptableObjects/CreateNewImageCollection", order = 3)]

    public class ImageCollection : ScriptableObject
    {
        public Sprite[] m_subjectImageCollection;
    }
}