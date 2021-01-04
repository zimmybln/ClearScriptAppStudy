using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClearScriptAppStudy.Types
{
    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string firstName;
        private string lastName;
        private string title;
        private string street;
        private string city;
        private string zipCode;
        private string notes;
        private Guid id = Guid.Empty;
        private string activeField;

        public Person()
        {

        }

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public Guid Id
        {
            get => id;
            set
            {
                if (!id.Equals(value))
                {
                    id = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (!String.Equals(firstName, value))
                {
                    firstName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (!String.Equals(lastName, value))
                {
                    lastName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Title
        {
            get => title;
            set
            {
                if (!String.Equals(title, value))
                {
                    title = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string City
        {
            get => city;
            set
            {
                if (!String.Equals(city, value))
                {
                    city = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ZipCode
        {
            get => zipCode;
            set
            {
                if (!String.Equals(zipCode, value))
                {
                    zipCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Street
        {
            get => street;
            set
            {
                if (!String.Equals(street, value))
                {
                    street = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => notes;
            set
            {
                if (!String.Equals(notes, value))
                {
                    notes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ActiveField
        {
            get => activeField;
            set
            {
                activeField = value;
                RaisePropertyChanged();

            }
        }


        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
