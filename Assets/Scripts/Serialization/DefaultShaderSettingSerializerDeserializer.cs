using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "New Shader Setting SerializerDeserializer", menuName = "Serialization/Default Shader SerializerDeserializer")]
public class DefaultShaderSettingSerializerDeserializer : ScriptableObject, IShaderSettingSerializer, IShaderSettingDeserializer
{
    public string Serialize<T>(ShaderSetting<T> setting)
    {
        return JsonConvert.SerializeObject(setting, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }

    public ShaderSetting<T> DeSerialize<T>(string serializedSetting)
    {
        return JsonConvert.DeserializeObject<ShaderSetting<T>>(serializedSetting);
    }
}