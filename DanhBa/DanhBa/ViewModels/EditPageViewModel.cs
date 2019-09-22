using System.Windows.Input;
using DanhBa.Business;
using DanhBa.Models;
using DanhBa.Resource;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
namespace DanhBa.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value, () => { ((DelegateCommand)SaveContactCommand).RaiseCanExecuteChanged(); }); }
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value, () => { ((DelegateCommand)SaveContactCommand).RaiseCanExecuteChanged(); }); }
        public string Company { get => _company; set => SetProperty(ref _company, value); }
        public string JobTitle { get => _jobTitle; set => SetProperty(ref _jobTitle, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value, () => { ((DelegateCommand)SaveContactCommand).RaiseCanExecuteChanged(); }); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string PostalCode { get => _postalCode; set => SetProperty(ref _postalCode, value); }
        public string State { get => _state; set => SetProperty(ref _state, value); }
        public string PhotoUrl { get => _photoUrl; set => SetProperty(ref _photoUrl, value); }
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)SaveContactCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SelectContactImageCommand { set; get; }
        public ICommand SaveContactCommand { set; get; }

        private Contact _contact;
        private string _firstName;
        private string _lastName;
        private string _company;
        private string _jobTitle;
        private string _email;
        private string _phone;
        private string _street;
        private string _city;
        private string _postalCode;
        private string _state;
        private string _photoUrl = "ContactImage.PNG";
        private bool _isInsert = true;

        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            InitializeCommand();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetContact(parameters);
        }

        private void InitializeCommand()
        {
            SaveContactCommand = new DelegateCommand(HandleSaveContactCommand, CanExecute);
            SelectContactImageCommand = new DelegateCommand(HandleSelectContactImageCommand);
        }

        private async void HandleSelectContactImageCommand()
        {
            IsBusy = true;
            string title = UI.Alert_Title_Select_Image;
            string decline = UI.btnDecline;
            string fromweb = UI.btnFromWeb;
            string fromdevice = UI.btnFromdevice;
            string result = await Application.Current.MainPage.DisplayActionSheet(title, decline, null, fromweb, fromdevice);
            if (result == fromweb)
            {
                await NavigationService.NavigateAsync("SelectImage");
                IsBusy = false;
            }
            else if (result == fromdevice)
                DeviceImage();
            else
                IsBusy = false;
        }

        private async void DeviceImage()
        {
            IsBusy = true;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                string title = UI.Alert_Title;
                string message = UI.Alert_Message;
                string accept = UI.btnAccept;
                string decline = UI.btnDecline;
                if (await Application.Current.MainPage.DisplayAlert(title, message, accept, decline))
                    DependencyService.Get<ISettings>().OpenManageApplicationsSettings();
                IsBusy = false;
                return;
            }
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                PickMediaOptions pickMediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium };
                MediaFile mediaFile = await CrossMedia.Current.PickPhotoAsync(pickMediaOptions);
                if (mediaFile != null)
                    PhotoUrl = mediaFile.Path;
            }
            IsBusy = false;
        }

        private bool CanExecute()
        {
            return IsNotBusy && (!string.IsNullOrEmpty(FirstName?.Trim()) || !string.IsNullOrEmpty(LastName?.Trim())) && !string.IsNullOrEmpty(Phone?.Trim());
        }

        private void HandleSaveContactCommand()
        {
            if (CanExecute())
            {
                IsBusy = true;
                PrepareForSave();
                if (_isInsert)
                    ContactBD.Instance.Insert(_contact.Entity);
                else
                    ContactBD.Instance.Update(_contact.Entity);
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("Contact", _contact);
                NavigationService.GoBackAsync(parameters);
            }
            else
            {
                ((DelegateCommand)SaveContactCommand).RaiseCanExecuteChanged();
            }
        }

        private void PrepareForSave()
        {
            if (_contact == null)
                _contact = new Contact();
            _contact.Entity.FirstName = FirstName ?? string.Empty;
            _contact.Entity.LastName = LastName ?? string.Empty;
            _contact.Entity.Company = Company ?? string.Empty;
            _contact.Entity.JobTitle = JobTitle ?? string.Empty;
            _contact.Entity.Email = Email ?? string.Empty;
            _contact.Entity.Phone = Phone ?? string.Empty;
            _contact.Entity.Street = Street ?? string.Empty;
            _contact.Entity.City = City ?? string.Empty;
            _contact.Entity.PostalCode = PostalCode ?? string.Empty;
            _contact.Entity.State = State ?? string.Empty;
            _contact.Entity.PhotoUrl = PhotoUrl ?? string.Empty;
        }

        private void GetContact(INavigationParameters parameters)
        {
            IsBusy = true;
            if (parameters.ContainsKey("Contact"))
            {
                Title = UI.EditPage_Edit_Title;
                _contact = parameters["Contact"] as Contact;
                _isInsert = false;
                OnContactChanged();
            }
            else
                Title = UI.EditPage_Add_Title;
            if (parameters.ContainsKey("PhotoUrl"))
                PhotoUrl = parameters["PhotoUrl"].ToString();
            IsBusy = false;
        }

        private void OnContactChanged()
        {
            FirstName = _contact.FirstName;
            LastName = _contact.LastName;
            Company = _contact.Company;
            JobTitle = _contact.JobTitle;
            Email = _contact.Email;
            Phone = _contact.Phone;
            Street = _contact.Street;
            City = _contact.City;
            PostalCode = _contact.PostalCode;
            State = _contact.State;
            PhotoUrl = _contact.PhotoUrl;
        }
    }
}