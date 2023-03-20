using UnityEngine;
using WPM.UI.Effects;

namespace WPM.Connect.Core
{
    public class LifePointsController : MonoBehaviour
    {
        [SerializeField] GameObject[] m_lifePointsImages;

        public void UpdateLifePoints(int _lifePoints)
        {
            switch (_lifePoints)
            {
                case 0:
                    SetLifePointImagesCount(0);
                    break;
                case 1:
                    SetLifePointImagesCount(1);
                    break;
                case 2:
                    SetLifePointImagesCount(2);
                    break;
                case 3:
                    SetLifePointImagesCount(3);
                    break;
                default:
                    SetLifePointImagesCount(3);
                    break;
            }
        }

        void SetLifePointImagesCount(int _numberOfLifePoints)
        {
            for (int i = 0; i < _numberOfLifePoints; i++)
            {
                m_lifePointsImages[i].GetComponent<PopUpEffect>().MaximiseWindow();
            }
            for (int i = _numberOfLifePoints; i < m_lifePointsImages.Length; i++)
            {
                if (!m_lifePointsImages[i].activeSelf) return;
                m_lifePointsImages[i].GetComponent<PopUpEffect>().MinimiseWindow();
            }
        }
    }
}