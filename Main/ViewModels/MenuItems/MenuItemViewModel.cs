using CoreCake;
using MahApps.Metro.IconPacks;
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
    public class MenuItemViewModel : GUIEntity
    {
        #region -- PROPERTIES --

        #region -- PUBLIC --

        #region -- BINDED --
        private PackIconBoxIconsKind _iconKind;
        public PackIconBoxIconsKind IconKind
        {
            get { return _iconKind; }
            set { _iconKind = value; RaisePropertyChanged(nameof(IconKind)); }
        }


        private string _itemDisplayName;
        public string ItemDisplayName
        {
            get { return _itemDisplayName; }
            set { _itemDisplayName = value; RaisePropertyChanged(nameof(ItemDisplayName)); }
        }

        public ICommand ItemClickCommand => new DefaultCommand(ItemClickedAction, () => true);
        #endregion
        #endregion

        #region -- PRIVATE --
        private CoreCake.MenuItems _menuItem;
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MenuItemViewModel(CoreCake.MenuItems menuItem, PackIconBoxIconsKind icoKind) : base()
        {
            IconKind = icoKind;
            ItemDisplayName = CoreCake.Core.GetDisplayName(menuItem);
            _menuItem = menuItem;
        }
        #endregion

        #region -- FUNCTIONS --
        private void ItemClickedAction()
        {
            _ea.GetEvent<MenuItemClickedEvent>().Publish(_menuItem);
        }
        #endregion
    }
}
