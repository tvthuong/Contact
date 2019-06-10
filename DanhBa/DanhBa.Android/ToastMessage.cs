using Android.Widget;
using Xamarin.Forms;
[assembly: Dependency(typeof(DanhBa.Droid.ToastMessage))]
namespace DanhBa.Droid
{
    class ToastMessage : IToastMessage
    {
        public void LongTime(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
        public void ShortTime(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}