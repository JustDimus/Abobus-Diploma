// See https://aka.ms/new-console-template for more information
using AbobusMobile.AndroidRoot.ViewModels;

Console.WriteLine("Hello, World!");

var appProvider = new ApplicationServiceProvider();

var profile = appProvider.ProfileViewModel;
profile.ToString();