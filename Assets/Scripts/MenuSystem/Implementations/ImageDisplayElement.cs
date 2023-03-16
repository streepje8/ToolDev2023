using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplayElement : MenuElement
{
    [HideInInspector]public RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }
}
