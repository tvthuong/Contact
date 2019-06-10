using System;
using Prism.Mvvm;
using SQLite;
namespace DanhBa.Models
{
    public class Contact : BindableBase, ICloneable
    {
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _photoUrl;
        private string _company;
        private string _jobTitle;
        private string _email;
        private string _street;
        private string _city;
        private string _postalCode;
        private string _state;
        [PrimaryKey]
        public string Id { set; get; }
        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                RaisePropertyChanged(nameof(FullName));
            }
        }
        public string FullName => FirstName.Trim() + (!string.IsNullOrEmpty(FirstName.Trim()) && !string.IsNullOrEmpty(LastName.Trim()) ? " " : "") + LastName.Trim();
        public string Company
        {
            get => _company;
            set => SetProperty(ref _company , value);
        }
        public string JobTitle
        {
            get => _jobTitle;
            set => SetProperty(ref _jobTitle , value);
        }
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                RaisePropertyChanged(nameof(HasEmailAddress));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
                RaisePropertyChanged(nameof(HasPhoneNumber));
            }
        }
        public string Street
        {
            get => _street;
            set
            {
                SetProperty(ref _street, value);
                RaisePropertyChanged(nameof(HasAddress));
            }
        }
        public string City
        {
            get => _city;
            set => SetProperty(ref _city , value);
        }
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                SetProperty(ref _postalCode, value);
                RaisePropertyChanged(nameof(StatePostal));
            }
        }
        public string State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                RaisePropertyChanged(nameof(HasAddress));
                RaisePropertyChanged(nameof(StatePostal));
            }
        }
        public string PhotoUrl
        {
            get => _photoUrl;
            set => SetProperty(ref _photoUrl, value);
        }
        public bool HasPhoneNumber => !string.IsNullOrEmpty(Phone);
        public bool HasEmailAddress => !string.IsNullOrEmpty(Email);
        public bool HasAddress => !string.IsNullOrEmpty(Street) || !string.IsNullOrEmpty(State);
        public string StatePostal => State + (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(PostalCode) ? " - " : "") + (!string.IsNullOrEmpty(PostalCode) ? "(" : "") + PostalCode + (!string.IsNullOrEmpty(PostalCode) ? ")" : "");
        public object Clone()
        {
            return MemberwiseClone();
        }
        public Contact()
        {
            PhotoUrl = "ContactImage.PNG";
            FirstName = "";
            LastName = "";
            Phone = "";
            Company = "";
            JobTitle = "";
            Email = "";
            Street = "";
            City = "";
            PostalCode = "";
            State = "";
            
        }
    }
}