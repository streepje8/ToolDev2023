using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class ProjectManager : Singleton<ProjectManager>
{
    private RuntimeProject openProject;
    public ProjectShaderSettingsManager shaderSettingsManager;
    public MenuManager menuManager;
    public Menu newProjectMenu;
    public bool projectIsOpen;
    public Transform saveAsModal;
    public TMP_InputField saveAsModalInput;

    public Action projectRefreshEvent;
    private static readonly int ShaderModePropertyID = Shader.PropertyToID("_ShaderMode");

    private void Awake()
    {
        Instance = this;
        projectIsOpen = false;
    }

    public void CreateProjectWithUI()
    {
        StringInputElement nameInput = newProjectMenu.GetMenuElementById<StringInputElement>("ProjectNameInput");
        FileInputElement fileInput = newProjectMenu.GetMenuElementById<FileInputElement>("FilePicker");
        CreateProject(nameInput.InputField.text,fileInput.FilePath);
    }

    public List<string> GetProjectPaths()
    {
        List<string> foundProjects = new List<string>();
        if(!Directory.Exists(Application.persistentDataPath + "/projects")) Directory.CreateDirectory(Application.persistentDataPath + "/projects");
        foreach (string directory in Directory.GetDirectories(Application.persistentDataPath + "/projects"))
        {
            if (File.Exists(directory.Replace("\\", "/") + "/projectData.json"))
            {
                foundProjects.Add(directory.Replace("\\", "/"));
            }
        }
        return foundProjects;
    }
    
    public List<string> GetProjectNames()
    {
        List<string> foundProjects = new List<string>();
        if(!Directory.Exists(Application.persistentDataPath + "/projects")) Directory.CreateDirectory(Application.persistentDataPath + "/projects");
        foreach (string directory in Directory.GetDirectories(Application.persistentDataPath + "/projects"))
        {
            if (File.Exists(Application.persistentDataPath + "/projects/" + directory + "/projectData.json"))
            {
                Project p = GetProject(Application.persistentDataPath + "/projects/" + directory);
                foundProjects.Add(p.name);
            }
        }
        return foundProjects;
    }

    public Project GetProject(string projectDirectory)
    {
        Project result = new Project() { name = "nullProject", projectDirectory = projectDirectory, originalSpritePath = null};
        if (File.Exists(projectDirectory + "/projectData.json"))
        {
            string projectJson = File.ReadAllText(projectDirectory + "/projectData.json");
            result = JsonConvert.DeserializeObject<Project>(projectJson);
        }
        return result;
    }

    public Project CreateProject(string name, string spriteFilePath, bool openProject = true)
    {
        //Sanitize the user name input
        name = Regex.Replace(name,@"[\s]", "_");
        name = Regex.Replace(name, @"[^a-zA-Z_]+|[^\w\s]|(?<=\s)\s+|\s+(?=\s|$)", ""); //Generated by ChatGPT because I barely know how regex works

        //Make sure the projects folder exists
        if(!Directory.Exists(Application.persistentDataPath + "/projects")) Directory.CreateDirectory(Application.persistentDataPath + "/projects"); 
        
        //Check if the project itself already exists
        int i = 1;
        string processedName = name;
        while (i < 100 && Directory.Exists(Application.persistentDataPath + "/projects/" + processedName))
        {
            processedName = name + "_" + i;
            i++;
        } 
        
        //Actually create the project
        Debug.Log("Creating project: " + processedName + " with file: " + spriteFilePath);
        Project project = new Project()
        {
            name = processedName,
            projectDirectory = Application.persistentDataPath + "/projects/" + processedName,
        };
        project.shaderSettings = shaderSettingsManager.SerializeAllSettings();
        Directory.CreateDirectory(project.projectDirectory);
        project.originalSpritePath = project.projectDirectory + "/ModelAndAnimations.gltf";
        File.Copy(spriteFilePath, project.originalSpritePath);
        string projectDataJson = JsonConvert.SerializeObject(project, Formatting.Indented);
        File.WriteAllText(project.projectDirectory + "/projectData.json",projectDataJson);
        Debug.Log("Project created successfully at " + project.projectDirectory + "! Opening project...");
        if(openProject)OpenProject(project);
        return project;

        // Old Code
        // string exe = "cmd.exe";
        // Debug.Log("WOO: " + CLI.Instance.NormalizeSlashes(CLI.Instance.AddQuotesIfRequired(Application.streamingAssetsPath + "/FBX2glTF.exe")));
        // ///CLI.Instance.NormalizeSlashes(CLI.Instance.AddQuotesIfRequired(Application.streamingAssetsPath + "/FBX2glTF.exe"))
        // CLI.Instance.RunConsoleCommand(exe, new ProcessStartInfo()
        // {
        //     ArgumentList =
        //     {
        //         "/c",
        //         CLI.Instance.NormalizeSlashes(CLI.Instance.AddQuotesIfRequired(Application.streamingAssetsPath + "/FBX2glTF.exe")),
        //         "--input",
        //         CLI.Instance.NormalizeSlashes(CLI.Instance.AddQuotesIfRequired(fbxFilePath)),
        //         "--output",
        //         CLI.Instance.AddQuotesIfRequired(CLI.Instance.NormalizeSlashes(project.modelPlusAnimationsFilePath))
        //     }
        // });
    }

    public void SaveCurrent()
    {
        if (projectIsOpen)
        {
            openProject.serializedProject.shaderSettings = shaderSettingsManager.SerializeAllSettings();
            string projectDataJson = JsonConvert.SerializeObject(openProject.serializedProject, Formatting.Indented);
            File.WriteAllText(openProject.serializedProject.projectDirectory + "/projectData.json", projectDataJson);
        }
    }
    
    public void SaveAs()
    {
        string newname = saveAsModalInput.text;
        SaveCurrent();
        string otherShaderSettings = openProject.serializedProject.shaderSettings;
        Project p = CreateProject(newname, openProject.serializedProject.originalSpritePath);
        p.shaderSettings = otherShaderSettings;
        saveAsModal.gameObject.SetActive(false);
        projectRefreshEvent.Invoke();
    }

    public void ExportCurrent(int mode)
    {
        if(!Export(openProject, mode)) Debug.LogWarning("The exporting of one or more projects failed!");
    }

    public bool Export(RuntimeProject p, int mode)
    {
        string filepath = "";
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "Sprites \0*.png\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = "%USERPROFILE%\\Desktop";
        ofn.title = "Select where to save an png file with the warped sprite.";
        ofn.defExt = "PNG";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST|OFN_NOCHANGEDIR
        if(FileInputElement.GetSaveFileName(ofn))
        {
            filepath = ofn.file;
        }
        else
        {
            filepath = null;
        }

        if (filepath != null)
        {
            RenderTexture temp = new RenderTexture(p.sprite.width, p.sprite.height, 1);
            ToolManager.Instance.ssi.material.SetInt(ShaderModePropertyID,mode);
            Graphics.Blit(p.sprite, temp, ToolManager.Instance.ssi.material);
            Texture2D result = new Texture2D(p.sprite.width, p.sprite.height);
            RenderTexture.active = temp;
            result.ReadPixels(new Rect(0, 0, p.sprite.width, p.sprite.height), 0, 0);
            result.Apply();
            RenderTexture.active = null;
            byte[] bytes = result.EncodeToPNG();
            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? throw new InvalidOperationException());
            }

            File.WriteAllBytes(filepath, bytes);
            return true;
        }
        return false;
    }

    public void OpenProject(Project p)
    {
        if (p.shaderSettings is { Length: > 0 })
        {
            shaderSettingsManager.DeserializeAllSettings(p.shaderSettings);
        }
        openProject = new RuntimeProject(p);
        projectIsOpen = true;
        menuManager.SwitchMenu(2);
    }

    public RuntimeProject GetOpenProject()
    {
        return openProject;
    }
}
