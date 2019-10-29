using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using LogViewerPlus.Enums;
using LogViewerPlus.Models;
using LogViewerPlus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LogViewerPlus.ViewModels
{
    public class LogsViewModel : ObservableObject
    {
        private LogsServices logsServices;
        private LogFile selectedLogFile;

        public RelayCommand RefreshCommand { get; set; }

        public RelayCommand ChangeFilterSelection { get; set; }

        public LogFile SelectedLogFile
        {
            get { return selectedLogFile; }
            set { Set(() => SelectedLogFile, ref selectedLogFile, value); }
        }

        public LogsViewModel()
        {
            this.logsServices = new LogsServices();

            this.ChangeFilterSelection = new RelayCommand(UpdateLogEntries);

            this.UpdateData();

        }

        private void UpdateData()
        {
            this.SelectedLogFile = new LogFile() { FilePath = System.Configuration.ConfigurationManager.AppSettings["LogsPath"] };

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

        private void RefreshLogs()
        {
            this.UpdateData();
        }
    }
}
