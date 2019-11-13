using log4net;
using LogViewerPlus.Enums;
using LogViewerPlus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LogViewerPlus.Services
{
    public class LogsServices
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogsServices));

        private List<DateTime> range;

        public string DaysForDate { get; set; }

        public LogsServices()
        {
            DaysForDate = System.Configuration.ConfigurationManager.AppSettings["DaysForDate"].ToString();

            range = new List<DateTime>();
            range.Add(DateTime.UtcNow.AddDays(Convert.ToInt32(DaysForDate) * -1));
            range.Add(DateTime.UtcNow);
        }

        public List<LogFileEntry> GetLogFileEntries(string logPath)
        {
            var logEntries = new List<LogFileEntry>();

            try
            {
                Log.DebugFormat("Getting logs from {0}.", logPath);

                string text = this.ReadFile(logPath, range);

                string[] lines = Regex.Split(text, @"(?<!\d)(?=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},[\d{5|\d{4}|\d{3}}])");

                for (int l = lines.Length -1; l >= 0; l--)
                {
                    if (string.IsNullOrEmpty(lines[l]) || lines[l].Length <= 30)
                        continue;

                    var regex = new Regex("\r\n");
                    var formatedLine = regex.Replace(lines[l], string.Empty, 1);

                    LogFileEntry logEntry;

                    if (this.MatchMessage(formatedLine, out logEntry))
                    {
                        if (logEntry.Time >= range[0] && logEntry.Time <= range[1])
                            logEntries.Add(logEntry);
                    }
                    else
                    {
                        DateTime time;
                        if (DateTime.TryParse(formatedLine.Substring(0, 19), out time))
                            time = time.ToLocalTime();

                        logEntries.Add(new LogFileEntry() { Message = formatedLine, ShortMessage = formatedLine, MessageType = MessageType.None, Time = time });

                    }
                }

                Log.Info("Success getting logs.");
            }
            catch (Exception e)
            {
                Log.Error("Failed getting logs.", e);
            }

            return logEntries;

        }

        private bool MatchMessage(string line, out LogFileEntry entry)
        {
            entry = new LogFileEntry();

            var regex = new Regex(@"(.*) \[(ERROR|WARN|DEBUG|INFO)\] ([a-z,A-Z,0-9,:,;,_,(,),@,�,&,/,!,\-,\*,\\,',\s]*)\.(.*)");

            Match match = regex.Match(line);
            if (!match.Success || match.Groups.Count < 5)
                return false;

            if (match.Groups[2].Value == "ERROR")
                entry.MessageType = MessageType.Error;
            else if (match.Groups[2].Value == "DEBUG")
                entry.MessageType = MessageType.Debug;
            else if (match.Groups[2].Value == "WARN")
                entry.MessageType = MessageType.Warning;
            else if (match.Groups[2].Value == "INFO")
                entry.MessageType = MessageType.Info;

            entry.Time = DateTime.Parse(match.Groups[1].Value.Substring(0, 19)).ToLocalTime();
            entry.ShortTime = entry.Time.ToString("yyyy-MM-dd HH:mm");
            entry.ShortMessage = match.Groups[3].Value;
            entry.Message = line;

            return true;
        }

        private string ReadFile(string logPath, List<DateTime> range)
        {
            StringBuilder builder = new StringBuilder();

            foreach(var file in this.GetFilesInRange(logPath,range))
            {
                using (Stream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        builder.Append(streamReader.ReadToEnd());
                    }
                }
            }

            return builder.ToString();
        }

        private List<string> GetFilesInRange(string logPath, List<DateTime> range)
        {
            List<string> files = new List<string>();

            if (!File.Exists(logPath))
                return files;

            int count = 1;

            DateTime currentFrom = DateTime.Now;
            DateTime currentTo;

            string currentFile = logPath;
            string nextFile = string.Format("{0}.{1}", logPath, count);

            while (File.Exists(currentFile))
            {
                currentTo = currentFrom;
                currentFrom = File.Exists(nextFile) ? File.GetLastWriteTime(nextFile) : DateTime.MinValue;

                if (currentFrom < range[0] && range[0] < currentTo)
                    files.Add(currentFile);
                else if (currentFrom < range[1] && range[1] < currentTo)
                    files.Add(currentFile);
                else if (currentFrom < range[0] && range[1] < currentTo)
                    files.Add(currentFile);
                else if (currentFrom > range[0] && range[1] > currentTo)
                    files.Add(currentFile);

                count++;
                currentFile = nextFile;
                nextFile = string.Format("{0}.{1}", logPath, count);
            }

            return files;
        }
        public List<LogFileEntry> SearchLogFileEntries(string logPath, string line)
        {
            var logEntries = new List<LogFileEntry>();

            try
            {
                Log.DebugFormat("Searching logs from {0}.", logPath);

                string text = this.ReadFile(logPath, range);

                string[] lines = Regex.Split(text, @"(?<!\d)(?=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},[\d{5|\d{4}|\d{3}}])");

                for (int l = lines.Length - 1; l >= 0; l--)
                {
                    if (string.IsNullOrEmpty(lines[l]) || lines[l].Length <= 30)
                        continue;

                    var regex = new Regex("\r\n");
                    var formatedLine = regex.Replace(lines[l], string.Empty, 1);

                    LogFileEntry logEntry;

                    if (this.MatchMessage(formatedLine, out logEntry))
                    {
                        if (formatedLine.ToString().Contains(line) || formatedLine.ToLower().Contains(line))
                            logEntries.Add(logEntry);
                    }
                }

                Log.Info("Success searching logs.");
            }
            catch (Exception e)
            {
                Log.Error("Failed serching logs.", e);
            }

            return logEntries;
        }
    }
   
}
