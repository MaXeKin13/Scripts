using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region CommandPattern
    public Player player;

    [Header("Input")]
    public List<KeyCommand> keyCommands = new List<KeyCommand>();   

    private Command command;

    public void HandleInput(Player player)
    {
        command.Execute(player);
    }

    private void Update()
    {
        //when any key is pressed
        if(Input.anyKeyDown)
        {
            command = HandleInput();

            if(command != null)
                HandleInput(player);
        }
    }

    //get command for button pressed
    public Command HandleInput()
    {
        foreach (KeyCommand keyCommand in keyCommands)
        {
            //add key to dictionary
            if (!keyCommand.key.Equals(KeyCode.None))
                if(Input.GetKeyDown(keyCommand.key))
                {
                    //return command
                    return keyCommand.command;
                }
        }
        return null;
    }
    #endregion
    #region Key-Command Bindings
    [System.Serializable]
    public class KeyCommand
    {
        public KeyCode key;
        public Command command;
    }
    #endregion   
}
