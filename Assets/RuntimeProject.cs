using System.IO;
using UnityEngine;

public class RuntimeProject
{
    public Texture2D sprite;
    public Project serializedProject;
    public RuntimeProject(Project serializedProject)
    {
        byte[] fileData = File.ReadAllBytes(serializedProject.originalSpritePath);
        sprite = new Texture2D(2, 2);
        sprite.LoadImage(fileData);
        this.serializedProject = serializedProject;
    }
}
