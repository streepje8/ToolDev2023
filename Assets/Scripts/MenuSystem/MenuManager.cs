using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<Menu> menus = new List<Menu>();

    private Menu activeMenu = null;

    private void Awake()
    {
        menus.ForEach(x => x.gameObject.SetActive(false));
        SwitchMenu(0);
    }

    public void SwitchMenu(int index)
    {
        if (index < 0 || index > menus.Count) return;
        if(activeMenu != null) activeMenu.gameObject.SetActive(false);
        activeMenu = menus[index];
        activeMenu.gameObject.SetActive(true);
    }
}
