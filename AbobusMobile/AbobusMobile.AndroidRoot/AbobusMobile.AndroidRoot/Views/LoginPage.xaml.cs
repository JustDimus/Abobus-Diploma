﻿using AbobusMobile.AndroidRoot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AbobusMobile.AndroidRoot.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BasePage
    {
        public LoginPage()
            : base()
        {
            InitializeComponent();
        }
    }
}