using UnityEngine;
using UnityEngine.Events;

using WPM.UI.Effects;

public class MainMenuController : MonoBehaviour
{
    private PopUpEffect m_myPopUpEffect;

    [SerializeField] UnityEvent m_startClicked;

    void Start()
    {
        m_myPopUpEffect = GetComponent<PopUpEffect>();
    }

    /// <summary>
    /// Create an event when start button is clicked
    /// </summary>
    public void StartButtonClicked()
    {
        m_myPopUpEffect.MinimiseWindow();
        m_startClicked.Invoke();
    }

    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitButtonClicked()
    {
        Application.Quit();
        Application.OpenURL("https://wordplay.media/Game");
    }
}
