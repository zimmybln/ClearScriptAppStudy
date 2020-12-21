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
using System.Linq;
using System.Text;
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
        private readonly IDialogService dialogService;
        private readonly ScriptService scriptService;
        private UIElement focusedElement;
        private ICommand showScriptDialogCommand;
        private ICommand newPersonCommand;
        private ICommand savePersonCommand;
        private ICommand onFocusCommand;

        private Person selectedPerson;
        private Person editablePerson;
        private ObservableCollection<Person> persons;
        private bool areToolsVisible = true;
        private string stateInfo;
        private string fieldInfo;
        private int stateInfoTimeout = 5;
        private DispatcherTimer stateInfoTimer;
        private DispatcherTimer fieldInfoTimer;
        private EventAction eventAction;


        public MainWindowViewModel(
                IContainerProvider container,
                IDialogService dialogService,
                ScriptService scriptService)
        {
            this.container = container;
            this.dialogService = dialogService;
            this.scriptService = scriptService;

            eventAction = new GotFocusToScriptAction(this.scriptService);

            stateInfoTimer = new DispatcherTimer(new TimeSpan(0, 0, stateInfoTimeout), DispatcherPriority.Background, OnStateTimer,
                Dispatcher.CurrentDispatcher);
            fieldInfoTimer = new DispatcherTimer(new TimeSpan(0, 0, stateInfoTimeout), DispatcherPriority.Background, OnFieldTimer, 
                Dispatcher.CurrentDispatcher);

            persons = new ObservableCollection<Person>();
        }

        private void OnFieldTimer(object? sender, EventArgs e)
        {
            FieldInfo = string.Empty;
        }

        private void OnStateTimer(object? sender, EventArgs e)
        {
            StateInfo = String.Empty;
        }

        public ICommand ShowScriptDialogCommand => 
            showScriptDialogCommand ??= new DelegateCommand(OnShowScriptDialog);

        public ICommand NewPersonCommand =>
            newPersonCommand ??= new DelegateCommand(OnNewPerson);

        public ICommand SavePersonCommand =>
            savePersonCommand ??= new DelegateCommand(OnSavePerson);

        public ICommand GotFocusCommand =>
            onFocusCommand ??= new DelegateCommand<RoutedEventArgs>(OnGotFocus);

        public ObservableCollection<OutputLine> Outputs => container.Resolve<ScriptService>().Outputs;

        public ObservableCollection<Person> Persons
        {
            get => persons;
            set => SetProperty(ref persons, value);
        }

        public Person SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value);
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


            if (!String.IsNullOrEmpty(FieldInfo) && StateInfoTimeout > 0)
            {
                fieldInfoTimer.Interval = new TimeSpan(0, 0, 0, StateInfoTimeout);
                fieldInfoTimer.Start();
            }
        }

        public EventAction GotFocusAction => eventAction;



        public int StateInfoTimeout
        {
            get => stateInfoTimeout;
            set => SetProperty(ref stateInfoTimeout, value);
        }

        private void OnShowScriptDialog()
        {
            var scriptService = container.Resolve<ScriptService>();

            scriptService.ShowScriptDialog();
        }

        private void OnGotFocus(RoutedEventArgs args)
        { 

        }

        private async void OnNewPerson()
        {
            var person = new Person();

            // Skript ausführen
            await container.Resolve<ScriptService>()?.OnNewPerson(person);

            // hinzufügen und auswählen
            EditablePerson = person;
        }

        private async void OnSavePerson()
        {
            if (EditablePerson != null)
            {
                // save the person here
                EditablePerson.Id = Guid.NewGuid();
                
                // inform the script about the saved person
                await container.Resolve<ScriptService>().OnPersonSaved(EditablePerson);

                // fit the ui
                Persons.Add(EditablePerson);
                SelectedPerson = EditablePerson;
                EditablePerson = null;
            }
        }
    }
}
