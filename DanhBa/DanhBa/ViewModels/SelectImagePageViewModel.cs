using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace DanhBa.ViewModels
{
    public class SelectImagePageViewModel : ViewModelBase
    {
        private string _txtUrl;
        private bool IsValidUrl;
        private bool _isConnected;

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                SetProperty(ref _isConnected, value);
                RaisePropertyChanged(nameof(IsNotConnected));
                ((DelegateCommand)cmdOk).RaiseCanExecuteChanged();
            }
        }
        public bool IsNotConnected => !IsConnected;
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ((DelegateCommand)cmdOk).RaiseCanExecuteChanged();
            }
        }
        public string txtUrl
        {
            get => _txtUrl;
            set => SetProperty(ref _txtUrl, value);
        }
        public ICommand cmdOk { get; set; }
        public SelectImagePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = new Helpers.TranslateExtension() { Text = "SelectImagePage_Title" }.ProvideValue(null).ToString();
            cmdOk = new DelegateCommand(Ok, CanExecute);
            IsValidUrl = false;
            txtUrl = @"https://www.google.com/";
            CrossConnectivity.Current.ConnectivityChanged += (sender, e) => { IsConnected = CrossConnectivity.Current.IsConnected; };
            CrossConnectivity.Current.ConnectivityTypeChanged += (sender, e) => { IsConnected = CrossConnectivity.Current.IsConnected; };
        }

        private bool CanExecute()
        {
            return IsNotBusy && IsConnected;
        }
        private void CheckUrl()
        {
            var req = (HttpWebRequest)WebRequest.Create(txtUrl);
            req.Method = "HEAD";
            try
            {
                using (var resp = req.GetResponse())
                {
                    bool res = resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                        .StartsWith("image/");
                    if (res)
                        IsValidUrl = true;
                    else
                        IsValidUrl = false;
                }
            }
            catch(Exception)
            {
                IsValidUrl = false;
            }
        }
        private async void Ok()
        {
            IsBusy = true;
            CheckUrl();
            if (IsValidUrl)
            {
                await NavigationService.GoBackAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", new Helpers.TranslateExtension() { Text = "SelectImage_Error_Message" }.ProvideValue(null).ToString(), "Ok");
                IsBusy = false;
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (IsValidUrl)
                parameters.Add("PhotoUrl", txtUrl);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsConnected = CrossConnectivity.Current.IsConnected;
            IsBusy = false;
        }
    }
}
