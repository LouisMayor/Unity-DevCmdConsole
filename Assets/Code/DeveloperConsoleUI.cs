using UnityEngine;
using UnityEngine.UI;

public class DeveloperConsoleUI : DeveloperConsole
{
    [Header("UI Elements")]
    [SerializeField] private Canvas m_ConsoleCanvas;
    [SerializeField] private Image m_ActiveFieldBackground;
    [SerializeField] private Image m_TextFieldBackground;
    [SerializeField] private Scrollbar m_Scroll;

    protected override void Awake()
    {
        base.Awake();

        if(m_ConsoleCanvas == null)
        {
            m_ConsoleCanvas = GetComponentInChildren<Canvas>();
        }

        if (m_ActiveFieldBackground == null)
        {
            Debug.LogError("Invalid Active Background");
        }

        if(m_TextFieldBackground == null)
        {
            Debug.LogError("Invalid Text Background");
        }

        if (m_Scroll == null)
        {
            m_Scroll = GetComponentInChildren<Scrollbar>();
        }

        OnUpdatedTextField = UpdateScroll;
    }

    private void UpdateScroll()
    {
        if (m_Scroll == null)
        {
            return;
        }

        // Always at the bottom
        m_Scroll.value = 0.0f;
    }

    protected override void Hide()
    {
        base.Hide();

        m_ConsoleCanvas.enabled = false;
        m_ActiveFieldBackground.enabled = false;
        m_TextFieldBackground.enabled = false;
    }

    protected override void Show()
    {
        m_ConsoleCanvas.enabled = true;
        m_ActiveFieldBackground.enabled = true;
        m_TextFieldBackground.enabled = true;

        base.Show();
    }
}
