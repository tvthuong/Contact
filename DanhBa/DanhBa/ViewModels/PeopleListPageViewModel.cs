using System.Windows.Input;
using DanhBa.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DanhBa.Resource;
using System.Linq;
using System.Collections.Generic;
using DanhBa.Business;
using DanhBa.Business.Models;

namespace DanhBa.ViewModels
{
    public class PeopleListPageViewModel : ViewModelBase
    {
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)AddContactCommand).RaiseCanExecuteChanged();
            }
        }
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, () => RaisePropertyChanged(nameof(Contacts)));
        }
        public ObservableCollection<IGrouping<string, Contact>> Contacts
        {
            get
            {
                var result = ContactItemModels?.Where(item =>
                item.IsContainsText(SearchText)).GroupBy(item => item.ShortName);
                if(result != null)
                    return new ObservableCollection<IGrouping<string, Contact>>(result);
                return null;
            }
        }

        public ICommand AddContactCommand { get; set; }
        public ICommand DeleteContactCommand { get; set; }
        public ICommand EditContactCommand { get; set; }
        public ICommand ViewDetailContactCommand { get; set; }

        protected IEnumerable<Contact> ContactItemModels
        {
            set => SetProperty(ref _contactItemModels, value, () => { RaisePropertyChanged(nameof(Contacts)); });
            get => _contactItemModels;
        }

        private IEnumerable<Contact> _contactItemModels;
        private string _searchText;

        public PeopleListPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            InitializeCommand();
            Title = UI.PeopleListPage_Title;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ContactItemModels = ContactBD.Instance.Contacts.Select(item => new Contact(item));
            IsBusy = false;
        }

        private void InitializeCommand()
        {
            AddContactCommand = new DelegateCommand(HandleAddContactCommand, CanExecuteAddContactCommand);
            DeleteContactCommand = new DelegateCommand<Contact>(HandleDeleteContactCommand);
            EditContactCommand = new DelegateCommand<Contact>(HandleEditContactCommand);
            ViewDetailContactCommand = new DelegateCommand<Contact>(HandleViewDetailContactCommand);
        }

        private bool CanExecuteAddContactCommand()
        {
            return IsNotBusy;
        }

        private async void HandleViewDetailContactCommand(Contact contact)
        {
            IsBusy = true;
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Contact", contact);
            await NavigationService.NavigateAsync("Detail", parameters);
            IsBusy = false;
        }

        private async void HandleEditContactCommand(Contact contact)
        {
            IsBusy = true;
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Contact", contact);
            await NavigationService.NavigateAsync("Edit", parameters);
            IsBusy = false;
        }

        private async void HandleDeleteContactCommand(Contact contact)
        {
            IsBusy = true;
            string message = UI.Alert_Message_Delete;
            string accept = UI.btnAccept_Delete;
            string decline = UI.btnDecline_Delete;
            if (await Application.Current.MainPage.DisplayAlert("", message, accept, decline))
            {
                ContactBD.Instance.Delete<ContactEntity>(contact.Id);
                DependencyService.Get<IToastMessage>().LongTime(UI.ToastDelete);
                ContactItemModels = ContactBD.Instance.Contacts.Select(item => new Contact(item));
            }
            IsBusy = false;
        }

        private async void HandleAddContactCommand()
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("Edit");
            IsBusy = false;
        }
    }
}