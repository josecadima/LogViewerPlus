using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LogViewerPlus.Enums;
using LogViewerPlus.Models;
using LogViewerPlus.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LogViewerPlus.ViewModels
{
    public class LogsViewModel : ObservableObject
    {
        private LogsServices logsServices;
        private LogFile selectedLogFile;
        private string serverName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
        private string remoteLogPath = System.Configuration.ConfigurationManager.AppSettings["RemoteLogPath"];
        private string localLogPath = System.Configuration.ConfigurationManager.AppSettings["LocallogsPath"];


        public RelayCommand RefreshCommand { get; set; }

        public RelayCommand ChangeFilterSelection { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public RelayCommand CopyCommand { get; set; }
        public LogFile SelectedLogFile
        {
            get { return selectedLogFile; }
            set { Set(() => SelectedLogFile, ref selectedLogFile, value); }
        }

        public LogsViewModel()
        {
            this.logsServices = new LogsServices();

            this.ChangeFilterSelection = new RelayCommand(UpdateLogEntries);
            this.RefreshCommand = new RelayCommand(RefreshLogEntries);
            this.SearchCommand = new RelayCommand(SearchLogs);
            this.CopyCommand = new RelayCommand(CopyLogsFile);

            this.UpdateData();

        }

        private void UpdateData()
        {
            this.SelectedLogFile = new LogFile() { FilePath = System.Configuration.ConfigurationManager.AppSettings["LocallogsPath"] };

            this.UpdateLogEntries();
        }

        public async void UpdateLogEntries()
        {
            List<LogFileEntry> filteredEntries = new List<LogFileEntry>();

            if (this.SelectedLogFile.LogFileEntries == null)
            {
                await Task.Factory.StartNew(() => this.SelectedLogFile.LogFileEntries = logsServices.GetLogFileEntries(this.SelectedLogFile.FilePath));
            }

            if (this.SelectedLogFile.LogFileEntries == null)
                return;

            this.SelectedLogFile.LogFileEntries = this.SelectedLogFile.LogFileEntries.OrderByDescending(e => e.Time).ToList();

            foreach (var entry in this.SelectedLogFile.LogFileEntries)
            {
                if (entry.MessageType == MessageType.None && this.SelectedLogFile.Filters.IsNoneSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Info && this.SelectedLogFile.Filters.IsInfoSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Debug && this.SelectedLogFile.Filters.IsDebugSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Warning && this.SelectedLogFile.Filters.IsWarningSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Error && this.SelectedLogFile.Filters.IsErrorSelected)
                    filteredEntries.Add(entry);
            }

            this.SelectedLogFile.FilteredFileEntries = filteredEntries;
        }

        public async void RefreshLogEntries()
        {
            List<LogFileEntry> filteredEntries = new List<LogFileEntry>();

                await Task.Factory.StartNew(() => this.SelectedLogFile.LogFileEntries = logsServices.GetLogFileEntries(this.SelectedLogFile.FilePath));

            if (this.SelectedLogFile.LogFileEntries == null)
                return;

            this.SelectedLogFile.LogFileEntries = this.SelectedLogFile.LogFileEntries.OrderByDescending(e => e.Time).ToList();

            foreach (var entry in this.SelectedLogFile.LogFileEntries)
            {
                if (entry.MessageType == MessageType.None && this.SelectedLogFile.Filters.IsNoneSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Info && this.SelectedLogFile.Filters.IsInfoSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Debug && this.SelectedLogFile.Filters.IsDebugSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Warning && this.SelectedLogFile.Filters.IsWarningSelected)
                    filteredEntries.Add(entry);
                else if (entry.MessageType == MessageType.Error && this.SelectedLogFile.Filters.IsErrorSelected)
                    filteredEntries.Add(entry);
            }

            this.SelectedLogFile.FilteredFileEntries = filteredEntries;
        }

        public async void SearchLogs()
        {
            List<LogFileEntry> searchedEntries = new List<LogFileEntry>();

            await Task.Factory.StartNew(() => this.SelectedLogFile.LogFileEntries = logsServices.SearchLogFileEntries(this.SelectedLogFile.FilePath, this.SelectedLogFile.SearchLine));

            if (this.SelectedLogFile.LogFileEntries == null)
                return;

            this.SelectedLogFile.LogFileEntries = this.SelectedLogFile.LogFileEntries.OrderByDescending(e => e.Time).ToList();

            foreach (var entry in this.SelectedLogFile.LogFileEntries)
            {
                if (entry.MessageType == MessageType.None && this.SelectedLogFile.Filters.IsNoneSelected)
                    searchedEntries.Add(entry);
                else if (entry.MessageType == MessageType.Info && this.SelectedLogFile.Filters.IsInfoSelected)
                    searchedEntries.Add(entry);
                else if (entry.MessageType == MessageType.Debug && this.SelectedLogFile.Filters.IsDebugSelected)
                    searchedEntries.Add(entry);
                else if (entry.MessageType == MessageType.Warning && this.SelectedLogFile.Filters.IsWarningSelected)
                    searchedEntries.Add(entry);
                else if (entry.MessageType == MessageType.Error && this.SelectedLogFile.Filters.IsErrorSelected)
                    searchedEntries.Add(entry);
            }

            this.SelectedLogFile.FilteredFileEntries = searchedEntries;

        }

        public void CopyLogsFile()
        {
            string sourcePath = String.Format(@"\\{0}\{1}", serverName, remoteLogPath);
            string targetPath = String.Format(@"{0}",localLogPath);

            try
            {
                if(Path.GetExtension(targetPath) == "")
                {
                    Path.Combine(targetPath, Path.GetFileName(sourcePath));
                }
                if(Directory.Exists(sourcePath))
                {
                    foreach (string sourceFilePath in Directory.GetFiles(sourcePath, "*Emerge*", SearchOption.AllDirectories))
                    {
                        File.Copy(sourceFilePath, sourceFilePath.Replace(sourcePath,targetPath), true);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
