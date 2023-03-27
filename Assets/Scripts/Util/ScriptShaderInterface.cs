using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptShaderInterface : MonoBehaviour
{
    public Material material;
    public List<string> propertyNames = new List<string>();
    private Dictionary<string, int> propertyIDLookup = new Dictionary<string, int>();
    private ProjectShaderSettingsManager manager;
    private static readonly int ShaderModePropertyID = Shader.PropertyToID("_ShaderMode");

    public void SetShaderMode(int mode)
    {
        material.SetInt(ShaderModePropertyID, mode);
    }

    public void Initialize(Material template)
    {
        material = new Material(template);
        manager = GetComponent<ProjectShaderSettingsManager>();
        string[] names = propertyNames.ToArray();
        int[] ids = Array.Empty<int>();
        foreach (var propertyName in names)
        {
            ids = ids.Append(Shader.PropertyToID(propertyName)).ToArray();
        }
        propertyIDLookup = names.Zip(ids, (name, id) => new { name, id }).ToDictionary(item => item.name, item => item.id);
    }

    public bool HasSetting(string name) => propertyIDLookup.ContainsKey(name);

    public ShaderSetting<T>? GetSetting<T>(string name)
    {
        if (propertyIDLookup.TryGetValue(name, out int id))
        {
            return manager.GetSetting<T>(id);
        }
        return null;
    }

    public bool SetSetting<T>(string name, T value, bool updateProjectFile = true)
    {
        if (propertyIDLookup.TryGetValue(name, out int id))
        {
            switch (typeof(T))
            {
                case { } type when type == typeof(int):
                    material.SetInt(id, (int)Convert.ChangeType(value, typeof(int)));
                    break;
                case { } type when type == typeof(float):
                    material.SetFloat(id, (float)Convert.ChangeType(value,typeof(float)));
                    break;
                case { } type when type == typeof(Vector4):
                    material.SetVector(id, (Vector4)Convert.ChangeType(value, typeof(Vector4)));
                    break;
                case { } type when type == typeof(Vector3):
                    material.SetVector(id, (Vector4)Convert.ChangeType(value, typeof(Vector4)));
                    break;
                case { } type when type == typeof(Vector3):
                    material.SetVector(id, (Vector4)Convert.ChangeType(value, typeof(Vector4)));
                    break;
                case { } type when type == typeof(Color):
                    material.SetColor(id, (Color)Convert.ChangeType(id,typeof(Color)));
                    break;
            }
            if(updateProjectFile)manager.SetSetting(id, new ShaderSetting<T>(name,value));
            return true;
        }
        return false;
    }
}
