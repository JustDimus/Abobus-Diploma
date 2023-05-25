using AbobusMobile.AndroidRoot.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}