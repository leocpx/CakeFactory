using CoreCake;
using DBManager;
using DBManager.Tables;
using MahApps.Metro.IconPacks;
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

        private UserControl _adminProductionPlanningDisplay { get; set; }
        private UserControl _adminPackagingPlanningDisplay { get; set; }
        private UserControl _workerPlanningDisplay { get; set; }
        private Dictionary<MenuItems, Action> MenuItemActions { get; set; }
        private Dictionary<int, List<UserControl>> MainMenuItems { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public EventManager(Users user)
        {
            _loggedUser = user;
            _ea = UnityCake.Unity.EventAggregator;

            InitEventService();
            InitMainMenuItems();
            InitMenuItemActions();
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

            _ea.GetEvent<RegisterNewOrderEvent>().Subscribe(
                order =>
                {
                    var worker = DbClient.GetUser(order._workerId);

                    if (worker != null)
                    {
                        if (worker._level == 2)
                            DbClient.RegisterNewProductionOrder(order);

                        if (worker._level == 3)
                            DbClient.RegisterNewPackagingOrder(order);
                    }
                });

            _ea.GetEvent<AskTodayOrdersEvent>().Subscribe(
                askParam =>
                {
                    var fgi = DbClient.GetFinishedGoodInfo(askParam.worker.id, askParam.startTime);

                    if (fgi != null)
                    {
                        askParam.FinishedGoodInfo = fgi;
                        askParam.productionOrder = DbClient.GetProductionOrder(askParam.worker.id, askParam.startTime);

                        _ea.GetEvent<ReplyTodayOrdersEvent>().Publish(askParam);
                        return;
                    }

                    var po = DbClient.GetPackagingOrder(askParam.worker.id, askParam.startTime);
                    if (po != null)
                    {
                        askParam.packagingOrder = po;
                        askParam.FinishedGoodInfo = po._FinishedGoodsInfo.FirstOrDefault();
                        _ea.GetEvent<ReplyTodayOrdersEvent>().Publish(askParam);
                        return;
                    }
                });

            _ea.GetEvent<AskDeleteOrderEvent>().Subscribe(
                order =>
                {
                    DbClient.DeleteOrder(order);
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

        private void InitMainMenuItems()
        {
            MainMenuItems = new Dictionary<int, List<UserControl>>()
            {
                { 1, new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.production_planning, PackIconBoxIconsKind.SolidFactory),
                        new MenuItemView(CoreCake.MenuItems.packaging_planning, PackIconBoxIconsKind.RegularPackage),
                        new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
                        new MenuItemView(CoreCake.MenuItems.database_management, PackIconBoxIconsKind.RegularFoodMenu),
                        new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
                        new MenuItemView(CoreCake.MenuItems.program_settings, PackIconBoxIconsKind.SolidWrench),
                        new MenuItemView(CoreCake.MenuItems.reports, PackIconBoxIconsKind.RegularBarChartSquare),
                        new MenuItemView(CoreCake.MenuItems.sales, PackIconBoxIconsKind.RegularLineChart),
                        new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
                    }},

                { 2, new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.schedules, PackIconBoxIconsKind.RegularListOl),
                        new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
                        new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
                        new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
                    }},

                { 3, new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.schedules, PackIconBoxIconsKind.RegularListOl),
                        new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
                        new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
                        new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
                    }},
            };
        }

        private void InitMenuItemActions()
        {
            MenuItemActions = new Dictionary<MenuItems, Action>()
            {
                { MenuItems.schedules, SchedulesItemAction },
                { MenuItems.production_planning, ProductionPlanningAction },
                { MenuItems.packaging_planning, PackagingPlanningAction },
                { MenuItems.account_administration, AccountAdministrationAction },
                { MenuItems.inventory_management, InventoryManagementAction },
                { MenuItems.database_management, DatabaseManagementAction },
                { MenuItems.reports, ReportsAction },
                { MenuItems.sales, SalesAction },
                { MenuItems.back_to_mainmenu, BackToMainMenuAction },
                { MenuItems.close_secondMenu, CloseSecondMenuAction },
                { MenuItems.create_new_account, CreateNewAccountAction },
                { MenuItems.program_settings, ProgramSettingsAction },
                { MenuItems.logout, LogoutAction },
                { MenuItems.register_new_finished_goods, RegisterNewFinishedGoodsAction },
                { MenuItems.register_new_raw_goods, RegisterNewRawGoodsAction },
                { MenuItems.register_new_packaging_supply, RegisterNewRawGoodsAction },
                { MenuItems.scan_raw_goods, ScanRawGoodsAction },
                { MenuItems.scan_pacaking_supplies, ScanPackagingSuppliesAction },
                { MenuItems.scan_finished_good, ScanFinishedGoodsAction },
            };
        }

        #endregion

        #region -- HELPERS --
        /// <summary>
        /// Called when one of the MAIN MENU ITEMS IS CLICKED
        /// </summary>
        /// <param name="itemName"></param>
        private void ExecuteMenuItemClicked(MenuItems itemName)
        {
            Action _action;
            MenuItemActions.TryGetValue(itemName, out _action);
            _action?.Invoke();

            #region -- obsolete --
            //switch (itemName)
            //{
            //    case MenuItems.schedules:
            //        SchedulesItemAction();
            //        break;

            //    case MenuItems.production_planning:
            //        ProductionPlanningAction();
            //        break;

            //    case MenuItems.packaging_planning:
            //        PackagingPlanningAction();
            //        break;

            //    case MenuItems.account_administration:
            //        AccountAdministrationAction();
            //        break;

            //    case MenuItems.inventory_management:
            //        InventoryManagementAction();
            //        break;

            //    case MenuItems.database_management:
            //        DatabaseManagementAction();
            //        break;

            //    case MenuItems.reports:
            //        ReportsAction();
            //        break;

            //    case MenuItems.sales:
            //        SalesAction();
            //        break;

            //    case MenuItems.back_to_mainmenu:
            //        BackToMainMenuAction();
            //        break;

            //    case MenuItems.close_secondMenu:
            //        CloseSecondMenuAction();
            //        break;

            //    case MenuItems.create_new_account:
            //        CreateNewAccountAction();
            //        break;

            //    case MenuItems.program_settings:
            //        ProgramSettingsAction();
            //        break;

            //    case MenuItems.logout:
            //        LogoutAction();
            //        break;


            //    case MenuItems.register_new_raw_goods:
            //        RegisterNewRawGoodsAction();

            //        break;

            //    case MenuItems.register_new_finished_goods:
            //        RegisterNewFinishedGoodsAction();
            //        break;
            //    default:
            //        break;
            //} 
            #endregion
        }

        #region -- execute menu item clicked helpers --

        private void ScanRawGoodsAction()
        {
            throw new NotImplementedException();
        }

        private void ScanFinishedGoodsAction()
        {
            throw new NotImplementedException();
        }

        private void ScanPackagingSuppliesAction()
        {
            throw new NotImplementedException();
        }

        private void SalesAction()
        {
            throw new NotImplementedException();
        }

        private void ReportsAction()
        {
            throw new NotImplementedException();
        }

        private void RegisterNewFinishedGoodsAction()
        {
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW FINISHED GOOD");
            _displayMenuSetter(new CreateNewFinishedGoods());
        }

        private void RegisterNewRawGoodsAction()
        {
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW RAW GOOD");
            _displayMenuSetter(new CreateNewRawGoods());
        }

        private void LogoutAction()
        {
            _ea.GetEvent<CloseMainWindowEvent>().Publish();
        }

        private void ProgramSettingsAction()
        {
            _secondMenuSetter(new SecondMenuControlView());
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("PROGRAM SETTINGS");
            _ea.GetEvent<SetSecondMenuItems>().Publish(GetProgramSettingsItems());
        }

        private void CreateNewAccountAction()
        {
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("CREATE NEW ACCOUNT");
            _displayMenuSetter(new CreateNewAccountView());
        }

        private void CloseSecondMenuAction()
        {
            _secondMenuSetter(null);
            _ea.GetEvent<ShrinkSecondMenuEvent>().Publish();
        }

        private void BackToMainMenuAction()
        {
            _ea.GetEvent<SetMainMenuItemsEvent>().Publish(GetMainMenuItems());
            _ea.GetEvent<ShrinkSecondMenuEvent>().Publish();
        }

        private void DatabaseManagementAction()
        {
            _secondMenuSetter(new SecondMenuControlView());
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("DATABASE MANAGEMENT");
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuItems>().Publish(GetDatabaseManagementItems());
        }

        private void InventoryManagementAction()
        {
            _secondMenuSetter(new SecondMenuControlView());
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("INVENTORY MANAGEMENT");
            _ea.GetEvent<SetSecondMenuItems>().Publish(GetInventoryMenuManagementItems());
        }

        private void PackagingPlanningAction()
        {
            _secondMenuSetter(new CompletedProductOrdersMenuControlView());
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("READY FOR PACKAGING");
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("PACKAGING SCHEDULES");

            _adminPackagingPlanningDisplay = _adminPackagingPlanningDisplay == null ? new AdminPackagingPlanningView() : _adminPackagingPlanningDisplay;
            _displayMenuSetter(_adminPackagingPlanningDisplay);
        }

        private void ProductionPlanningAction()
        {
            _secondMenuSetter(new SecondMenuControlView());
            _ea.GetEvent<ExpandSecondMenuEvent>().Publish();
            _ea.GetEvent<SetSecondMenuHeaderEvent>().Publish("FINISHED GOODS");
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("PRODUCTION SCHEDULES");
            _ea.GetEvent<SetMainMenuHeaderEvent>().Publish("FINISHED GOODS CATEGORIES");
            _ea.GetEvent<SetMainMenuItemsEvent>().Publish(GetCategoryFinishedGoodMenuItems());

            _adminProductionPlanningDisplay = _adminProductionPlanningDisplay == null ? new AdminProductionPlanningView() : _adminProductionPlanningDisplay;
            _displayMenuSetter(_adminProductionPlanningDisplay);
        }

        private void SchedulesItemAction()
        {
            _ea.GetEvent<SetDisplayHeaderEvent>().Publish("YOUR SCHEDULE FOR TODAY");
            //_workerPlanningDisplay = _workerPlanningDisplay == null ? new WorkerProductionPlanningView() : _workerPlanningDisplay;
            _workerPlanningDisplay = new WorkerProductionPlanningView();
            _displayMenuSetter(_workerPlanningDisplay);
        }

        private List<UserControl> GetInventoryMenuManagementItems()
        {
            return new List<UserControl>()
            {
                new MenuItemView(MenuItems.scan_raw_goods, PackIconBoxIconsKind.RegularQrScan),
                new MenuItemView(MenuItems.scan_pacaking_supplies, PackIconBoxIconsKind.RegularQrScan),
                new MenuItemView(MenuItems.scan_finished_good, PackIconBoxIconsKind.RegularQrScan),
            };
        }
        #region -- execute menu item clicked functions --
        private List<UserControl> GetCategoryFinishedGoodMenuItems()
        {
            var finishedGoods = DbClient.GetFinishedGoodInfoList().Select(fg => fg._category).Distinct();
            var result = finishedGoods.Select(fg => (UserControl)new CategoryFinishedGoodItem(fg)).ToList();
            result.Add(new MenuItemView(CoreCake.MenuItems.back_to_mainmenu, PackIconBoxIconsKind.RegularArrowBack));

            return result;
        }

        private List<UserControl> GetDatabaseManagementItems()
        {
            return new List<UserControl>()
            {
                new MenuItemView(CoreCake.MenuItems.register_new_raw_goods, PackIconBoxIconsKind.RegularCookie),
                new MenuItemView(CoreCake.MenuItems.register_new_finished_goods, PackIconBoxIconsKind.SolidCake),
                new MenuItemView(CoreCake.MenuItems.modify_raw_good_info, PackIconBoxIconsKind.SolidEdit),
                new MenuItemView(CoreCake.MenuItems.modify_finished_good_info, PackIconBoxIconsKind.SolidEdit),
                new MenuItemView(CoreCake.MenuItems.delete_raw_good_info, PackIconBoxIconsKind.RegularX),
                new MenuItemView(CoreCake.MenuItems.delete_finished_good_info, PackIconBoxIconsKind.RegularX),
                new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
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
                        new MenuItemView(CoreCake.MenuItems.sql_connection_settings, PackIconBoxIconsKind.SolidData),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };

                // PRODUCTION USER MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };

                // PACKAGING USER MENU ITEMS
                case 3:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };

                default:
                    return null;
            }
        }
        private void AccountAdministrationAction()
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
                        new MenuItemView(CoreCake.MenuItems.create_new_account, PackIconBoxIconsKind.SolidUserAccount),
                        new MenuItemView(CoreCake.MenuItems.modify_account, PackIconBoxIconsKind.RegularCreditCardFront),
                        new MenuItemView(CoreCake.MenuItems.delete_account, PackIconBoxIconsKind.RegularX),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };

                // PRODUCTION USER MENU ITEMS
                case 2:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.modify_account, PackIconBoxIconsKind.RegularCreditCardFront),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };

                // PACKAGING USER MENU ITEMS
                case 3:
                    return new List<UserControl>()
                    {
                        new MenuItemView(CoreCake.MenuItems.modify_account, PackIconBoxIconsKind.RegularCreditCardFront),
                        new MenuItemView(CoreCake.MenuItems.close_secondMenu, PackIconBoxIconsKind.RegularArrowBack),
                    };
                default:
                    return null;
            }
        }

        #endregion

        #endregion
        public List<UserControl> GetMainMenuItems()
        {
            List<UserControl> result;
            MainMenuItems.TryGetValue(_loggedUser._level, out result);
            return result;

            #region -- obsolete --
            //switch (_loggedUser._level)
            //{
            //    // ADMINISTRATOR MENU ITEMS
            //    case 1:
            //        return new List<UserControl>()
            //        {
            //            new MenuItemView(CoreCake.MenuItems.production_planning, PackIconBoxIconsKind.SolidFactory),
            //            new MenuItemView(CoreCake.MenuItems.packaging_planning, PackIconBoxIconsKind.RegularPackage),
            //            new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
            //            new MenuItemView(CoreCake.MenuItems.database_management, PackIconBoxIconsKind.RegularFoodMenu),
            //            new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
            //            new MenuItemView(CoreCake.MenuItems.program_settings, PackIconBoxIconsKind.SolidWrench),
            //            new MenuItemView(CoreCake.MenuItems.reports, PackIconBoxIconsKind.RegularBarChartSquare),
            //            new MenuItemView(CoreCake.MenuItems.sales, PackIconBoxIconsKind.RegularLineChart),
            //            new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
            //        };

            //    // PRODUCTION WORKER MENU ITEMS
            //    case 2:
            //        return new List<UserControl>()
            //        {
            //            new MenuItemView(CoreCake.MenuItems.schedules, PackIconBoxIconsKind.RegularListOl),
            //            new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
            //            new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
            //            new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
            //        };

            //    // PACKAGING WORKER MENU ITEMS
            //    case 3:
            //        return new List<UserControl>()
            //        {
            //            new MenuItemView(CoreCake.MenuItems.schedules, PackIconBoxIconsKind.RegularListOl),
            //            new MenuItemView(CoreCake.MenuItems.account_administration, PackIconBoxIconsKind.SolidUserAccount),
            //            new MenuItemView(CoreCake.MenuItems.inventory_management, PackIconBoxIconsKind.RegularQr),
            //            new MenuItemView(CoreCake.MenuItems.logout, PackIconBoxIconsKind.RegularLogOut),
            //        };

            //    default:
            //        return null;
            //} 
            #endregion
        }
        #endregion
    }
}
