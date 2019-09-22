using System.Windows.Input;
using DanhBa.Business;
using DanhBa.Business.Models;
using DanhBa.Models;
using DanhBa.Resource;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace DanhBa.ViewModels
{
    class DetailPageViewModel : ViewModelBase
    {
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string Company { get => _company; set => SetProperty(ref _company, value); }
        public string JobTitle { get => _jobTitle; set => SetProperty(ref _jobTitle, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string PhotoUrl { get => _photoUrl; set => SetProperty(ref _photoUrl, value); }
        public string StatePostal { get => _statePostal; set => SetProperty(ref _statePostal, value); }
        public bool HasAddress { get => _hasAddress; set => SetProperty(ref _hasAddress, value); }
        public bool HasPhoneNumber { get => _hasPhoneNumber; set => SetProperty(ref _hasPhoneNumber, value); }
        public bool HasEmailAddress { get => _hasEmailAddress; set => SetProperty(ref _hasEmailAddress, value); }
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)DeleteContactCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)EditContactCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand DeleteContactCommand { get; set; }
        public ICommand EditContactCommand { get; set; }
        public ICommand MessageCommand { get; set; }
        public ICommand PhoneCommand { get; set; }
        public ICommand SendEmailCommand { get; set; }

        private Contact _contact;
        private string _fullName;
        private string _company;
        private string _jobTitle;
        private string _email;
        private string _phone;
        private string _street;
        private string _city;
        private string _photoUrl;
        private bool _hasAddress;
        private string _statePostal;
        private bool _hasPhoneNumber;
        private bool _hasEmailAddress;

        public DetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = UI.DetailPage_Title;
            InitializeCommand();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetContact(parameters);
        }

        private void InitializeCommand()
        {
            MessageCommand = new DelegateCommand(HandleMessageCommand);
            PhoneCommand = new DelegateCommand(HandlePhoneCommand);
            SendEmailCommand = new DelegateCommand(HandleSendEmailCommand);
            DeleteContactCommand = new DelegateCommand(HandleDeleteContactCommand, CanExecute);
            EditContactCommand = new DelegateCommand(HandleEditContactCommand, CanExecute);
        }

        private bool CanExecute()
        {
            return IsNotBusy;
        }

        private async void HandleEditContactCommand()
        {
            IsBusy = true;
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Contact", _contact);
            await NavigationService.NavigateAsync("Edit", parameters);
            IsBusy = false;
        }

        private async void HandleDeleteContactCommand()
        {
            IsBusy = true;
            string message = UI.Alert_Message_Delete;
            string accept = UI.btnAccept_Delete;
            string decline = UI.btnDecline_Delete;
            if (await Application.Current.MainPage.DisplayAlert("", message, accept, decline))
            {
                ContactBD.Instance.Delete<ContactEntity>(_contact.Id);
                DependencyService.Get<IToastMessage>().LongTime(UI.ToastDelete);
                await NavigationService.GoBackAsync();
            }
            else
                IsBusy = false;
        }

        private void HandleSendEmailCommand()
        {
            Plugin.Messaging.CrossMessaging.Current.EmailMessenger.SendEmail(Email);
        }

        private void HandlePhoneCommand()
        {
            Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(Phone, FullName);
        }

        private void HandleMessageCommand()
        {
            Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(Phone);
        }

        private void GetContact(INavigationParameters parameters)
        {
            IsBusy = true;
            if (parameters.ContainsKey("Contact"))
            {
                _contact = parameters["Contact"] as Contact;
                OnContactChanged();
            }
            IsBusy = false;
        }

        private void OnContactChanged()
        {
            FullName = _contact.FullName;
            Company = _contact.Company;
            JobTitle = _contact.JobTitle;
            Email = _contact.Email;
            Phone = _contact.Phone;
            Street = _contact.Street;
            City = _contact.City;
            PhotoUrl = _contact.PhotoUrl;
            HasAddress = _contact.HasAddress;
            StatePostal = _contact.StatePostal;
            HasPhoneNumber = _contact.HasPhoneNumber;
            HasEmailAddress = _contact.HasEmailAddress;
        }
    }
}