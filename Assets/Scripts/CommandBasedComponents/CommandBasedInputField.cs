using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class CommandBasedInputField<T>
{
    private T oldValue;
    private TMP_InputField ifield;
    private IInputParser<T> parser;
    private IFieldValidator<T> validator;
    private Action<T> callback;
    private ShaderBinding binding;
    
    public CommandBasedInputField(TMP_InputField inputField, IInputParser<T> parser, IFieldValidator<T> validator, Action<T> callback)
    {
        ifield = inputField;
        ifield.onEndEdit.AddListener(Poke);
        oldValue = validator.Invoke(parser.Parse(inputField.text));
        this.parser = parser;
        this.validator = validator;
        this.callback = callback;
        ifield.SetTextWithoutNotify(oldValue.ToString());
    }

    public void Poke(string newValue)
    {
        Action<T> injectedCallback = (val) =>
        {
            binding?.UpdateValue(val);
            callback?.Invoke(val);
        };
        CommandManager.Instance.PushCommand(new ChangeFieldCommand<T>(ifield,oldValue,parser.Parse(newValue),validator,injectedCallback));
        oldValue = validator.Invoke(parser.Parse(newValue));
    }

    public void BindToShader(ShaderBinding shaderBinding)
    {
        binding = shaderBinding;
    }
}
