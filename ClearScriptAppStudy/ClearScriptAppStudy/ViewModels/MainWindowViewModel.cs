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

namespace ClearScriptAppStudy.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerProvider container;
        private readonly IDialogService dialogService;
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
        private int stateInfoTimeout = 5;
        private DispatcherTimer stateInfoTimer;


        public MainWindowViewModel(IContainerProvider container,
                IDialogService dialogService)
        {
            this.container = container;
            this.dialogService = dialogService;

            stateInfoTimer = new DispatcherTimer(new TimeSpan(0, 0, stateInfoTimeout), DispatcherPriority.Background, OnStateTimer,
                Dispatcher.CurrentDispatcher);

            persons = new ObservableCollection<Person>();
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

        private void OnSavePerson()
        {
            if (EditablePerson != null)
            {
                Persons.Add(EditablePerson);
                SelectedPerson = EditablePerson;
            }
        }
    }
}
