using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ToolManager : Singleton<ToolManager>
{
    public ScriptShaderInterface ssi;
    public SpriteProcessor processor;
    public Menu mainMenu;
    
    public Transform exitModal;

    public bool projectIsSaved = true;

    private bool skipSave = false; 
    
    private ImageDisplayElement preview;
    private void Awake()
    {
        Instance = this;
        preview = mainMenu.GetMenuElementById<ImageDisplayElement>("preview");
        Application.wantsToQuit += () =>
        {
            Debug.Log("Project is saved: " + projectIsSaved);
            if (!projectIsSaved && !skipSave)
            {
                exitModal.gameObject.SetActive(true);
                return false;
            }
            else
            {
                return true;
            }
        };
    }

    public void CloseProject()
    {
        ProjectManager.Instance.SaveCurrent();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void New()
    {
        ProjectManager.Instance.SaveCurrent();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UnityAction<Scene,LoadSceneMode> action = (arg0, mode) => { };
        action += (arg0, mode) =>
        {
            ProjectManager.Instance.menuManager.SwitchMenu(1);
            SceneManager.sceneLoaded -= action;
        };
        SceneManager.sceneLoaded += action;
    }

    public void Exit()
    {
        ProjectManager.Instance.SaveCurrent();
        Application.Quit();
    }

    public void ExitNoSave()
    {
        skipSave = true;
        Application.Quit();
    }

    private void Update()
    {
        if (ProjectManager.Instance.projectIsOpen)
        {
            processor.ProcessImage(ProjectManager.Instance.GetOpenProject().sprite);
            if (preview.image.texture == null) preview.image.texture = processor.processed;
        }
    }
}
