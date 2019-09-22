using DanhBa.Business;
using DanhBa.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
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
            ContactBD.Instance.Initialize(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            NavigationService.NavigateAsync("Navigation/PeopleList");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Navigation");
            containerRegistry.RegisterForNavigation<PeopleListPage>("PeopleList");
            containerRegistry.RegisterForNavigation<DetailPage>("Detail");
            containerRegistry.RegisterForNavigation<EditPage>("Edit");
            containerRegistry.RegisterForNavigation<SelectImagePage>("SelectImage");
        }
    }
}
