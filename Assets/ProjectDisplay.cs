using UnityEngine;

public class ProjectDisplay : MonoBehaviour
{
    public ProjectManager manager;
    public GameObject templateProject;
    void Start()
    {
        Debug.Log("Getting all projects.");
        foreach (string projectPath in manager.GetProjectPaths())
        {
            Project p = manager.GetProject(projectPath);
            ProjectButton button = Instantiate(templateProject).GetComponent<ProjectButton>();
            button.transform.SetParent(transform);
            button.transform.localScale = Vector3.one;
            button.transform.localPosition = Vector3.zero;
            button.label.text = p.name;
            button.button.onClick.AddListener(() => manager.OpenProject(p));
            button.gameObject.SetActive(true);
        }
    }
}
