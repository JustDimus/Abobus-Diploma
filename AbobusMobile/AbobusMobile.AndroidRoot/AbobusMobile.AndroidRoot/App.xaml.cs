using AbobusMobile.AndroidRoot.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AbobusMobile.AndroidRoot
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
