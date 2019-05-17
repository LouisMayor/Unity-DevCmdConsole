using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private void LogState()
    {
        Debug.Log("I am alive!");
    }

    private void Start()
    {
        ConsoleCommand cmd = new ConsoleCommand();
        cmd.Description = "Logs character state to console";
        cmd.Name = "log character state";
        cmd.Function = LogState;

        DeveloperConsole.Instance.AddCommand("CharacterState", cmd);
    }
}
