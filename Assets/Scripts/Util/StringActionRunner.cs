using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct StringAction
{
    public string action;
    public UnityEvent reaction;
}

public class StringActionRunner : MonoBehaviour
{
    public List<StringAction> actions = new List<StringAction>();

    private Dictionary<string, StringAction> lookup = new Dictionary<string, StringAction>();
    private void Awake()
    {
        actions.ForEach(x => lookup.Add(x.action.ToLower(CultureInfo.InvariantCulture),x)); //Bake the actions
    }

    public bool TryRunAction(string action)
    {
        if (lookup.TryGetValue(action.ToLower(CultureInfo.InvariantCulture), out StringAction saction))
        {
            saction.reaction.Invoke();
            return true;
        }
        return false;
    }

    public void RunActionTMP()
    {
        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        if (!TryRunAction(dropdown.options[dropdown.value].text)) Debug.LogWarning("One or more TMP String Actions did not execute!");
    }
}
