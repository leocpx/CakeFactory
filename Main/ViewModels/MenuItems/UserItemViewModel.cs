using CoreCake;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.MenuItems
{
    public class UserItemViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- BINDED --
        private string _userName;
        public ICommand UserClickedCommand => new DefaultCommand(UserClickedAction, () => true);
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(nameof(UserName)); }
        }

        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public UserItemViewModel(string name)
        {
            UserName = name;
        }

        #endregion

        #region -- FUNCTIONS --
        #region -- ICOMMAND ACTIONS --
        private void UserClickedAction()
        {
            _ea.GetEvent<UserItemCLickedEvent>().Publish(UserName);
        }
        #endregion
        #endregion
    }
}
