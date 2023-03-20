using UnityEngine;
using TMPro;

namespace WPM.Connect.Core
{
    public class TextEffectController : MonoBehaviour
    {
        [SerializeField] GameObject m_popUpTextPrefab;

        public void StartCorrectTextEffect()
        {
            Vector3 t_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 l_correctTextPosition = new Vector3(t_mousePosition.x, t_mousePosition.y, 0);

            GameObject l_textEffect = Instantiate(m_popUpTextPrefab, l_correctTextPosition, Quaternion.identity);
            l_textEffect.GetComponent<TextMeshPro>().text = "CORRECT";
            l_textEffect.GetComponent<TextMeshPro>().color = Color.green;
        }

        public void StartWrongTextEffect()
        {
            Vector3 t_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 l_wrongTextPosition = new Vector3(t_mousePosition.x, t_mousePosition.y, 0);

            GameObject l_textEffect = Instantiate(m_popUpTextPrefab, l_wrongTextPosition, Quaternion.identity);
            l_textEffect.GetComponent<TextMeshPro>().text = "WRONG";
            l_textEffect.GetComponent<TextMeshPro>().color = Color.red;
        }

        public void StartComboTextEffect(int _comboScore)
        {
            Vector3 t_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 l_comboTextPosition = new Vector3(t_mousePosition.x - 1, t_mousePosition.y - 1, 0);

            GameObject l_textEffect = Instantiate(m_popUpTextPrefab, l_comboTextPosition, Quaternion.identity);
            l_textEffect.GetComponent<TextMeshPro>().text = "BONUS x" + _comboScore.ToString();
            l_textEffect.GetComponent<TextMeshPro>().color = Color.red;
        }
    }
}