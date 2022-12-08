using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker
{
    public static List<ICommand> commands = new List<ICommand>();
    int current = 0;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commands.Add(command);
        current++;
    }

    public void Redo()
    {
        if (current < commands.Count - 1)
        {
            ICommand command = commands[current++];
            command.Execute();
        }
    }
}
