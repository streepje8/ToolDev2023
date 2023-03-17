using System;
using TMPro;

public class ChangeFieldCommand<T> : ICommand
{
    private TMP_InputField field;
    private FieldValidator validator;
    private T from;
    private T to;
    private Action<T> callback;


    public delegate T FieldValidator(T input);
    
    public ChangeFieldCommand(TMP_InputField field, T from, T to, FieldValidator validator, Action<T> callback)
    {
        this.field = field;
        this.from = from;
        this.to = to;
        this.validator = validator;
        this.callback = callback;
    }
    
    public void Execute()
    {
        T newValue = validator.Invoke(to);
        field.SetTextWithoutNotify(newValue.ToString());
        callback.Invoke(newValue);
    }

    public void Undo()
    {
        T newValue = validator.Invoke(from);
        field.SetTextWithoutNotify(newValue.ToString());
        callback.Invoke(newValue);
    }

    public void Redo()
    {
        T newValue = validator.Invoke(to);
        field.SetTextWithoutNotify(newValue.ToString());
        callback.Invoke(newValue);
    }
}
