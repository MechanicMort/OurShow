using UnityEngine;
using UnityEngine.Events;
using WPM.SayIt.Core;

namespace WPM.Connect.Core
{
    public class BonusController : MonoBehaviour
    {
        [SerializeField] TextEffectController m_myTextEffectController;
        [SerializeField] CoinSpawner m_myCoinSpawner;

        [SerializeField] int m_coinsToAdd = 2;

        int m_comboCounter = 0;
        int m_comboModifier = 1;

        [SerializeField] UnityEvent m_comboEvent;


        public void AddCombo()
        {
            m_comboCounter++;

            switch (m_comboCounter)
            {
                case 2:
                    m_myTextEffectController.StartComboTextEffect(2);
                    m_comboModifier = 2;
                    break;
                case 5:
                    m_myTextEffectController.StartComboTextEffect(5);
                    m_comboModifier = 5;
                    m_comboEvent.Invoke(); // Increase speed
                    break;
                case 9:
                    m_myTextEffectController.StartComboTextEffect(9);
                    m_comboModifier = 9;
                    break;
                case 14:
                    m_myTextEffectController.StartComboTextEffect(14);
                    m_comboModifier = 14;
                    break;
                case 20:
                    m_myTextEffectController.StartComboTextEffect(20);
                    m_comboModifier = 20;
                    break;
                default:
                    break;
            }

            int t_totalCoins = m_coinsToAdd * m_comboModifier;
            m_myCoinSpawner.AddCoins(t_totalCoins);
        }

        public void Reset()
        {
            m_comboCounter = 0;
            m_comboModifier = 1;
        }
    }
}
