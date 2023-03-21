using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Dropdown))]
public class CommandBasedDropdown : MonoBehaviour
{
    public UnityEvent<int> onValueChanged;
    
    private TMP_Dropdown dropdown;
    private int oldValue;
    
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(PokeDropdownDetection);
        oldValue = dropdown.value;
    }

    public void PokeDropdownDetection(int newValue)
    {
        CommandManager.Instance.PushCommand(new ChangeDropdownCommand(dropdown,oldValue,newValue, (i) => { onValueChanged.Invoke(i); Debug.Log("Changed to: " + i); }));
        oldValue = newValue;
    }
    
}
