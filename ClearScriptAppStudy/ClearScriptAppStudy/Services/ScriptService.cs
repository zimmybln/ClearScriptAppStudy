using ClearScriptAppStudy.Dialogs;
using ClearScriptAppStudy.Types;
using Microsoft.ClearScript.V8;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClearScriptAppStudy.Services
{
    public class ScriptService : BindableBase, IDisposable
    {
        private readonly IDialogService dialogService;
        private ApplicationScript applicationScript = null;
        private V8ScriptEngine scriptEngine = null;
        private ObservableCollection<OutputLine> outputs = new ObservableCollection<OutputLine>();
        private bool disposedValue;

        public ScriptService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public ObservableCollection<OutputLine> Outputs
        {
            get => outputs;
            set => SetProperty(ref outputs, value); 
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
                    Outputs.Clear();

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

        private void OnWriteLine(string message)
        {
            var outputLine = new OutputLine(message);

            if (outputs.Any())
            {
                outputs.Insert(0, outputLine);
            }
            else
            {
                outputs.Add(outputLine);
            }
        }

        public async Task OnNewPerson(Person person)
        {
            if (scriptEngine != null && new List<string>(scriptEngine.Script.PropertyNames).Contains("OnNewPerson"))
            {
                await Task.Run(() =>
                {
                    scriptEngine.Script.OnNewPerson(person);
                });
            }
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

    public class OutputLine
    {
        public OutputLine(string message)
        {
            Occurrence = DateTime.Now;
            Message = message;
        }

        public DateTime Occurrence { get; }

        public string Message { get; set; }
    }

}
