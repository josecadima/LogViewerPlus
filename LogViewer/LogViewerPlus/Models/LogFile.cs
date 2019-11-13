using GalaSoft.MvvmLight;
using LogViewerPlus.Enums;
using System;
using System.Collections.Generic;

namespace LogViewerPlus.Models
{
    public class LogFile : ObservableObject
    {
        private string logFileEntriesText;
        private List<LogFileEntry> logFileEntries;
        private List<LogFileEntry> filteredFileEntries;
        private LogFileEntry selectedLogEntry;

        private LogFilter filters;

        public string FileName { get; set; }

        public string UserName { get; set; }

        public string MachineName { get; set; }

        public string FilePath { get; set; }

        public string ShortDate { get; set; }

        public string SearchLine { get; set; }

        public string RDLogFilePath { get; set; }

        public List<LogFileEntry> LogFileEntries
        {
            get { return logFileEntries; }
            set
            {
                Set(() => LogFileEntries, ref logFileEntries, value);

                this.UpdateLogCount();
            }
        }

        public List<LogFileEntry> FilteredFileEntries
        {
            get { return filteredFileEntries; }
            set { Set(() => FilteredFileEntries, ref filteredFileEntries, value); }
        }

        public LogFileEntry SelectedLogEntry
        {
            get
            {
                return selectedLogEntry;
            }
            set
            {
                Set(() => SelectedLogEntry, ref selectedLogEntry, value);
            }
        }

        public String LogFileEntriesText
        {
            get { return logFileEntriesText; }
            set { Set(() => LogFileEntriesText, ref logFileEntriesText, value); }
        }

        public LogFilter Filters
        {
            get {return filters; }
            set { Set(() => Filters, ref filters, value); }
        }

        public LogFile()
        {
            this.Filters = new LogFilter();
        }

        public void UpdateLogCount()
        {
            int none = 0;
            int error = 0;
            int warning = 0;
            int info = 0;
            int debug = 0;

            foreach (var log in this.LogFileEntries)
            {
                switch(log.MessageType)
                {
                    case MessageType.None:
                        none++;
                        break;
                    case MessageType.Info:
                        info++;
                        break;
                    case MessageType.Debug:
                        debug++;
                        break;
                    case MessageType.Warning:
                        warning++;
                        break;
                    case MessageType.Error:
                        error++;
                        break;
                }
            }

            this.Filters.NoneCount = none;
            this.Filters.InfoCount = info;
            this.Filters.DebugCount = debug;
            this.Filters.WarningCount = warning;
            this.Filters.ErrorCount = error;
        }
    }
}