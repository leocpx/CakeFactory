using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays;
using Main.Views.MenuItems;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.ViewModels.Displays
{
    public class ExistingAccountsListViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- BINDED --
        public ObservableCollection<UserControl> UsersListViewItems { get; set; }

        #endregion

        #region -- PRIVATE --
        private EventAggregator _ea { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public ExistingAccountsListViewModel() : base()
        {
            _ea = UnityCake.Unity.EventAggregator;

            _ea.GetEvent<ReplyUsersList>().Subscribe(
                users =>
                {
                    UsersListViewItems = GenerateUserControls(users);
                    RaisePropertyChanged(nameof(UsersListViewItems));
                });

            _ea.GetEvent<UpdateExistingUserListEvent>().Subscribe(() => LoadUserList());

            LoadUserList();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PUBLIC --
        public void LoadUserList()
        {
            _ea.GetEvent<AskUsersList>().Publish();
        }
        #endregion
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GenerateUserControls(List<Users> users)
        {
            var usersList = users.Select(u => new UserItemView(u._user));
            return new ObservableCollection<UserControl>(usersList);
        }
        #endregion
        #endregion
    }
}
