using ClearScriptAppStudy.Dialogs;
using ClearScriptAppStudy.Types;
using Microsoft.ClearScript.V8;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Services
{
    public class ScriptService : IDisposable
    {
        private readonly IDialogService dialogService;
        private ApplicationScript applicationScript = null;
        private V8ScriptEngine scriptEngine = null;
        private bool disposedValue;

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
                
        private void InitScript(ApplicationScript scriptSettings)
        {
            scriptEngine = new V8ScriptEngine();

            if (!String.IsNullOrEmpty(scriptSettings.Script))
            {

                try
                {


                    //scriptEngine.AddHostObject("Questions", questionDictionary);
                    scriptEngine.AddHostObject("Console", new ClearScriptAppStudy.ScriptObjects.Console(OnWriteLine));

                    //scriptEngine.AddHostType(typeof(QuestionTextContent));

                    scriptEngine.Execute(scriptSettings.Script);

                    // AddDebugMessage("Die Skriptausführungs wurde erfolgreich erstellt");

                    if (new List<string>(scriptEngine.Script.PropertyNames).Contains("Initialized"))
                    {
                        scriptEngine.Script.Initialized();
                    }
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                // AddDebugMessage("Es wurde keine Umgebung für die Skriptausführung erstellt");
            }
        }


        public void ShowScriptDialog()
        {
            DialogParameters dialogParameters = new DialogParameters();

            dialogParameters.Add(nameof(Script), Script.Script);

            dialogService.ShowDialog(nameof(ScriptDialogView), dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey(nameof(Script)))
                {
                    Script = new ApplicationScript() { Script = result.Parameters.GetValue<string>(nameof(Script)) };
                }
            });
        }

        private void OnWriteLine(string format)
        {
            Debug.WriteLine(format);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    scriptEngine?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ScriptService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
