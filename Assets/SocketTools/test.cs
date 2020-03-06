using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Shinn.Common;
using System.Threading;

public class test : MonoBehaviour
{
    public bool enableAutoRunBridge;
    public SenderInterface sender;
    public ReceiverInterface receiver;
    public RunExtensionProgram extensionProgram;

    public InputField inputData;
    Console console;

    void Start()
    {
        console = GetComponent<Console>();
        console.SizeOffset = new Rect(0, 50, 0, -50);
        console.Show = true;
        receiver.SetConnect = true;

        if(enableAutoRunBridge)
            extensionProgram.RunExtensionApp();

        inputData.text = "800101";
    }

    public void Send()
    {
        sender.SendData(inputData.text);
    }

    private void OnApplicationQuit()
    {
        sender.SendData("8099");
        sender.Dispose();
    }
}
