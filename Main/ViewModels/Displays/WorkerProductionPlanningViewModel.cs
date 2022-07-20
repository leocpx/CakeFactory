using Core.Interfaces;
using CoreCake;
using DBManager;
using DBManager.Tables;
using Main.Models;
using Main.ViewModels.Menus.abstracts;
using Main.Views.dialogs;
using Main.Views.Displays.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.Displays
{
    public class WorkerProductionPlanningViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private UserControl _workerPlanningContent;
        public UserControl WorkerPlanningContent
        {
            get { return _workerPlanningContent; }
            set { _workerPlanningContent = value; RaisePropertyChanged(nameof(WorkerPlanningContent)); }
        }

        private int _detailColumnWidth = 0;
        public int DetailColumnWidth
        {
            get { return _detailColumnWidth; }
            set { _detailColumnWidth = value;RaisePropertyChanged(nameof(DetailColumnWidth)); }
        }

        public ObservableCollection<UserControl> RecipeIngredients { get; set; }

        private string _finishedGoodName;
        public string FinishedGoodName
        {
            get { return _finishedGoodName; }
            set { _finishedGoodName = value; RaisePropertyChanged(nameof(FinishedGoodName)); }
        }

        //public ICommand CompleteOrderCommand => new DefaultCommand(CompleteOrderAction, () => true);
        public ICommand CompleteOrderCommand => new ConfirmationDialogCommand("Please confirm you want to complete the order.",CompleteOrderAction);
        #endregion
        #region -- CORE --
        public IOrder Order { get; set; }
        #endregion
        #endregion
        #region -- PRIVATE --
        #region -- CORE --
        private int _maxColumnWidth { get; set; } = 500;
        private int _animationDelay = 1;
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public WorkerProductionPlanningViewModel() : base()
        {
            WorkerPlanningContent = GenerateWorkerPlanningItem();

            _ea.GetEvent<ProductionOrderClickedEvent>().Subscribe(
                order =>
                {
                    Order = order;
                    if (DetailColumnWidth==0)
                    {
                        new Thread(() =>
                        {
                            for (int i = 0; i < _maxColumnWidth; i += 2)
                            {
                                DetailColumnWidth = i;
                                Thread.Sleep(_animationDelay);
                            }
                        }).Start();
                    }

                    var _order = order as ProductionOrders;
                    RecipeIngredients = GetRecipeIngredientItems(_order._finishedGoodId);
                    RaisePropertyChanged(nameof(RecipeIngredients));
                    FinishedGoodName = DbClient.GetFinishedGoodInfo(order._finishedGoodId)._finishedGoodName;
                });

            _ea.GetEvent<PackagingOrderClickedEvent>().Subscribe(
                order =>
                {
                    Order = order;
                    if (DetailColumnWidth == 0)
                    {
                        new Thread(() =>
                        {
                            for (int i = 0; i < _maxColumnWidth; i += 2)
                            {
                                DetailColumnWidth = i;
                                Thread.Sleep(_animationDelay);
                            }
                        }).Start();
                    }

                    var _order = order as PackagingOrders;

                    RecipeIngredients = GetRecipeIngredientItems(_order._finishedGoodId);
                    RaisePropertyChanged(nameof(RecipeIngredients));
                    FinishedGoodName = DbClient.GetFinishedGoodInfo(_order._finishedGoodId)._finishedGoodName;
                });
        }
        private ObservableCollection<UserControl> GetRecipeIngredientItems(long finishedGoodId)
        {
            var finishedGoodDetails = DbClient.GetFinishedGoodDetails(finishedGoodId);
            var ingredients = finishedGoodDetails.Select(
                fg =>
                {
                    var rawGoodInfo = DbClient.GetRawGoodsInfo(fg._rawGoodId);
                    return new RecipeItemView()
                    {
                        RawGoodName = rawGoodInfo._rawgoodname,
                        Quantity = fg._quantity.ToString(),
                        Unit = fg._unit,
                    };
                });

            return new ObservableCollection<UserControl>(ingredients);
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMANDS --
        private void CompleteOrderAction()
        {
            //Action _executeAction = () =>
            //{
                if (Order != null)
                {
                    if (CurrentUser._level == 2)
                    {
                        DbClient.CompleteOrder<ProductionOrders>(Order);
                        _ea.GetEvent<CompleteOrderEvent>().Publish(Order);
                    }

                    if (CurrentUser._level == 3)
                    {
                        DbClient.CompleteOrder<PackagingOrders>(Order);
                        _ea.GetEvent<CompleteOrderEvent>().Publish(Order);
                    }
                }
            //};

            //new ConfirmationDialogView("Please confirm you want to complete the order", _executeAction).Show();
        }
        #endregion
        private UserControl GenerateWorkerPlanningItem()
        {
            return new WorkerScheduleView(CurrentUser);
        }
        #endregion
        #endregion
    }
}
