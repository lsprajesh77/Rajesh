using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WebAPI.Controllers.Common
{
    public class Processes
    {
        
    private static StringBuilder outputString = null;
    private static int numOutputLines = 0;

    public static StringBuilder GetProcessResult(string input = null)
        {
            // Initialize the process and its StartInfo properties.
            // The sort command is a console application that
            // reads and sorts text input.

            Process proc = new Process();
            proc.StartInfo.FileName = @"D:\HTC\POC\HelloWorldConApp\bin\Debug\netcoreapp3.1\HelloWorldConApp.exe";
            proc.StartInfo.Arguments = @""+input+"";

            // Set UseShellExecute to false for redirection.
            proc.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the sort command.
            // This stream is read asynchronously using an event handler.
            proc.StartInfo.RedirectStandardOutput = true;
            outputString = new StringBuilder();

            // Set our event handler to asynchronously read the sort output.
            proc.OutputDataReceived += OutputHandler;

            // Redirect standard input as well.  This stream
            // is used synchronously.
            proc.StartInfo.RedirectStandardInput = true;

            // Start the process.
            proc.Start();

            // Start the asynchronous read of the sort output stream.
            proc.BeginOutputReadLine();

            // Wait for the exe to complete.
            proc.WaitForExit();

            proc.Close();

            return outputString;
        }

    private static void OutputHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            // Collect the sort command output.
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                numOutputLines++;

                // Add the text to the collected output.
                outputString.Append(Environment.NewLine +
                    $"[{numOutputLines}] - {outLine.Data}");
            }
        }
    }
}