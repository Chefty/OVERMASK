#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ServerLauncher : EditorWindow
{
    [MenuItem("Tools/Start Server")]
    public static void StartServer()
    {
        var serverPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../server"));

        var startInfo = new ProcessStartInfo();

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/k cd /d \"{serverPath}\" && node main.js";
            
            startInfo.UseShellExecute = true; // Important: Allows launching external processes
            startInfo.CreateNoWindow = false; // Show the terminal window (use true to hide)
        }
        else
        {
            startInfo.FileName = "osascript";
            startInfo.Arguments = $"-e 'tell application \"Terminal\" to do script \"cd ''{serverPath}'' && node main.js\"'";
            
            
            startInfo.UseShellExecute = false; // Important: Allows launching external processes
            startInfo.CreateNoWindow = true; // Show the terminal window (use true to hide)
        }


        Process.Start(startInfo);
    }
}
#endif