using CoreCake;
using DBManager;
using DBManager.Tables;
using Main.Views.Displays;
using Main.Views.MenuItems;
using Main.Views.Menus;
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
        private Action<UserControl> _secondMenuSetter { get; set; }
        private Action<UserControl> _displayMenuSetter { get; set; }
        private SubscriptionToken _subscriptionToken1 { get; set; }
        private SubscriptionToken _subscriptionToken2 { get; set; }
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
            #region -- SUBSCRIPTIONS --
            _ea.GetEvent<ReplySecondMenuSetterEvent>().Subscribe(
                sms =>
        {
            _secondMenuSetter = sms;
        });

            _ea.GetEvent<ReplyDisplaySetterEvent>().Subscribe(
                dms =>
                {
                    _displayMenuSetter = dms;
                });

            _ea.GetEvent<AskLoggedUser>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyLoggedUser>().Publish(_loggedUser);
                });

            _ea.GetEvent<MenuItemClickedEvent>().Subscribe(ExecuteMenuItemClicked);

            _ea.GetEvent<AskUsersList>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyUsersList>().Publish(DBManager.DbClient.GetUserList());
                });

            _ea.GetEvent<RegisterNewUserEvent>().Subscribe(DbClient.RegisterNewUser);
            #endregion


            #region -- PUBLISHES --

            _ea.GetEvent<AskSecondSetterMenuEvent>().Publish();
            _ea.GetEvent<AskDisplayMenuSetterEvent>().Publish(); 
            #endregion

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

                case MenuItems.close_secondMenu:
                    _secondMenuSetter(null);
                    _ea.GetEvent<ShrinkSecondMenuEvent>().Publish();
                    break;

                case MenuItems.logout:
                    _ea.GetEvent<CloseMainWindowEvent>().Publish();
                    break;

                case MenuItems.create_new_account:
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW ACCOUNT");
                    _displayMenuSetter(new CreateNewAccountView());
                    break;
                default:
                    break;
            }
        }
        #region -- execute menu item clicked functions --
        private void ManageAccountAdministration()
        {
            _secondMenuSetter(new SecondMenuControlView());
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("ACCOUNT ADMINISTRATION");
            _ea.GetEvent<SetSecondMenuItems>().Publish(GetAccountAdministrationSubItems());
        }
        private List<UserControl> GetAccountAdministrationSubItems()
        {
            switch (_loggedUser._level)
            {
                // ADMINISTRATOR MENU ITEMS
                case 1:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.create_new_account),
                        new MenuItemView(CoreCake.MenuItems.modify_account),
                        new MenuItemView(CoreCake.MenuItems.delete_account),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu),
                    };

                // OPERATOR MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.modify_account),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu),
                    };
                default:
                    return null;
            }
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
                        new MenuItemView(CoreCake.MenuItems.logout),
                    };

                // OPERATOR MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.schedules),
                        new MenuItemView(CoreCake.MenuItems.account_administration),
                        new MenuItemView(CoreCake.MenuItems.inventory_management),
                        new MenuItemView(CoreCake.MenuItems.logout),
                    };

                default:
                    return null;
            }
        }
        #endregion
    }
}
