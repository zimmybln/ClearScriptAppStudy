using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ClearScriptAppStudy.ViewModels;
using Prism.Mvvm;

namespace ClearScriptAppStudy.ScriptObjects
{
    public class Application 
    {
        public string StateInfo
        {
            get => ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).StateInfo;
            set
            {
                if (System.Windows.Application.Current.Dispatcher.CheckAccess())
                {
                    ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).StateInfo = value;
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() =>
                      {
                          ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).StateInfo = value;
                      }));
                }
            }
                
        }

        public string FieldInfo
        {
            get => ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).FieldInfo;
            set
            {
                if (System.Windows.Application.Current.Dispatcher.CheckAccess())
                {
                    ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).FieldInfo = value;
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() =>
                      {
                          ((MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext).FieldInfo = value;
                      }));
                }
            }
        }
    }
}
