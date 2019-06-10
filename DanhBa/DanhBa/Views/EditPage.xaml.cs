using DanhBa.ViewModels;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace DanhBa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
		public EditPage ()
		{
			InitializeComponent ();
		}
        private void EntryCell_Completed(object sender, System.EventArgs e)
        {
            ((DelegateCommand)((EditPageViewModel)BindingContext).cmdSave).RaiseCanExecuteChanged();
        }
    }
}