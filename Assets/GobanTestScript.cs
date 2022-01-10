using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GobanTestScript : MonoBehaviour
{

    public Text coordinateText;
    public Process process = new Process();
    private static StringBuilder errorOutput = new StringBuilder();
    private static StringBuilder stdOutput = new StringBuilder();
    private static StreamWriter myStreamWriter;
    

    private void Start()
    {
        // Setup Start info for process
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = $"{Application.dataPath}/katago/katago.exe",
            Arguments = $"gtp -model {Application.dataPath}/katago/b20.bin.gz",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        process.StartInfo = startInfo;
    }
    private void CheckRegex(string data)
    {
        var pattern = "^= ([A-S]([1-9]|1[0-9]))$";
        if (Regex.IsMatch(data, pattern)) UnityEngine.Debug.Log("MATCHED!");
    }

    public async void GenMove()
    {
        process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                CheckRegex(e.Data);
                stdOutput.Append($"\n{e.Data}");
            }
        });

        process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!String.IsNullOrEmpty(e.Data)) errorOutput.Append($"\n{e.Data}");
        });

        await Task.Run(() =>
        {
            Process[] katagoProcess = Process.GetProcessesByName("katago.exe");
            if(katagoProcess.Length==0)process.Start();
            // Asynchronously read the standard output/Error of the spawned process.
            // This raises OutputDataReceived events for each line of output.
            
        });
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        UnityEngine.Debug.Log(process.HasExited);
        myStreamWriter = process.StandardInput;

        await Task.Run(() =>
        {
            
            myStreamWriter.WriteLine("genmove B");
            
        });

        myStreamWriter.WriteLine("quit");

        //process.WaitForExit();
        //Output any error and stdout
        UnityEngine.Debug.Log(errorOutput);
        UnityEngine.Debug.Log(stdOutput);
        process.Dispose();
    }
}
