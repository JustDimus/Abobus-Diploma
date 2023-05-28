using AbobusMobile.AndroidRoot.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.Views
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            this.Appearing += BasePage_Appeared;
            this.Disappearing += BasePage_Disappeared;
        }

        private void BasePage_Appeared(object sender, EventArgs e)
        {
            if (BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnPageAppeared();
            }
        }

        private void BasePage_Disappeared(object sender, EventArgs e)
        {
            if (BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnPageDisappeared();
            }
        }
    }
}
