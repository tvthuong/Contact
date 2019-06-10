using DanhBa.Models;
using DanhBa.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace DanhBa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PeopleListPage : ContentPage
    {
		public PeopleListPage ()
		{
			InitializeComponent ();
		}
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((PeopleListPageViewModel)BindingContext).cmdView.Execute((Contact)e.Item);
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                ((PeopleListPageViewModel)BindingContext).cmdSearch.Execute(null);
        }
    }
}