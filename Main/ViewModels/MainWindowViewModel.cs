using CoreCake;
using DBManager.Tables;
using Main.Views.MenuItems;
using Main.Views.Menus;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UnityCake;
using UnityCake.Events;

namespace Main.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        #region -- BINDED
        public event PropertyChangedEventHandler PropertyChanged;

        
        private UserControl _mainMenu;
        public UserControl MainMenu
        {
            get { return _mainMenu; }
            set { _mainMenu = value; RaisePropertyChanged(nameof(MainMenu)); }
        }


        private UserControl _secondMenu;
        public UserControl SecondMenu
        {
            get { return _secondMenu; }
            set { _secondMenu = value; RaisePropertyChanged(nameof(SecondMenu)); }
        }



        #endregion
        #region -- PRIVATE --
        private Models.EventManager _eventManager { get; set; }
        private Users LoggedUser { get; set; }
        private EventAggregator _ea { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel(Users user)
        {
            LoggedUser = user;
            _ea = UnityCake.Unity.EventAggregator;
            _eventManager = new Models.EventManager(LoggedUser);
            InitMainWindowControls();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- CORE --
        private void InitMainWindowControls()
        {
            MainMenu = new MainMenuControlView();
            _ea.GetEvent<SetMainMenuItems>().Publish(_eventManager.GetMainMenuItems());
        }

        #endregion

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
