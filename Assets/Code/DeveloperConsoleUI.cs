using UnityEngine;
using UnityEngine.UI;

public class DeveloperConsoleUI : DeveloperConsole
{
    [SerializeField] private Canvas m_ConsoleCanvas;
    [SerializeField] private Image m_ActiveFieldBackground;

    protected override void Awake()
    {
        base.Awake();

        if(m_ConsoleCanvas == null)
        {
            m_ConsoleCanvas = GetComponentInChildren<Canvas>();
        }

        if (m_ActiveFieldBackground == null)
        {
            m_ActiveFieldBackground = GetComponentInChildren<Image>();
        }

        m_ConsoleUI = this;
    }

    protected override void Hide()
    {
        base.Hide();

        m_ConsoleCanvas.enabled = false;
        m_ActiveFieldBackground.enabled = false;
    }

    protected override void Show()
    {
        m_ConsoleCanvas.enabled = true;
        m_ActiveFieldBackground.enabled = true;

        base.Show();
    }
}
