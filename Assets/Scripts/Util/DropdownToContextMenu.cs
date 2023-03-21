using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownToContextMenu : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private int resetIndex;
    
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.options.Add(new TMP_Dropdown.OptionData("",null));
        resetIndex = dropdown.options.Count - 1;
        dropdown.SetValueWithoutNotify(resetIndex);
        dropdown.onValueChanged.AddListener((i) => dropdown.SetValueWithoutNotify(resetIndex));
    }
}
