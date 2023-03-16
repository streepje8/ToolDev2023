using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private Dictionary<string, MenuElement> elements = new Dictionary<string, MenuElement>();

    private bool isInitialized = false;

    public T GetMenuElementById<T>(string id) where T : MenuElement
    {
        if (!isInitialized)
        {
            foreach (var menuElement in GetComponentsInChildren<MenuElement>())
                elements.Add(menuElement.id,menuElement);
            isInitialized = true;
        }
        if (elements.TryGetValue(id, out MenuElement el))
        {
            if(typeof(T) == el.GetType() || el.GetType().IsAssignableFrom(typeof(T))) return (T)Convert.ChangeType(el,typeof(T));
            else Debug.LogError("The menu element you requested is of type " + el.GetType().Name + ". But you requested a " + typeof(T).Name);
        }
        return null;
    }
}
