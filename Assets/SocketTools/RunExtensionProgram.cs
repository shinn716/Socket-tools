using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class RunExtensionProgram : MonoBehaviour
{
    private string configName = "ExtensionPath_v2.txt";

    [ContextMenu("GetAppName")]
    public string GetAppName()
    {
        try
        {
            string m_readPath = Path.Combine(Application.streamingAssetsPath, configName);
            string m_appname = File.ReadAllText(m_readPath);
            string[] splitArray = m_appname.Split(char.Parse("."));
            name = splitArray[0];
            return name;
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            return "0";
        }
    }

    public void RunExtensionApp()
    {
        string m_readPathq = Path.Combine(Application.streamingAssetsPath, configName);
        string filalPathq = Path.Combine(Environment.CurrentDirectory, File.ReadAllText(m_readPathq));
        print("RunExtensionProgram " + filalPathq);

        try
        {
            string m_readPath = Path.Combine(Application.streamingAssetsPath, configName);
            string filalPath = Path.Combine(Environment.CurrentDirectory, File.ReadAllText(m_readPath));
            WindowsEventAPI.SetWindowEvent(filalPath, WindowsEventAPI.WindowsStyle.Minimized);
            //print("RunExtensionProgram " + m_readPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}