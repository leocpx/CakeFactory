using CoreCake;
using DBManager.Tables;
using Main.Views.MenuItems;
using Main.Views.Menus;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        private string _userDisplayName;
        public string UserDisplayName
        {
            get { return _userDisplayName; }
            set { _userDisplayName = value; RaisePropertyChanged(nameof(UserDisplayName)); }
        }


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


        private UserControl _mainDisplay;

        public UserControl MainDisplay
        {
            get { return _mainDisplay; }
            set { _mainDisplay = value; RaisePropertyChanged(nameof(MainDisplay)); }
        }


        private string _mainMenuHeader = "MAIN MENU";
        public string MainMenuHeader
        {
            get { return _mainMenuHeader; }
            set { _mainMenuHeader = value; RaisePropertyChanged(nameof(MainMenuHeader)); }
        }


        private string _secondMenuHeader = "OPTIONS";
        public string SecondMenuHeader
        {
            get { return _secondMenuHeader; }
            set { _secondMenuHeader = value; RaisePropertyChanged(nameof(SecondMenuHeader)); }
        }


        private string _displayHeader = "CONTROL PANEL";
        public string DisplayHeader
        {
            get { return _displayHeader; }
            set { _displayHeader = value; RaisePropertyChanged(nameof(DisplayHeader)); }
        }


        private int _mainMenuColumnWidth = 262;
        public int MainMenuColumnWidth
        {
            get { return _mainMenuColumnWidth; }
            set { _mainMenuColumnWidth = value;RaisePropertyChanged(nameof(MainMenuColumnWidth)); }
        }


        private int _secondMenuColumnWidth = 0;
        public int SecondMenuColumnWidth
        {
            get { return _secondMenuColumnWidth; }
            set { _secondMenuColumnWidth = value; RaisePropertyChanged(nameof(SecondMenuColumnWidth)); }
        }



        #endregion
        #region -- PRIVATE --
        private int _secondMenuMaxColumnWidth = 262;
        private int _animationDelay = 1;
        private Models.EventManager _eventManager { get; set; }
        private Users LoggedUser { get; set; }
        private EventAggregator _ea { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel(Users user)
        {
            LoggedUser = user;
            UserDisplayName = LoggedUser._user;
            _ea = UnityCake.Unity.EventAggregator;
            _ea.GetEvent<AskSecondSetterMenuEvent>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplySecondMenuSetterEvent>().Publish((v) => SecondMenu=v);
                });
            _ea.GetEvent<AskDisplayMenuSetterEvent>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyDisplaySetterEvent>().Publish((v) => MainDisplay = v);
                });
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
            _ea.GetEvent<SetMainMenuItemsEvent>().Publish(_eventManager.GetMainMenuItems());

            _ea.GetEvent<SetMainMenuHeaderEvent>().Subscribe(s => MainMenuHeader = s);
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Subscribe(s => SecondMenuHeader = s);
            _ea.GetEvent<SetDisplayHeaderEvent>().Subscribe(s => DisplayHeader = s);


            _ea.GetEvent<ExpandSecondMenuEvent>().Subscribe(ExpandSecondMenuColumn);
            _ea.GetEvent<ShrinkSecondMenuEvent>().Subscribe(ShrinkSecondMenuColumn);

            _ea.GetEvent<CloseMainWindowEvent>().Subscribe(
                ()=>
                {
                    var mainApp = Directory.GetCurrentDirectory() + "\\Login.exe";
                    Application.Current.Dispatcher.Invoke(() => Process.Start(mainApp));
                    Application.Current.Dispatcher.Invoke(App.Current.Shutdown);
                });
        }

        #endregion
        #region -- ANIMATIONS --
        private void ExpandSecondMenuColumn()
        {
            new Thread(
                () =>
                {
                    Console.WriteLine("starting expansion animation thread");
                    for (int i = 0; i < _secondMenuMaxColumnWidth; i+=2)
                    {
                        SecondMenuColumnWidth = i;
                        Thread.Sleep(_animationDelay);
                    }
                }
                ).Start();
        }
        private void ShrinkSecondMenuColumn()
        {
            new Thread(
                () =>
                {
                    for (int i = _secondMenuMaxColumnWidth; i >0 ; i--)
                    {
                        SecondMenuColumnWidth = i;
                        Thread.Sleep(_animationDelay);
                    }
                }
                ).Start();
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
