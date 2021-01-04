using ClearScriptAppStudy.Components;
using ClearScriptAppStudy.Dialogs;
using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;
using ClearScriptAppStudy.ViewModels;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Mvvm;

namespace ClearScriptAppStudy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(OnTextBoxGotFocus));
            
            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            
            
            containerRegistry
                .Register(typeof(MainWindow))
                .Register(typeof(MainWindowViewModel));

            // Dienste registrieren
            containerRegistry
                .RegisterSingleton(typeof(ScriptService))
                .Register<IScriptDialogs>(container => container.Resolve<ScriptService>())
                .Register<IPersonMethods>(container => container.Resolve<ScriptService>())
                .Register<IFieldMethods>(container => container.Resolve<ScriptService>());
                
            // Dialoge registrieren
            containerRegistry
                .RegisterDialog<ScriptDialogView, ScriptDialogViewModel>();

            containerRegistry
                .RegisterDialogWindow<DialogWindow>();

        }

        protected override Window CreateShell()
        {
            var window = this.Container.Resolve<MainWindow>();

            window.DataContext = Container.Resolve(typeof(MainWindowViewModel));

            return window;

        }

        protected override void InitializeShell(Window shell)
        {
            var settingsScript = new SettingsManager<ApplicationScript>("ClearScriptAppStudy.json");

            this.Container.Resolve<IScriptDialogs>().Script = settingsScript.LoadSettings();
            
            base.InitializeShell(shell);

            App.Current.MainWindow = shell;
            App.Current.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {

            var settingsManager = new SettingsManager<ApplicationScript>("ClearScriptAppStudy.json");

            settingsManager.SaveSettings(this.Container.Resolve<IScriptDialogs>().Script);

            base.OnExit(e);
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }
    }
}
