using CoreCake;
using DBManager.Tables;
using Main.Views.MenuItems;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.Models
{
    public class EventManager
    {
        #region -- PROPERTIES --

        #region -- PRIVATE --
        private Users _loggedUser { get; set; }
        private EventAggregator _ea { get; set; }
        private Func<UserControl> _secondMenuGetter { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public EventManager(Users user)
        {
            _loggedUser = user;
            _ea = UnityCake.Unity.EventAggregator;

            InitEventService();
        }
        #endregion


        #region -- EVENT SERVICES --
        private void InitEventService()
        {
            _ea.GetEvent<AskSecondMenuEvent>().Publish();
            _ea.GetEvent<ReplySecondMenuEvent>().Subscribe(
                sme =>
                {
                    _secondMenuGetter = sme;
                });

            _ea.GetEvent<AskLoggedUser>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyLoggedUser>().Publish(_loggedUser);
                });

            _ea.GetEvent<MenuItemClickedEvent>().Subscribe(ExecuteMenuItemClicked);
        }
        #endregion

        #region -- HELPERS --
        /// <summary>
        /// Called when one of the MAIN MENU ITEMS IS CLICKED
        /// </summary>
        /// <param name="itemName"></param>
        private void ExecuteMenuItemClicked(MenuItems itemName)
        {
            switch (itemName)
            {
                case MenuItems.schedules:
                    break;
                case MenuItems.production_planning:
                    break;
                case MenuItems.account_administration:
                    ManageAccountAdministration();
                    break;
                case MenuItems.inventory_management:
                    break;
                case MenuItems.reports:
                    break;
                case MenuItems.sales:
                    break;
                case MenuItems.back_to_mainmenu:
                    _ea.GetEvent<SetMainMenuItemsEvent>().Publish(GetMainMenuItems());
                    _ea.GetEvent<ShrinkSecondMenuEvent>().Publish();
                    break;

                case MenuItems.logout:
                    _ea.GetEvent<CloseMainWindowEvent>().Publish();
                    break;
                default:
                    break;
            }
        }
        #region -- execute menu item clicked functions --
        private void ManageAccountAdministration()
        {
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("ACCOUNT ADMINISTRATION");
        }
        #endregion

        public List<UserControl> GetMainMenuItems()
        {
            switch (_loggedUser._level)
            {
                // ADMINISTRATOR MENU ITEMS
                case 1:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.production_planning),
                        new MenuItemView(CoreCake.MenuItems.inventory_management),
                        new MenuItemView(CoreCake.MenuItems.account_administration),
                        new MenuItemView(CoreCake.MenuItems.program_settings),
                        new MenuItemView(CoreCake.MenuItems.reports),
                        new MenuItemView(CoreCake.MenuItems.sales),
                    };

                // OPERATOR MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.schedules),
                        new MenuItemView(CoreCake.MenuItems.account_administration),
                        new MenuItemView(CoreCake.MenuItems.inventory_management),
                    };

                default:
                    return null;
            }
        }
        #endregion
    }
}
