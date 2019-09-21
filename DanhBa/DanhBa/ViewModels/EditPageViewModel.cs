using System.Linq;
using System.Windows.Input;
using DanhBa.Models;
using DanhBa.Resource;
using DanhBa.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
namespace DanhBa.ViewModels
{
    class EditPageViewModel : ViewModelBase
    {
        private IDataService<Contact, IGrouping<string, Contact>> _dataService;
        private Contact contact;
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)cmdSave).RaiseCanExecuteChanged();
            }
        }
        public ICommand cmdImage { set; get; }
        public ICommand cmdSave { set; get; }
        public Contact Contact
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }
        public EditPageViewModel(INavigationService navigationService, IDataService<Contact, IGrouping<string, Contact>> dataservice) : base(navigationService)
        {
            _dataService = dataservice;
            Contact = new Contact();
            cmdSave = new DelegateCommand(Save,CanExecute);
            cmdImage = new DelegateCommand(SelectImage);
        }
        private async void SelectImage()
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
            else if(result == fromdevice)
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
            if(CrossMedia.Current.IsPickPhotoSupported)
            {
                PickMediaOptions pickMediaOptions = new PickMediaOptions() { PhotoSize = PhotoSize.Medium };
                MediaFile mediaFile = await CrossMedia.Current.PickPhotoAsync(pickMediaOptions);
                if (mediaFile != null)
                    Contact.PhotoUrl = mediaFile.Path;
            }
            IsBusy = false;
        }
        private bool CanExecute()
        {
            return IsNotBusy && !string.IsNullOrEmpty(Contact.FullName.Trim()) && !string.IsNullOrEmpty(Contact.Phone.Trim()) ? true : false;
        }
        private void Save()
        {
            if (CanExecute())
            {
                IsBusy = true;
                if (string.IsNullOrEmpty(Contact.Id))
                    _dataService.AddNewElement(Contact);
                else
                    _dataService.SaveElement(Contact);
                NavigationService.GoBackAsync(new NavigationParameters("Id=" + Contact.Id));
            }
            else
            {
                ((DelegateCommand)cmdSave).RaiseCanExecuteChanged();
            }
        }
        public async void GetInfo(INavigationParameters parameters)
        {
            IsBusy = true;
            if (parameters.ContainsKey("Id"))
            {
                Title = UI.EditPage_Edit_Title;
                Contact temp = (Contact)await _dataService.GetElementById(parameters["Id"].ToString());
                Contact = (Contact)temp.Clone();
            }
            else
                Title = UI.EditPage_Add_Title;
            if (parameters.ContainsKey("PhotoUrl"))
                Contact.PhotoUrl = parameters["PhotoUrl"].ToString();
            IsBusy = false;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            parameters.Add("Id", Contact.Id);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetInfo(parameters);
        }
    }
}