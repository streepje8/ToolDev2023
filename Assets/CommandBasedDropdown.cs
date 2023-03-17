using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Dropdown))]
public class CommandBasedDropdown : MonoBehaviour
{
    public UnityEvent<int> onValueChanged;

    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void PokeDropdownDetection(int newValue)
    {
        CommandManager.Instance.PushCommand(new ChangeDropdownCommand(dropdown,dropdown.value,newValue, (i) => { onValueChanged.Invoke(i); }));
    }
    
}
