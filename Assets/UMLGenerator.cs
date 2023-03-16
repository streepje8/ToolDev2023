#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UMLGenerator : MonoBehaviour
{
    [MenuItem("streep/GenerateUML")]
    public static void GenerateUML()
    {
        string UMLCode = "@startuml\n";
        Assembly ass = typeof(ToolManager).Assembly;
        foreach (var type in ass.GetTypes())
        {
            if (type.IsClass)
            {
                if (type.IsAbstract)
                {
                    UMLCode += "abstract ";
                }

                string name = type.Name;
                if (type.IsGenericType) name = name.Replace("`1", "");
                if (!name.StartsWith("<"))
                {
                    UMLCode += type.IsStruct() ? "struct" : "class" + " " + name + "{\n";
                    UMLCode += "--Public Fields--\n";
                    foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        UMLCode += "+" + fieldInfo.Name + "\n";
                    }
                    UMLCode += "--Private Fields--\n";
                    foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        UMLCode += "~" + fieldInfo.Name + "\n";
                    }
                    UMLCode += "}\n";
                }
            }

            if (type.IsInterface)
            {
                string name = type.Name;
                if (type.IsGenericType) name = name.Replace("`1", "");
                if (!name.StartsWith("<"))
                {
                    UMLCode += "interface " + name + "{\n";
                    UMLCode += "--Public Fields--\n";
                    foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        UMLCode += "+" + fieldInfo.Name + "\n";
                    }
                    UMLCode += "--Private Fields--\n";
                    foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        UMLCode += "~" + fieldInfo.Name + "\n";
                    }
                    UMLCode += "}\n";
                }
            }
        }
        UMLCode += "\n@enduml";
        Debug.Log(UMLCode);
    }
}

#endif