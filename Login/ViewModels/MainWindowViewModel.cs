using DBManager;
using Login.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnityCake.Events;

namespace Login.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ConnectCommand => new DefaultCommand(ConnectAction);

        private string _user;
        public string User
        {
            get { return _user; }
            set { _user = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User))); }
        }

        public string Password { private get; set; }

        #region -- PRIVATE --
        #endregion

        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel()
        {
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- ACTIONS --
        private void ConnectAction()
        {
            var user = DbClient.CheckUser(User, Password);
            if (user!=null)
            {
                new Main.MainWindow(user).Show();
                UnityCake.Unity.EventAggregator.GetEvent<CloseLoginDialogEvent>().Publish();
            }
            else
            {
                MessageBox.Show("Failed to log in");
            }
        }
        #endregion
        #endregion
    }
}
