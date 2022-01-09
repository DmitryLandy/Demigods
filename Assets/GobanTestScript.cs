using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GobanTestScript : MonoBehaviour
{

    public Text coordinateText;
    private static int lineCount = 0;
    private static StringBuilder errorOutput = new StringBuilder();
    public Process process;

    private void StartKatagoProcess()
    {
        using (Process process = new Process())
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = $"{Application.dataPath}/katago/katago.exe";
            startInfo.Arguments = $"gtp -model {Application.dataPath}/katago/b20.bin.gz";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            process.StartInfo = startInfo;

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                var pattern = "^= ([A-S]([1-9]|1[1-9]))$";
                if (!String.IsNullOrEmpty(e.Data))
                {
                    //if (Regex.IsMatch(e.Data, pattern)) coordinateText.text = e.Data; // CAUSES HUNG PROCESS! needs async?
                    UnityEngine.Debug.Log("\n[" + ++lineCount + "]: " + e.Data);
                }
            });

            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(e.Data))
                {
                    errorOutput.Append("\n[" + ++lineCount + "]: " + e.Data);
                }
            });


            process.Start();

            // Asynchronously read the standard output/Error of the spawned process.
            // This raises OutputDataReceived events for each line of output.
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using (StreamWriter myStreamWriter = process.StandardInput)
            {
                myStreamWriter.WriteLine("genmove B");
            }

            process.WaitForExit();
            //Output any error and stdout
            UnityEngine.Debug.Log(errorOutput);

        }


    }

    public void GenMove()
    {
        StartKatagoProcess();
    }
}
