using System;
using TMPro;
using UnityEngine;

public class ChangeDropdownCommand : ICommand
{
    private TMP_Dropdown dropdown;
    private int from;
    private int to;

    private Action<int> callback;
    
    public ChangeDropdownCommand(TMP_Dropdown dropdown,int from, int to, Action<int> callback)
    {
        this.dropdown = dropdown;
        this.from = from;
        this.to = to;
        this.callback = callback;
    }

    public void Execute()
    {
        dropdown.SetValueWithoutNotify(to);
        callback.Invoke(to);
    }

    public void Undo()
    {
        dropdown.SetValueWithoutNotify(from);
        callback.Invoke(from);
    }

    public void Redo()
    {
        dropdown.SetValueWithoutNotify(to);
        callback.Invoke(to);
    }
}
