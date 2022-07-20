using CoreCake;
using DBManager.Tables;
using Main.Models;
using Main.ViewModels.Menus.abstracts;
using Main.Views.dialogs;
using Main.Views.Displays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.Displays
{
    public class CreateNewAccountViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- BINDED --


        private UserControl _existingUsersControl;
        public UserControl ExistingUsersControl
        {
            get { return _existingUsersControl; }
            set { _existingUsersControl = value; RaisePropertyChanged(nameof(ExistingUsersControl)); }
        }


        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { _password = value; UpdateCreateButton(); RaisePropertyChanged(nameof(Password)); }
        }


        private string _fullName = "";
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; UpdateCreateButton(); RaisePropertyChanged(nameof(FullName)); }
        }


        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                UserNameErrorVisibility = UserNameExists(value) ? Visibility.Visible : Visibility.Hidden;
                UpdateCreateButton();
                RaisePropertyChanged(nameof(UserName));
            }
        }


        private int _userLevel;
        public int UserLevel
        {
            get { return _userLevel; }
            set { _userLevel = value; UpdateCreateButton(); RaisePropertyChanged(nameof(UserLevel)); }
        }


        private Visibility _userNameErrorVisibility = Visibility.Hidden;
        public Visibility UserNameErrorVisibility
        {
            get { return _userNameErrorVisibility; }
            set { _userNameErrorVisibility = value; RaisePropertyChanged(nameof(UserNameErrorVisibility)); }
        }


        #region -- ICOMMANDS --
        public ICommand CreateNewUserCommand => new ConfirmationDialogCommand($"Please confirm user {UserName}", CreateNewUserAction);
        #endregion
        #endregion

        #region -- PRIVATE --
        private List<Users> UserList { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CreateNewAccountViewModel() : base()
        {
            ExistingUsersControl = new ExistingAccountsList();

            _ea.GetEvent<ReplyUsersListEvent>().Subscribe(users => UserList = users);
            _ea.GetEvent<AskUsersListEvent>().Publish();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMAND ACTIONS --
        private void CreateNewUserAction()
        {
            var newUser = new Users()
            {
                _user = UserName,
                _fullname = FullName,
                _pass = GetHASH256(Password),
                _level = UserLevel + 1,
            };

            _ea.GetEvent<RegisterNewUserEvent>().Publish(newUser);
            _ea.GetEvent<UpdateExistingUserListEvent>().Publish();
            _ea.GetEvent<AskUsersListEvent>().Publish();
            UpdateCreateButton();
        }
        #region -- HELPERS --
        private void UpdateCreateButton()
        {
            RaisePropertyChanged(nameof(CreateNewUserCommand));
        }
        private bool CanCreateNewUser()
        {
            var condition1 = !UserNameExists(UserName);
            var condition2 = FullName != "";
            var condition3 = Password != "";
            var result = condition1 == condition2 == condition3 == true;
            return result;
        }
        private string GetHASH256(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            byte[] resultedData;
            SHA256 shaM = new SHA256Managed();
            resultedData = shaM.ComputeHash(data);

            var result = BitConverter.ToString(resultedData);

            return result;
        }
        #endregion
        #endregion
        private bool UserNameExists(string userName)
        {
            return UserList.Any(u => u._user.ToUpper().Equals(userName.ToUpper()));
        }

        #endregion
        #endregion
    }
}
