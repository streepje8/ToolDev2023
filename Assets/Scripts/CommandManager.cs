using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CommandManager : Singleton<CommandManager>
{

    public Stack<ICommand> history = new Stack<ICommand>();
    public Stack<ICommand> redoHistory = new Stack<ICommand>();
    public int changesBeforeRedoDiscard = 1;


    private int changesSinceLastRedo = 0;
    private void Awake()
    {
        Instance = this;
    }

    public void PushCommand(ICommand command)
    {
        if (redoHistory.Count > 0 && changesSinceLastRedo > changesBeforeRedoDiscard) {redoHistory = new Stack<ICommand>();}
        changesSinceLastRedo++;
        command?.Execute();
        history.Push(command);
    }

    public void Undo()
    {
        if (history.Count > 0)
        {
            ICommand command = history.Pop();
            command.Undo();
            redoHistory.Push(command);
        }
    }

    public void Redo()
    {
        if (redoHistory.Count > 0)
        {
            ICommand command = redoHistory.Pop();
            command.Redo();
            history.Push(command);
            changesSinceLastRedo = 0;
        }
    }
}
