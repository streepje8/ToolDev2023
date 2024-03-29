using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class CLI : Singleton<CLI>
{
    void Awake()
    {
        Instance = this;
    }
    
    public int RunConsoleCommand(string command, ProcessStartInfo startInfo)
    {
        Process process = new Process();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.FileName = command;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
        Debug.Log(process.StandardOutput.ReadToEnd());
        Debug.LogError(process.StandardError.ReadToEnd());
        return process.ExitCode;
    }

    public string NormalizeSlashes(string path)
    {
        return path.Replace("/", "\\");
    }
    
    //Gestolen van https://stackoverflow.com/questions/6521546/how-to-handle-spaces-in-file-path-if-the-folder-contains-the-space
    public string AddQuotesIfRequired(string path)
    {
        return !string.IsNullOrWhiteSpace(path) ? 
            path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ? 
                '"' + path + '"' : path : 
            string.Empty;
    }
}
