using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCommand
{
    public string Name;
    public string Description;
    public Action Function;
}

[ExecuteInEditMode]
public class DeveloperConsole : MonoBehaviour
{
    [SerializeField] private KeyCode m_ActivationKey = KeyCode.BackQuote;
    [SerializeField] private KeyCode m_AcceptInput = KeyCode.KeypadEnter;

    [SerializeField] private InputField m_ConsoleText;
    [SerializeField] private Text m_InputText;

    protected DeveloperConsoleUI m_ConsoleUI;

    private Dictionary<string, ConsoleCommand> m_Commands;

    public static DeveloperConsole Instance { get; private set; }
    public bool IsConsoleVisible { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (m_Commands == null)
        {
            m_Commands = new Dictionary<string, ConsoleCommand>();
        }

        if (m_InputText == null)
        {
            m_InputText = GetComponentInChildren<Text>();
        }

        if(m_ConsoleText == null)
        {
            m_ConsoleText = GetComponentInChildren<InputField>();
        }

        Hide();
    }

    protected virtual void Hide()
    {
        m_ConsoleText.DeactivateInputField();

        // Thanks Unity, make something soo bloody difficult to turn off
        m_ConsoleText.readOnly = true;
        m_ConsoleText.enabled = false;
    }

    protected virtual void Show()
    {
        m_ConsoleText.enabled = true;
        m_ConsoleText.readOnly = false;
        m_ConsoleText.ActivateInputField();
        m_ConsoleText.text = string.Empty;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(m_ActivationKey))
        {
            IsConsoleVisible = !IsConsoleVisible;

            if(IsConsoleVisible)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        if (IsConsoleVisible)
        {
            if (Input.GetKeyDown(m_AcceptInput) && m_ConsoleText.textComponent.text != "")
            {
                SubmitCommand(m_ConsoleText.textComponent.text);
            }
        }
    }

    public void AddCommand(string cmdName, ConsoleCommand command)
    {
        m_Commands.Add(cmdName, command);
    }

    public void RemoveCommand(string cmdName)
    {
        m_Commands.Remove(cmdName);
    }

    private void Print(string text)
    {
        m_ConsoleText.text += text + "\n";
    }

    private void SubmitCommand(string cmdName)
    {
        if (!VerifyInput(ref cmdName))
        {
            Print("Invalid text");
            return;
        }

        m_Commands.TryGetValue(cmdName, out ConsoleCommand command);
        if (command != null)
        {
            command.Function.Invoke();
        }
        else
        {
            Print("Failed to find command. Type \'Help\' to find out more");
        }
    }

    private bool VerifyInput(ref string cmdName)
    {
        return !string.IsNullOrEmpty(cmdName) && !string.IsNullOrWhiteSpace(cmdName);
    }
}