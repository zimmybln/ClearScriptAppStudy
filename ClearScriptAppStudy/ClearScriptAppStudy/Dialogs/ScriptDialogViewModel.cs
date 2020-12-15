using ICSharpCode.AvalonEdit.Document;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClearScriptAppStudy.Dialogs
{
    public class ScriptDialogViewModel : BindableBase, IDialogAware
    {
        private ICommand dialogCommand;

        public string Title => "Script";

        public event Action<IDialogResult> RequestClose;

        public ICommand DialogCommand => dialogCommand ??= new DelegateCommand<string>(OnCloseDialogCommand);
        

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        private void OnCloseDialogCommand(string parameter)
        {
            DialogResult result = null;

            if (parameter?.ToLower() == "true")
            {
                result = new DialogResult(ButtonResult.OK);
                result.Parameters.Add(nameof(Script), Script.Text);
            }
            else 
            {
                result = new DialogResult(ButtonResult.Cancel);
            }

            RequestClose?.Invoke(result);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            string script = null;

            if (parameters?.ContainsKey(nameof(Script)) ?? false)
            {
                script = parameters.GetValue<string>(nameof(Script));
            }

            scriptDocument = new TextDocument() { Text = String.IsNullOrEmpty(script) ? String.Empty : script };
        }

        private TextDocument scriptDocument;

        public TextDocument Script
        {
            get => scriptDocument;
            set => SetProperty(ref scriptDocument, value);
        }
    }
}
