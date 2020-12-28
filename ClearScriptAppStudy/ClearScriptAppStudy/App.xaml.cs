using ClearScriptAppStudy.Components;
using ClearScriptAppStudy.Dialogs;
using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;
using ClearScriptAppStudy.ViewModels;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            var settingsScript = new SettingsManager<ApplicationScript>("ClearScriptAppStudy.json");

            this.Container.Resolve<ScriptService>().Script = settingsScript.LoadSettings();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .Register(typeof(MainWindow))
                .Register(typeof(MainWindowViewModel));

            // Dienste registrieren
            containerRegistry
                .RegisterSingleton(typeof(ScriptService));
            
            // Dialoge registrieren
            containerRegistry
                .RegisterDialog<ScriptDialogView, ScriptDialogViewModel>();

            containerRegistry
                .RegisterDialogWindow<DialogWindow>();

        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);

            App.Current.MainWindow = shell;
            App.Current.MainWindow.Show();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {

            var settingsManager = new SettingsManager<ApplicationScript>("ClearScriptAppStudy.json");

            settingsManager.SaveSettings(this.Container.Resolve<ScriptService>().Script);

            base.OnExit(e);
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }
    }
}
