using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class RuntimeCommandBasedInputFieldComponent<T>
{
    private CommandBasedInputField<T> cbif;
    
    public void Init(TMP_InputField inputField, InputParser parser, InputValidator validator, Action<object> callback)
    {
        cbif = new CommandBasedInputField<T>(inputField,(IInputParser<T>)parser,(IFieldValidator<T>)validator, (t) => callback?.Invoke(t));
    }
}
