using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ConsoleCommand
{
    public string Name;
    public string Description;
    public Action Function;
}

[ExecuteInEditMode]
public class DeveloperConsole : MonoBehaviour
{
    [Header("Console Elements")]
    [SerializeField] private KeyCode m_ActivationKey = KeyCode.BackQuote;
    [SerializeField] private KeyCode m_AcceptInput = KeyCode.KeypadEnter;

    [SerializeField] private Text m_ConsoleTextField;
    [SerializeField] private InputField m_InputTextField;
    [SerializeField] private Text m_InputText;

    private Dictionary<string, ConsoleCommand> m_Commands;

    public static DeveloperConsole Instance { get; private set; }
    public bool IsConsoleVisible { get; private set; }

    protected Action OnUpdatedTextField;

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

        if(m_InputTextField == null)
        {
            m_InputTextField = GetComponentInChildren<InputField>();
        }

        Hide();
        //Show();

        ConsoleCommand cmd = new ConsoleCommand();
        cmd.Description = "Displays All Active Commands";
        cmd.Name = "Help";
        cmd.Function = CmdHelp;

        Instance.AddCommand(cmd.Name, cmd);
    }

    protected virtual void Hide()
    {
        m_InputTextField.DeactivateInputField();

        // Thanks Unity, make something soo bloody difficult to turn off
        m_InputTextField.readOnly = true;
        m_InputTextField.enabled = false;
    }

    protected virtual void Show()
    {
        m_InputTextField.enabled = true;
        m_InputTextField.readOnly = false;
        m_InputTextField.ActivateInputField();
        m_InputTextField.text = string.Empty;
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
            if (Input.GetKeyDown(m_AcceptInput) && m_InputTextField.textComponent.text != "")
            {
                SubmitCommand(m_InputTextField.textComponent.text);
                m_InputTextField.text = string.Empty;
            }
        }
    }

    private void CmdHelp()
    {
        foreach (KeyValuePair<string, ConsoleCommand> keyValuePair in m_Commands)
        {
            Print($"{keyValuePair.Key} - {keyValuePair.Value.Name} - {keyValuePair.Value.Description}");
        }
    }

    public void AddCommand(string cmdName, ConsoleCommand command)
    {
        if (m_Commands.ContainsKey(cmdName))
        {
            Debug.LogError($"Tried adding command {cmdName}, but it already exists");
            return;
        }

        m_Commands.Add(cmdName, command);
    }

    public void RemoveCommand(string cmdName)
    {
        if(!m_Commands.ContainsKey(cmdName))
        {
            Debug.LogError($"Tried removing command {cmdName}, but it doesn't exist");
            return;
        }

        m_Commands.Remove(cmdName);
    }

    private void Print(string text)
    {
        m_ConsoleTextField.text += text + "\n";
        OnUpdatedTextField?.Invoke();
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
            Print(cmdName);
            command.Function.Invoke();
        }
        else
        {
            Print($"Failed to find command \'{cmdName}\'. Type \'Help\' to find out more");
        }
    }

    private bool VerifyInput(ref string cmdName)
    {
        return !string.IsNullOrEmpty(cmdName) && !string.IsNullOrWhiteSpace(cmdName);
    }
}