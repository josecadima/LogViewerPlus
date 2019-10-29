using GalaSoft.MvvmLight;
using LogViewerPlus.ViewModels;

namespace LogViewerPlus.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public LogsViewModel LogVM
        {
            get { return ViewModelLocator.Instance.LogVM; }
        }

        public MainViewModel()
        {
           
        }
    }
}