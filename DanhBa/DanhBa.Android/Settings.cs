using Xamarin.Forms;
[assembly: Dependency(typeof(DanhBa.Droid.Settings))]
namespace DanhBa.Droid
{
    public class Settings : ISettings
    {
        public void OpenManageApplicationsSettings()
        {
            Android.App.Application.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionManageApplicationsSettings));
        }
    }
}