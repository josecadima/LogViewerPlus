using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewerPlus.Models
{
    public class LogFilter : ObservableObject
    {
        private int noneCount;
        private int debugCount;
        private int warningCount;
        private int infoCount;
        private int errorCount;

        private bool isNoneSelected = true;
        private bool isDebugSelected = true;
        private bool isWarningSelected = true;
        private bool isInfoSelected = true;
        private bool isErrorSelected = true;

        public int NoneCount
        {
            get { return noneCount; }
            set { Set(() => NoneCount, ref noneCount, value); }
        }

        public int DebugCount
        {
            get { return debugCount; }
            set { Set(() => DebugCount, ref debugCount, value); }
        }
        public int WarningCount
        {
            get { return noneCount; }
            set { Set(() => WarningCount, ref warningCount, value); }
        }
        public int InfoCount
        {
            get { return noneCount; }
            set { Set(() => InfoCount, ref infoCount, value); }
        }
        public int ErrorCount
        {
            get { return errorCount; }
            set { Set(() => ErrorCount, ref errorCount, value); }
        }

        public bool IsNoneSelected
        {
            get { return isNoneSelected; }
            set { Set(() => IsNoneSelected, ref isNoneSelected, value); }
        }

        public bool IsDebugSelected
        {
            get { return isDebugSelected; }
            set { Set(() => IsDebugSelected, ref isDebugSelected, value); }
        }

        public bool IsWarningSelected
        {
            get { return isNoneSelected; }
            set { Set(() => IsWarningSelected, ref isWarningSelected, value); }
        }

        public bool IsInfoSelected
        {
            get { return isNoneSelected; }
            set { Set(() => IsInfoSelected, ref isInfoSelected, value); }
        }

        public bool IsErrorSelected
        {
            get { return isErrorSelected; }
            set { Set(() => IsErrorSelected, ref isErrorSelected, value); }
        }

       public LogFilter()
        {
        }
    }
}
