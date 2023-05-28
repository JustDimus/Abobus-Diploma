using AbobusMobile.AndroidRoot.Models;
using AbobusMobile.Database.Models;
using AbobusMobile.Database.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    //public class NewItemViewModel : BaseViewModel
    //{
    //    private readonly IRepository<ConfigurationModel> _configurations;

    //    private string text;
    //    private string description;

    //    public NewItemViewModel(
    //        IRepository<ConfigurationModel> configurationsRepository)
    //    {
    //        _configurations = configurationsRepository;

    //        SaveCommand = new Command(OnSave, ValidateSave);
    //        CancelCommand = new Command(OnCancel);
    //        this.PropertyChanged +=
    //            (_, __) => SaveCommand.ChangeCanExecute();
    //    }

    //    private bool ValidateSave()
    //    {
    //        return !String.IsNullOrWhiteSpace(text)
    //            && !String.IsNullOrWhiteSpace(description);
    //    }

    //    public string Text
    //    {
    //        get => text;
    //        set => SetProperty(ref text, value);
    //    }

    //    public string Description
    //    {
    //        get => description;
    //        set => SetProperty(ref description, value);
    //    }

    //    public Command SaveCommand { get; }
    //    public Command CancelCommand { get; }

    //    private async void OnCancel()
    //    {
    //        // This will pop the current page off the navigation stack
    //        await Shell.Current.GoToAsync("..");
    //    }

    //    private async void OnSave()
    //    {
    //        ConfigurationModel newModel = new ConfigurationModel()
    //        {
    //            Name = text,
    //            Value = Description
    //        };

    //        var entity = await _configurations.FirstOrDefaultAsync();

            
    //        try
    //        {
    //            var result = await _configurations.InsertAsync(newModel);
    //            result.ToString();
    //        }
    //        catch(Exception ex)
    //        {
    //            ex.ToString();
    //        }

    //        newModel.ToString();

    //        // This will pop the current page off the navigation stack
    //        await Shell.Current.GoToAsync("..");
    //    }
    //}
}
