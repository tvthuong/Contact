using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(DanhBa.UWP.Settings))]
namespace DanhBa.UWP
{
    class Settings : ISettings
    {
        async void ISettings.OpenManageApplicationsSettings()
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures"));
        }
    }
}
