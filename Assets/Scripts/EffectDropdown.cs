using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CommandBasedDropdown))]
public class EffectDropdown : MonoBehaviour
{
    public List<Transform> effectItems = new List<Transform>();

    private int frame = 0;
    private void Awake()
    {
        GetComponent<CommandBasedDropdown>().onValueChanged.AddListener(SetEffect);
        effectItems.ForEach(x => x.gameObject.SetActive(true));
    }

    private void Update()
    {
        //give everything time to load without causing race conditions
        if (frame < 4)
        {
            if (frame > 2) DisableAll();
            frame++;
        }
    }

    private void DisableAll()
    {
        effectItems.ForEach(x => x.gameObject.SetActive(false));
    }

    public void SetEffect(int newVal)
    {
        if (newVal - 1 < effectItems.Count) //Ignore the whitespace at the end
        {
            DisableAll();
            if (newVal > 0)
            {
                effectItems[newVal - 1].gameObject.SetActive(true);
            }
        }
    }
}
