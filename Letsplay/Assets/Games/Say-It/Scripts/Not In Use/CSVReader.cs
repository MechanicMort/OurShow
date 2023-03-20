using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace SayIt.Utils
{
    public class CSVReader : MonoBehaviour
    {
        StreamReader m_streamReader;

        Dictionary<int, string> m_wordsCollection;
        string m_pathCSV;

        void Start()
        {
        }

        public Dictionary<int, string> ReadFileForEpisode(int _episodeNumber)
        {
            SetEpisode(_episodeNumber);
            bool l_endOfFile = false;

            m_streamReader = new StreamReader(m_pathCSV);
            m_wordsCollection = new Dictionary<int, string>();

            int lineNumber = 0;
            while (!l_endOfFile)
            {
                string l_dataString = m_streamReader.ReadLine();

                if (l_dataString == null)
                {
                    l_endOfFile = true;
                    break;
                }

                m_wordsCollection.Add(lineNumber, l_dataString);
                lineNumber++;
            }
            return m_wordsCollection;

            /*
            // Debugging only
            foreach( KeyValuePair<int, string> kvp in m_wordsCollection )
            {
                Debug.Log("Key = " + kvp.Key + " , Value = " + kvp.Value);
            }
            */
        }

        void SetEpisode(int _episodeNumber)
        {
            switch (_episodeNumber)
            {
                case 1:
                    m_pathCSV = "Assets/DataCollection/Word_List_Episode_1.csv";
                    break;
                case 2:
                    m_pathCSV = "Assets/DataCollection/Word_List_Episode_2.csv";
                    break;
                default:
                    m_pathCSV = "Assets/DataCollection/Word_List_Episode_3.csv";
                    break;
            }
        }
    }
}
