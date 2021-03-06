﻿using ClearScriptAppStudy.Dialogs;
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
using System.Windows.Data;
using System.Windows.Documents;
using ClearScriptAppStudy.ScriptObjects;


namespace ClearScriptAppStudy.Services
{
    public class ScriptService : BindableBase, IDisposable,
                    IScriptDialogs,
                    IPersonMethods,
                    IFieldMethods
    {
        private readonly IDialogService dialogService;
        private ApplicationScript applicationScript = null;
        private V8ScriptEngine scriptEngine = null;
        private ObservableCollection<OutputLine> outputs = new ObservableCollection<OutputLine>();
        private readonly object lockOutputs = new object();
        private bool disposedValue;
        private List<string> listOfPropertyNames;

        public ScriptService(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            BindingOperations.EnableCollectionSynchronization(outputs, lockOutputs);
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
            listOfPropertyNames = new List<string>();

            if (!String.IsNullOrEmpty(scriptSettings.Script))
            {

                try
                {
                    Outputs.Clear();

                    scriptEngine.AddHostObject("Console", new ScriptObjects.Console(OnWriteLine));
                    scriptEngine.AddHostObject("Application", new ScriptObjects.Application());

                    var field = new Dictionary<string, string>()
                    {
                        { "eins", "Das ist der Wert für eins" }
                    };

                    scriptEngine.AddHostObject("Field", field);

                    scriptEngine.Execute(scriptSettings.Script);

                    listOfPropertyNames.AddRange(scriptEngine.Script.PropertyNames);
                }
                catch(Exception ex)
                {
                    OnWriteLine($"Fehler bei der Initialisierung {ex.Message}", OutputTypes.Error);
                }
            }
        }


        public void ShowScriptDialog()
        {
            DialogParameters dialogParameters = new DialogParameters {{nameof(Script), Script.Script}};


            dialogService.ShowDialog(nameof(ScriptDialogView), dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey(nameof(Script)))
                {
                    Script = new ApplicationScript() { Script = result.Parameters.GetValue<string>(nameof(Script)) };
                }
            });
        }

        private void OnWriteLine(string message, OutputTypes outputType = OutputTypes.Info)
        {
            var outputLine = new OutputLine(message) {OutputType = outputType};

            if (outputs.Any())
            {
                outputs.Insert(0, outputLine);
            }
            else
            {
                outputs.Add(outputLine);
            }
        }

        async Task IPersonMethods.OnNewPerson(Person person)
        {
            if (scriptEngine != null && listOfPropertyNames.Contains(nameof(IPersonMethods.OnNewPerson)))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        scriptEngine.Script.OnNewPerson(person);
                    }
                    catch(Exception ex)
                    {
                        OnWriteLine(ex.Message, OutputTypes.Error);
                    }
                    
                });
            }
        }

        async Task IPersonMethods.OnPersonSaved(Person person)
        {
            if (scriptEngine != null && listOfPropertyNames.Contains(nameof(IPersonMethods.OnPersonSaved)))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        scriptEngine.Script.OnPersonSaved(person);
                    }
                    catch(Exception ex)
                    {
                        OnWriteLine(ex.Message, OutputTypes.Error);
                    }
                    
                });
            }
        }

        async Task IFieldMethods.OnFieldGotFocus(object fieldInstance, string propertyName)
        {
            if (scriptEngine != null && listOfPropertyNames.Contains(nameof(IFieldMethods.OnFieldGotFocus)))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        scriptEngine.Script.OnFieldGotFocus(fieldInstance, propertyName);
                    }
                    catch (Exception ex)
                    {
                        OnWriteLine(ex.Message, OutputTypes.Error);
                    }
                });
            }
        }

        async Task IFieldMethods.OnFieldLostFocus(object fieldInstance, string propertyName)
        {
            if (scriptEngine != null && listOfPropertyNames.Contains(nameof(IFieldMethods.OnFieldLostFocus)))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        scriptEngine.Script.OnFieldLostFocus(fieldInstance, propertyName);
                    }
                    catch (Exception ex)
                    {
                        OnWriteLine(ex.Message, OutputTypes.Error);
                    }

                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    scriptEngine?.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public enum OutputTypes
    {
        Info = 0,
        
        Warning = 1,
        
        Error = 2
    }

    public class OutputLine
    {
        public OutputLine(string message)
        {
            Occurrence = DateTime.Now;
            Message = message;
        }
        public OutputTypes OutputType { get; set; }

        public DateTime Occurrence { get; }

        public string Message { get; set; }
    }

}
