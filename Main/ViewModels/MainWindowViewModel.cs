using DBManager.Tables;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnityCake;
using UnityCake.Events;

namespace Main.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        public event PropertyChangedEventHandler PropertyChanged;

        #region -- PRIVATE --
        private Users LoggedUser { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel(Users user)
        {
            LoggedUser = user;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- HELPERS --
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
        #endregion
    }
}
