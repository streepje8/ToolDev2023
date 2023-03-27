using UnityEngine.Device;
using UnityEngine.Windows;

public class ToolManager : Singleton<ToolManager>
{
    public ScriptShaderInterface ssi;
    public SpriteProcessor processor;
    public Menu mainMenu;

    private bool performsave = true; 
    
    private ImageDisplayElement preview;
    private void Awake()
    {
        Instance = this;
        preview = mainMenu.GetMenuElementById<ImageDisplayElement>("preview");
        Application.quitting += () =>
        {
            if(performsave)ProjectManager.Instance.SaveCurrent();
        };
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitNoSave()
    {
        performsave = false;
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
