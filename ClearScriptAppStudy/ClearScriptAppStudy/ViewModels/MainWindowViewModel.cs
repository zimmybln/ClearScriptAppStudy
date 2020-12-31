using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ClearScriptAppStudy.Components.Behaviors;

namespace ClearScriptAppStudy.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerProvider container;
        private readonly ScriptService scriptService;
        private readonly IPersonMethods personScriptMethods;
        private ICommand showScriptDialogCommand;
        private ICommand newPersonCommand;
        private ICommand savePersonCommand;
        private ICommand newFileCommand;
        private ICommand saveFileCommand;
        private ICommand openFileCommand;

        private Person selectedPerson;
        private Person editablePerson;
        private ObservableCollection<Person> persons;
        private bool areToolsVisible = true;
        private string fileName;
        private string stateInfo;
        private string fieldInfo;
        private int stateInfoTimeout = 5;
        private int fieldInfoTimeout = 10;
        private readonly DispatcherTimer stateInfoTimer;
        private readonly DispatcherTimer fieldInfoTimer;

        public MainWindowViewModel(
                IContainerProvider container,
                ScriptService scriptService)
        {
            this.container = container;
            this.scriptService = scriptService;
            this.personScriptMethods = scriptService as IPersonMethods;
            
            CommandManager.RegisterClassCommandBinding(this.GetType(), new CommandBinding(ApplicationCommands.Close, (sender, args) => App.Current.Shutdown()));

            GotFocusAction = new GotFocusToScriptAction<Person>(this.scriptService);

            stateInfoTimer = new DispatcherTimer(new TimeSpan(0, 0, stateInfoTimeout), DispatcherPriority.Background, 
                (sender, args) => StateInfo = string.Empty, Dispatcher.CurrentDispatcher);
            fieldInfoTimer = new DispatcherTimer(new TimeSpan(0, 0, fieldInfoTimeout), DispatcherPriority.Background,
                (sender, args) => FieldInfo = string.Empty, Dispatcher.CurrentDispatcher);

            persons = new ObservableCollection<Person>();
        }


        public ICommand ShowScriptDialogCommand => 
            showScriptDialogCommand ??= new DelegateCommand(OnShowScriptDialog);

        public ICommand NewPersonCommand =>
            newPersonCommand ??= new DelegateCommand(OnNewPerson);

        public ICommand SavePersonCommand =>
            savePersonCommand ??= new DelegateCommand(OnSavePerson);

        public ICommand NewFileCommand =>
            newFileCommand ??= new DelegateCommand(OnNewFile);

        public ICommand SaveFileCommand =>
            saveFileCommand ??= new DelegateCommand(OnSaveFile);

        public ICommand OpenFileCommand =>
            openFileCommand ??= new DelegateCommand(OnOpenFile);
        
        public CommandBindingCollection CommandBindings { get; set; }

        private void OnOpenFile()
        {
            fileName = @"D:\test.json";

            var items = JsonSerializer.Deserialize<Person[]>(File.ReadAllText(fileName, Encoding.UTF8));
            
            Persons.Clear();

            if (items.Any())
            {
                Persons.AddRange(items);
            }
        }

        private void OnSaveFile()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                // Datei auswählen
                fileName = @"D:\test.json";
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var serializedContent = JsonSerializer.Serialize(Persons.ToArray(), options);
            
            if (File.Exists(fileName))
                File.Delete(fileName);
            
            File.WriteAllText(fileName, serializedContent, Encoding.UTF8);
        }

        private void OnNewFile()
        {
            Persons.Clear();
        }

        public ObservableCollection<OutputLine> Outputs => scriptService.Outputs;

        public ObservableCollection<Person> Persons
        {
            get => persons;
            set => SetProperty(ref persons, value);
        }

        public Person SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value, OnSelectedPersonChanged);
        }

        private void OnSelectedPersonChanged()
        {
            EditablePerson = SelectedPerson;
        }

        public Person EditablePerson
        {
            get => editablePerson;
            set => SetProperty(ref editablePerson, value);
        }

        public bool AreToolsVisible
        {
            get => areToolsVisible;
            set => SetProperty(ref areToolsVisible, value);
        }

        public string StateInfo
        {
            get => stateInfo;
            set => SetProperty(ref stateInfo, value, OnStateInfoChanged);
        }

        private void OnStateInfoChanged()
        {
            stateInfoTimer.Stop();


            if (!String.IsNullOrEmpty(StateInfo) && StateInfoTimeout > 0)
            {
                stateInfoTimer.Interval = new TimeSpan(0, 0, 0, StateInfoTimeout);
                stateInfoTimer.Start();
            }
        }

        public string FieldInfo
        {
            get => fieldInfo;
            set => SetProperty(ref fieldInfo, value, OnFieldInfoChanged);
        }

        private void OnFieldInfoChanged()
        {
            fieldInfoTimer.Stop();


            if (!String.IsNullOrEmpty(FieldInfo) && FieldInfoTimeout > 0)
            {
                fieldInfoTimer.Interval = new TimeSpan(0, 0, 0, FieldInfoTimeout);
                fieldInfoTimer.Start();
            }
        }

        public EventAction GotFocusAction { get; }


        public int StateInfoTimeout
        {
            get => stateInfoTimeout;
            set => SetProperty(ref stateInfoTimeout, value);
        }

        public int FieldInfoTimeout
        {
            get => fieldInfoTimeout;
            set => SetProperty(ref fieldInfoTimeout, value);
        }

        private void OnShowScriptDialog()
        {
            scriptService.ShowScriptDialog();
        }

        private async void OnNewPerson()
        {
            var person = new Person();

            // Skript ausführen
            await personScriptMethods.OnNewPerson(person);

            SelectedPerson = null;
            
            // hinzufügen und auswählen
            EditablePerson = person;
        }

        private async void OnSavePerson()
        {
            if (EditablePerson != null)
            {
                if (EditablePerson.Id.Equals(Guid.Empty))
                {
                    EditablePerson.Id = Guid.NewGuid();
                    Persons.Add(EditablePerson);
                    SelectedPerson = EditablePerson;
                }
                else
                {
                    // when the person has got already an id, we don't need
                    // to do something here
                }
                
                // inform the script about the saved person
                await scriptService.OnPersonSaved(EditablePerson);

            }
        }
    }
}
