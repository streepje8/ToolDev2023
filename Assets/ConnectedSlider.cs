using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedSlider : MonoBehaviour
{
    public CommandBasedInputFieldComponent inputField;
    public ShaderBinding previewBinding;
    
    public float maxValue = 1f;
    public float minValue = 0f;
    
    private float value = 0f;
    private bool valueChanged = false;
    private Slider slider;
    private dynamic cbif;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(Poke);
        slider.maxValue = 1;
        slider.minValue = 0;
        inputField.callback += (i) => slider.value = float.Parse(i.ToString());;
        value = slider.value;
    }

    private void Start()
    {
        cbif = inputField.rcbifc.cbif;
    }

    private void Update()
    {
        if (valueChanged && !Input.GetMouseButton(0))
        {
            cbif.Poke(value.ToString(CultureInfo.InvariantCulture));
            valueChanged = false;
        }
    }

    public void Poke(float newVal)
    {
        valueChanged = true;
        value = Mathf.Lerp(minValue,maxValue,newVal);
        previewBinding.PreviewValue(value);
    }
}
