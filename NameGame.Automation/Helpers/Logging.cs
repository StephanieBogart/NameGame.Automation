using System;
using System.IO;

namespace NameGame.Automation.Helpers
{
    class Logging
    {
        private static readonly string Filepath = @"C:\AutomationLogs\";

        public static void Log(string MessageToLog)
        {
            var LogMessage = $"{MakeTimeStamp()} - {MessageToLog} {Environment.NewLine}";
            Directory.CreateDirectory($@"{Filepath}");
            File.AppendAllText($@"{Filepath}\NameGameAutomation_Log.txt", LogMessage);
        }

        public static string MakeTimeStamp()
        {
            var stamp = DateTime.Now.ToString("dd-MMMM-yyy_HH-mm-ss");
            return stamp;            
        }
    }
}
