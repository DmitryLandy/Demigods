using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GobanTestScript : MonoBehaviour
{

    public Text coordinateText;
    private static StringBuilder output = new StringBuilder();
    
    public void GenMove()
    {
        
        var command = $"{Application.dataPath}/katago/katago.exe";//gtp - model g170-b30c320x2-s4824661760-d1229536699.bin.gz
        
        var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
               coordinateText.text = $"\n line: {e.Data}";

            }
        });

        process.Start();
        process.BeginOutputReadLine();
        process.WaitForExit();
        
    }
}
