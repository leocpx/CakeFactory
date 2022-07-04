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

        private UserControl _adminPlanningDisplay { get; set; }
        private UserControl _workerPlanningDisplay { get; set; }
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
            _ea.GetEvent<ReplySecondMenuSetterEvent>().Subscribe(sms => _secondMenuSetter = sms);
            _ea.GetEvent<ReplyDisplaySetterEvent>().Subscribe(dms => _displayMenuSetter = dms);
            _ea.GetEvent<AskLoggedUser>().Subscribe(() => _ea.GetEvent<ReplyLoggedUser>().Publish(_loggedUser));
            _ea.GetEvent<AskRawGoodsInfoEvent>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyRawGoodsInfoEvent>().Publish(DBManager.DbClient.GetRawGoodsInfoList());
                });
            _ea.GetEvent<MenuItemClickedEvent>().Subscribe(ExecuteMenuItemClicked);
            _ea.GetEvent<AskUsersListEvent>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyUsersListEvent>().Publish(DBManager.DbClient.GetUserList());
                });


            _ea.GetEvent<RegisterNewUserEvent>().Subscribe(DbClient.RegisterNewUser);
            _ea.GetEvent<RegisterNewRawGoodInfoEvent>().Subscribe(DbClient.RegisterNewRawGoodInfo);
            _ea.GetEvent<RegisterNewFinishedGoodInfoEvent>().Subscribe(DbClient.RegisterNewFinishedGoodInfo);
            _ea.GetEvent<RegisterFinishedGoodsDetailsEvent>().Subscribe(DbClient.RegisterFinishedGoodsDetails);
            _ea.GetEvent<AskFInishedGoodInfoEvent>().Subscribe(
                () =>
                {
                    _ea.GetEvent<ReplyFinishedGoodInfoEvent>().Publish(DbClient.GetFinishedGoodInfoList());
                });
            _ea.GetEvent<FinishedGoodCategoryItemClickedEvent>().Subscribe(
                category =>
                {
                    _secondMenuSetter(new FinishedGoodListView(category));
                });

            _ea.GetEvent<RegisterNewProductionOrderEvent>().Subscribe(DbClient.RegisterNewProductionOrder);

            _ea.GetEvent<AskTodayOrdersEvent>().Subscribe(
                order =>
                {
                    var fgi = DbClient.GetFinishedGoodOrder(order.worker.id, order.startTime);

                    if(fgi!=null)
                    {
                        order.OrderRecipe = fgi;
                        _ea.GetEvent<ReplyTodayOrdersEvent>().Publish(order);
                    }    
                });

            _ea.GetEvent<AskDeleteOrderEvent>().Subscribe(
                order =>
                {
                    DbClient.DeleteOrder(order.worker.id,order.startTime);
                });

            _ea.GetEvent<AskForProductionOrderEvent>().Subscribe(
                askParam =>
                {
                    var order = DbClient.GetProductionOrder(askParam.worker.id, askParam.startTime);
                    _ea.GetEvent<ReplyProductionOrderEvent>().Publish(order);
                });
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
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("WORKER SCHEDULE");
                    _workerPlanningDisplay = _workerPlanningDisplay == null ? new WorkerProductionPlanningView() : _workerPlanningDisplay;
                    _displayMenuSetter(_workerPlanningDisplay);
                    break;

                case MenuItems.production_planning:
                    _secondMenuSetter(new SecondMenuControlView());
                    _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
                    _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("FINISHED GOODS");
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("WORKER SCHEDULES");
                    _ea.GetEvent<SetMainMenuHeaderEvent>().Publish("FINISHED GOODS CATEGORIES");
                    _ea.GetEvent<SetMainMenuItemsEvent>().Publish(GetCategoryFinishedGoodMenuItems());

                    _adminPlanningDisplay = _adminPlanningDisplay == null ? new AdminProductionPlanningView() : _adminPlanningDisplay;
                    _displayMenuSetter(_adminPlanningDisplay);
                    //_secondMenuSetter(new FinishedGoodListView());
                    break;

                case MenuItems.account_administration:
                    ManageAccountAdministration();
                    break;

                case MenuItems.inventory_management:
                    _secondMenuSetter(new SecondMenuControlView());
                    _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
                    _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("INVENTORY MANAGEMENT");
                    break;

                case MenuItems.database_management:
                    _secondMenuSetter(new SecondMenuControlView());
                    _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("DATABASE MANAGEMENT");
                    _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
                    _ea.GetEvent<SetSecondMenuItems>().Publish(GetDatabaseManagementItems());
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

                case MenuItems.create_new_account:
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW ACCOUNT");
                    _displayMenuSetter(new CreateNewAccountView());
                    break;

                case MenuItems.program_settings:
                    _secondMenuSetter(new SecondMenuControlView());
                    _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
                    _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("PROGRAM SETTINGS");
                    _ea.GetEvent<SetSecondMenuItems>().Publish(GetProgramSettingsItems());
                    break;

                case MenuItems.logout:
                    _ea.GetEvent<CloseMainWindowEvent>().Publish();
                    break;


                case MenuItems.register_new_raw_goods:
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW RAW GOOD");
                    _displayMenuSetter(new CreateNewRawGoods());

                    break;

                case MenuItems.register_new_finished_goods:
                    _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW FINISHED GOOD");
                    _displayMenuSetter(new CreateNewFinishedGoods());
                    break;
                default:
                    break;
            }
        }
        #region -- execute menu item clicked functions --
        private List<UserControl> GetCategoryFinishedGoodMenuItems()
        {
            var finishedGoods = DbClient.GetFinishedGoodInfoList().Select(fg=>fg._category).Distinct();
            var result = finishedGoods.Select(fg =>(UserControl) new CategoryFinishedGoodItem(fg)).ToList();
            result.Add(new MenuItemView(CoreCake.MenuItems.back_to_mainmenu));

            return result;
        }


        private List<UserControl> GetDatabaseManagementItems()
        {
            return new List<UserControl>()
            {
                new MenuItemView(CoreCake.MenuItems.register_new_raw_goods),
                new MenuItemView(CoreCake.MenuItems.register_new_finished_goods),
                new MenuItemView(CoreCake.MenuItems.modify_raw_good_info),
                new MenuItemView(CoreCake.MenuItems.modify_finished_good_info),
                new MenuItemView(CoreCake.MenuItems.delete_raw_good_info),
                new MenuItemView(CoreCake.MenuItems.delete_finished_good_info),
                new MenuItemView(CoreCake.MenuItems.close_secondMenu),
            };
        }
        private List<UserControl> GetProgramSettingsItems()
        {
            switch (_loggedUser._level)
            {
                // ADMINISTRATOR MENU ITEMS
                case 1:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.sql_connection_settings),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu),
                    };

                // OPERATOR MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu),
                    };
                default:
                    return null;
            }
        }
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
                        new MenuItemView(CoreCake.MenuItems.database_management),
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
