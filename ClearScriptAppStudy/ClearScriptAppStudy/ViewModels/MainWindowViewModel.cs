using ClearScriptAppStudy.Services;
using ClearScriptAppStudy.Types;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ICommand newPersonCommand;
        private ICommand savePersonCommand;

        private Person selectedPerson;
        private Person editablePerson;
        private ObservableCollection<Person> persons;


        public MainWindowViewModel(IContainerProvider container,
                IDialogService dialogService)
        {
            this.container = container;
            this.dialogService = dialogService;

            persons = new ObservableCollection<Person>();
        }

        public ICommand ShowScriptDialogCommand => 
            showScriptDialogCommand ??= new DelegateCommand(OnShowScriptDialog);

        public ICommand NewPersonCommand =>
            newPersonCommand ??= new DelegateCommand(OnNewPerson);

        public ICommand SavePersonCommand =>
            savePersonCommand ??= new DelegateCommand(OnSavePerson);


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

        private void OnShowScriptDialog()
        {
            var scriptService = container.Resolve<ScriptService>();

            scriptService.ShowScriptDialog();
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
