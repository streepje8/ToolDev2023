using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ProjectShaderSettingsManager : MonoBehaviour
{
    public Dictionary<int, string> currentSettings = new Dictionary<int, string>();
    public IShaderSettingSerializer serializer;
    public IShaderSettingDeserializer deserializer;

    private void Awake()
    {
        DefaultShaderSettingSerializerDeserializer serializerDeserializer = ScriptableObject.CreateInstance<DefaultShaderSettingSerializerDeserializer>();
        serializer ??= serializerDeserializer; deserializer ??= serializerDeserializer;
    }

    public void SetSetting<T>(int id, ShaderSetting<T> setting)
    {
        if (currentSettings.ContainsKey(id)) currentSettings.Remove(id);
        currentSettings.Add(id, serializer.Serialize(setting));
    }
    public ShaderSetting<T>? GetSetting<T>(int id)
    {
        if (currentSettings.TryGetValue(id, out string serializedSetting))
        {
            return deserializer.DeSerialize<T>(serializedSetting);
        }
        return null;
    }

    public string SerializeAllSettings()
    {
        return JsonConvert.SerializeObject(currentSettings,Formatting.Indented);
    }

    public void DeserializeAllSettings(string json)
    {
        currentSettings = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
    }
}

public interface IShaderSettingDeserializer
{
    public ShaderSetting<T> DeSerialize<T>(string serializedSetting);
}

public interface IShaderSettingSerializer
{
    public string Serialize<T>(ShaderSetting<T> setting);
}

[Serializable]
public struct ShaderSetting<T>
{
    public string name;
    public T value;

    public ShaderSetting(string name, T value)
    {
        this.name = name;
        this.value = value;
    }
}