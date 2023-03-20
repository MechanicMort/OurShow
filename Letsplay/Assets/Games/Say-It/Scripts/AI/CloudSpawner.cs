using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.AI
{
    public class CloudSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] m_cloudPrefabs;
        List<GameObject> m_cloudList;

        float m_cloudStartingPosition = -12.0f;
        float m_cloudHorizontalPosition = 1.5f;
        float m_cloudDistance = -1.0f;
        float m_allCloudsSpeed = 0.5f;

        // Start is called before the first frame update
        void Start()
        {

            m_cloudList = new List<GameObject>();

            for (int i=0; i<m_cloudPrefabs.Length; i++)
            {
                m_cloudStartingPosition = Random.Range(-14.0f, -12.0f) + i;
                m_cloudList.Add(Instantiate(m_cloudPrefabs[i], new Vector3(m_cloudStartingPosition, i + m_cloudHorizontalPosition, m_cloudDistance - i), Quaternion.identity));

                m_cloudList[i].GetComponent<CloudMover>().cloudSpeed = (2 * (i / 10.0f)) + m_allCloudsSpeed;
                m_cloudList[i].transform.SetParent(this.transform, false);
            }
            m_cloudList[1].GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
}