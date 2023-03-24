using UnityEngine;

public class KeybindingManager : MonoBehaviour
{
    void Update()
    {
        bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        #if UNITY_EDITOR
        control = true;
#endif
        if (control && Input.GetKeyDown(KeyCode.Z))
        {
            CommandManager.Instance.Undo();
        }
        if (control && Input.GetKeyDown(KeyCode.Y))
        {
            CommandManager.Instance.Redo();
        }
        if (control && Input.GetKeyDown(KeyCode.S))
        {
            ProjectManager.Instance.SaveCurrent();
        }
    }
}
