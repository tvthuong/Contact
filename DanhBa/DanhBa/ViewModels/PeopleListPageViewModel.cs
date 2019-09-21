using System.Windows.Input;
using DanhBa.Models;
using DanhBa.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DanhBa.Resource;
using System.Linq;

namespace DanhBa.ViewModels
{
    public class PeopleListPageViewModel : ViewModelBase
    {
        private IDataService<Contact, IGrouping<string, Contact>> _dataService;
        private ObservableCollection<IGrouping<string, Contact>> _contacts;
        private string _txtFilter;
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)cmdAdd).RaiseCanExecuteChanged();
            }
        }
        public string txtFilter
        {
            get => _txtFilter;
            set => SetProperty(ref _txtFilter, value);
        }
        public ICommand cmdSearch { get; set; }
        public ICommand cmdAdd { get; set; }
        public ICommand cmdDelete { get; set; }
        public ICommand cmdEdit { get; set; }
        public ICommand cmdView { get; set; }
        public ObservableCollection<IGrouping<string, Contact>> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }
        public PeopleListPageViewModel(INavigationService navigationService, IDataService<Contact, IGrouping<string, Contact>> dataservice) : base(navigationService)
        {
            _dataService = dataservice;
            cmdAdd = new DelegateCommand(Add, CanExecute);
            cmdDelete = new DelegateCommand<Contact>(Delete);
            cmdEdit = new DelegateCommand<Contact>(Edit);
            cmdView = new DelegateCommand<Contact>(View);
            cmdSearch = new DelegateCommand(Search);
            Contacts = new ObservableCollection<IGrouping<string, Contact>>();
            txtFilter = "";
            Title = UI.PeopleListPage_Title;
        }
        private async void Search()
        {
            IsBusy = true;
            if (!string.IsNullOrEmpty(txtFilter))
                Contacts = await _dataService.FilterElements(txtFilter);
            else
                Contacts = await _dataService.GetAllElements();
            IsBusy = false;
        }
        private bool CanExecute()
        {
            return IsNotBusy;
        }
        private async void View(Contact obj)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("Detail", new NavigationParameters("Id=" + obj.Id));
            IsBusy = false;
        }
        private async void Edit(Contact obj)
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("Edit", new NavigationParameters("Id=" + obj.Id));
            IsBusy = false;
        }
        private async void Delete(Contact obj)
        {
            IsBusy = true;
            string message = UI.Alert_Message_Delete;
            string accept = UI.btnAccept_Delete;
            string decline = UI.btnDecline_Delete;
            if (await Application.Current.MainPage.DisplayAlert("", message, accept, decline))
            {
                _dataService.DeleteElement(obj);
                DependencyService.Get<IToastMessage>().LongTime(UI.ToastDelete);
                Search();
            }
            else
                IsBusy = false;
        }
        private async void Add()
        {
            IsBusy = true;
            await NavigationService.NavigateAsync("Edit");
            IsBusy = false;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Search();
        }
    }
}