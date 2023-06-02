using AbobusMobile.AndroidRoot.Services;
using AbobusMobile.AndroidRoot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AbobusMobile.AndroidRoot.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationPage : BasePage
    {
        public NavigationPage()
            : base()
        {
            InitializeComponent();

            var mapService = new MapManipulatorService(MainMap);

            var navigationViewModel = this.BindingContext as NavigationViewModel;

            navigationViewModel.MapService = mapService;
        }
    }
}