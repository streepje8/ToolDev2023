using UnityEngine;

public class SpriteProcessor : MonoBehaviour
{
    public Material template;
    [HideInInspector]public RenderTexture processed;
    
    private void Awake()
    {
        ToolManager.Instance.ssi.Initialize(template);
    }

    public void ProcessImage(Texture2D sprite)
    {
        if (processed == null) processed = new RenderTexture(sprite.width,sprite.height,1);
        Graphics.Blit(sprite,processed,ToolManager.Instance.ssi.material);
    }
}
