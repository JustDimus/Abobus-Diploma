using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public StartViewModel()
        {

        }

        public override async void OnPageAppeared()
        {
            await Shell.Current.GoToAsync("//login");
        }
    }
}
