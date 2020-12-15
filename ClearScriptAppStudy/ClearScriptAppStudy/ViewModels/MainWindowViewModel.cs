using ClearScriptAppStudy.Services;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClearScriptAppStudy.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerProvider container;
        private readonly IDialogService dialogService;
        private ICommand showScriptDialogCommand;

        public MainWindowViewModel(IContainerProvider container,
                IDialogService dialogService)
        {
            this.container = container;
            this.dialogService = dialogService;
        }


        public ICommand ShowScriptDialogCommand => 
            showScriptDialogCommand ??= new DelegateCommand(OnShowScriptDialog);


        private void OnShowScriptDialog()
        {
            var scriptService = container.Resolve<ScriptService>();


            scriptService.ShowScriptDialog();
        }
    }
}
