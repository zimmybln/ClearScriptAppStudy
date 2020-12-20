using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClearScriptAppStudy.ViewModels;
using Prism.Mvvm;

namespace ClearScriptAppStudy.ScriptObjects
{
    public class Application 
    {
        public string StateInfo
        {
            get => ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).StateInfo;
            set => ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).StateInfo = value;
        }
    }
}
