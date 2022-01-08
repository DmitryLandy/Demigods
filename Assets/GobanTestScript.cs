using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GobanTestScript : MonoBehaviour
{

    public Text coordinateText;
    private static int lineCount = 0;
    private static StringBuilder output = new StringBuilder();
    private static StringBuilder errorOutput = new StringBuilder();
    public Process process;

    public void GenMove()
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

            process.StartInfo = startInfo;

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(e.Data))
                {
                    lineCount++;
                    output.Append("\n[" + lineCount + "]: " + e.Data);
                }
            });

            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                // Prepend line numbers to each line of the output.
                if (!String.IsNullOrEmpty(e.Data))
                {
                    lineCount++;
                    errorOutput.Append("\n[" + lineCount + "]: " + e.Data);
                }
            });
            
            process.Start();
            process.WaitForExit(); //matters where this is placed for proper execution


            // Asynchronously read the standard output/Error of the spawned process.
            // This raises OutputDataReceived events for each line of output.
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            //StreamWriter sw = process.StandardInput;
            //sw.WriteLine($"gtp -model {Application.dataPath}/katago/b20.bin.gz");
            //sw.WriteLine("genmove B");

            UnityEngine.Debug.Log(output);
            UnityEngine.Debug.Log(errorOutput);
            //process.StandardInput.WriteLine("gtp - model g170-b30c320x2-s4824661760-d1229536699.bin.gz");
            

        }


    }
}
