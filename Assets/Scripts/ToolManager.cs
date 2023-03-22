public class ToolManager : Singleton<ToolManager>
{
    public ScriptShaderInterface ssi;
    public SpriteProcessor processor;
    public Menu mainMenu;

    private ImageDisplayElement preview;
    private void Awake()
    {
        Instance = this;
        preview = mainMenu.GetMenuElementById<ImageDisplayElement>("preview");
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
