using DanhBa.Models;
using DanhBa.Services;
using DanhBa.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DanhBa
{
    public partial class App : PrismApplication
    {
        public App() : base(null) { }
        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync("Navigation/PeopleList");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(IDataService<Contact, IGrouping<string, Contact>>), typeof(ContactDataService));
            containerRegistry.RegisterForNavigation<NavigationPage>("Navigation");
            containerRegistry.RegisterForNavigation<PeopleListPage>("PeopleList");
            containerRegistry.RegisterForNavigation<DetailPage>("Detail");
            containerRegistry.RegisterForNavigation<EditPage>("Edit");
            containerRegistry.RegisterForNavigation<SelectImagePage>("SelectImage");
        }
    }
}
