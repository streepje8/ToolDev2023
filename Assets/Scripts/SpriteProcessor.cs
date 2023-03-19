using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteProcessor : MonoBehaviour
{
    public Material template;
    /*[HideInInspector]*/public Material runtimeMaterial;
    [HideInInspector]public RenderTexture processed;
    
    private void Awake()
    {
        runtimeMaterial = new Material(template);
    }

    public void ProcessImage(Texture2D sprite)
    {
        if (processed == null) processed = new RenderTexture(sprite.width,sprite.height,1);
        Graphics.Blit(sprite,processed,runtimeMaterial);
    }
}
