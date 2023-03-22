using UnityEngine;

public class ShaderBinding : MonoBehaviour
{
    public string propertyName;

    public void UpdateValue<T>(T val)
    {
        if(!ToolManager.Instance.ssi.SetSetting(propertyName,val)) Debug.LogWarning("Shader property " + propertyName + " was not found!");
    }

    public void PreviewValue<T>(T val)
    {
        if(!ToolManager.Instance.ssi.SetSetting(propertyName,val,false)) Debug.LogWarning("Shader property " + propertyName + " was not found!");
    }
}
