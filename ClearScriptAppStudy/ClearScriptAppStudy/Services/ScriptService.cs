using ClearScriptAppStudy.Dialogs;
using ClearScriptAppStudy.Types;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Services
{
    public class ScriptService
    {
        private readonly IDialogService dialogService;
        private ApplicationScript applicationScript = null;

        public ScriptService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public ApplicationScript Script
        {
            get => applicationScript;
            set
            {
                applicationScript = value;

                InitScript(applicationScript);
            }
        }
                
        private void InitScript(ApplicationScript script)
        {

        }


        public void ShowScriptDialog()
        {
            DialogParameters dialogParameters = new DialogParameters();

            dialogService.ShowDialog(nameof(ScriptDialogView), dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey(nameof(Script)))
                {
                    Script = new ApplicationScript() { Script = result.Parameters.GetValue<string>(nameof(Script)) };
                }
            });
        }

    }
}
