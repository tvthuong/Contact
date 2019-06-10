using System.Windows.Input;
using DanhBa.Models;
using DanhBa.Services;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DanhBa.ViewModels
{
    public class PeopleListPageViewModel : ViewModelBase
    {
        private IDataService _dataService;
        private ObservableCollection<GroupObservableCollection> _contacts;
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
        public ObservableCollection<GroupObservableCollection> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }
        public PeopleListPageViewModel(INavigationService navigationService, IDataService dataservice) : base(navigationService)
        {
            _dataService = dataservice;
            cmdAdd = new DelegateCommand(Add, CanExecute);
            cmdDelete = new DelegateCommand<Contact>(Delete);
            cmdEdit = new DelegateCommand<Contact>(Edit);
            cmdView = new DelegateCommand<Contact>(View);
            cmdSearch = new DelegateCommand(Search);
            Contacts = new ObservableCollection<GroupObservableCollection>();
            txtFilter = "";
            Title = new Helpers.TranslateExtension() { Text = "PeopleListPage_Title" }.ProvideValue(null).ToString();
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
            Helpers.TranslateExtension translate = new Helpers.TranslateExtension();
            translate.Text = "Alert_Message_Delete";
            string message = translate.ProvideValue(null).ToString();
            translate.Text = "btnAccept_Delete";
            string accept = translate.ProvideValue(null).ToString();
            translate.Text = "btnDecline_Delete";
            string decline = translate.ProvideValue(null).ToString();
            if (await Application.Current.MainPage.DisplayAlert("", message, accept, decline))
            {
                _dataService.DeleteElement(obj);
                DependencyService.Get<IToastMessage>().LongTime(new Helpers.TranslateExtension() { Text = "ToastDelete" }.ProvideValue(null).ToString());
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