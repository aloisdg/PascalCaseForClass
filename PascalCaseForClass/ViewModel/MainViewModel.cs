using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PascalCaseForClass.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand Copy { get; private set; }

        public MainViewModel()
        {
            Copy = new RelayCommand<string>(CopyExecute);
        }

        private static void CopyExecute(string s)
        {
            Clipboard.SetText(s);
        }
    }
}