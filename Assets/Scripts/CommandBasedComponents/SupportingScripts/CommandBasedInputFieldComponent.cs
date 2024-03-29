using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class CommandBasedInputFieldComponent : MonoBehaviour
{
    public string inputType;
    public string defaultValue;
    public InputParser parser;
    public InputValidator validator;
    public Action<object> callback;

    [HideInInspector] public dynamic rcbifc;
    private void Awake()
    {
        Type a = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            a = assembly.GetType(inputType) ?? a;
        }
        Type constructed = typeof(RuntimeCommandBasedInputFieldComponent<>).MakeGenericType(a);
        rcbifc = Activator.CreateInstance(constructed);
        rcbifc.Init(GetComponent<TMP_InputField>(),parser,validator,callback);
        rcbifc.BindToShader(GetComponent<ShaderBinding>());
    }

    private void Start()
    {
        if(!rcbifc.cbif.LoadValueFromSave()) rcbifc.cbif.Poke(defaultValue, false);
    }
}
