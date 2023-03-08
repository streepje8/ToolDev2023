using System;
using TMPro;

public class StringInputElement : MenuElement
{
    public TMP_InputField InputField { get; private set; }
    private void Awake()
    {
        InputField = GetComponent<TMP_InputField>();
    }
}
