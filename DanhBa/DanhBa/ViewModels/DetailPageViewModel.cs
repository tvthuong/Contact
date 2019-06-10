using System.Windows.Input;
using DanhBa.Models;
using DanhBa.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace DanhBa.ViewModels
{
    class DetailPageViewModel : ViewModelBase
    {
        private IDataService _dataService;
        private Contact _contact;
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)cmdDelete).RaiseCanExecuteChanged();
                ((DelegateCommand)cmdEdit).RaiseCanExecuteChanged();
            }
        }
        public ICommand cmdDelete { get; set; }
        public ICommand cmdEdit { get; set; }
        public ICommand cmdMessage { get; set; }
        public ICommand cmdPhone { get; set; }
        public ICommand cmdEmail { get; set; }
        public Contact Contact
        {
            get => _contact;
            set => SetProperty(ref _contact, value);
        }
        public DetailPageViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService)
        {
            _dataService = dataService;
            Contact = new Contact();
            cmdMessage = new DelegateCommand(Message);
            cmdPhone = new DelegateCommand(Phone);
            cmdEmail = new DelegateCommand(Email);
            cmdDelete = new DelegateCommand(Delete, CanExecute);
            cmdEdit = new DelegateCommand(Edit, CanExecute);
            Title = new Helpers.TranslateExtension() { Text = "DetailPage_Title" }.ProvideValue(null).ToString();
        }
        private bool CanExecute()
        {
            return IsNotBusy;
        }
        private async void Edit()
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("Edit", new NavigationParameters("Id=" + Contact.Id));
            IsBusy = false;
        }
        private async void Delete()
        {
            IsBusy = true;
            Helpers.TranslateExtension translate = new Helpers.TranslateExtension();
            translate.Text = "Alert_Message_Delete";
            string message = translate.ProvideValue(null).ToString();
            translate.Text = "btnAccept_Delete";
            string accept = translate.ProvideValue(null).ToString();
            translate.Text = "btnDecline_Delete";
            string decline = translate.ProvideValue(null).ToString();
            if (await Application.Current.MainPage.DisplayAlert("", message, accept, decline))
            {
                _dataService.DeleteElement(Contact);
                DependencyService.Get<IToastMessage>().LongTime(new Helpers.TranslateExtension() { Text = "ToastDelete" }.ProvideValue(null).ToString());
                await NavigationService.GoBackAsync();
            }
            else
                IsBusy = false;
        }
        private void Email()
        {
            Plugin.Messaging.CrossMessaging.Current.EmailMessenger.SendEmail(Contact.Email);
        }
        private void Phone()
        {
            Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(Contact.Phone, Contact.FullName);
        }
        private void Message()
        {
            Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(Contact.Phone);
        }
        public async void GetInfo(INavigationParameters parameters)
        {
            IsBusy = true;
            Contact = (Contact) await _dataService.GetElementById(parameters["Id"].ToString());
            IsBusy = false;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetInfo(parameters);
        }
    }
}