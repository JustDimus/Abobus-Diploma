using AbobusMobile.AndroidRoot.Models;
using AbobusMobile.AndroidRoot.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AbobusMobile.AndroidRoot.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public virtual void OnPageAppeared() { }

        public virtual void OnPageDisappeared() { }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
