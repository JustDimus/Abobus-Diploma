using AbobusMobile.AndroidRoot.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//main/browse");
            });
        }

        public ICommand OpenWebCommand { get; }
    }
}