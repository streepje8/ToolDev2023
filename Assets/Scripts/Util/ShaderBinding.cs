using UnityEngine;

public class ShaderBinding : MonoBehaviour
{
    public string propertyName;

    public (bool,T) RestoreLoadedValue<T>()
    {
        ShaderSetting<T>? loadedValue = ToolManager.Instance.ssi.GetSetting<T>(propertyName);
        if (loadedValue != null)
        {
            if (!ToolManager.Instance.ssi.SetSetting(propertyName, loadedValue.Value.value)) Debug.LogWarning("Shader property " + propertyName + " was not found!");
            return (true,loadedValue.Value.value);
        }
        return (false, default);
    }

    public void UpdateValue<T>(T val)
    {
        if(!ToolManager.Instance.ssi.SetSetting(propertyName,val)) Debug.LogWarning("Shader property " + propertyName + " was not found!");
    }

    public void PreviewValue<T>(T val)
    {
        if(!ToolManager.Instance.ssi.SetSetting(propertyName,val,false)) Debug.LogWarning("Shader property " + propertyName + " was not found!");
    }
}
