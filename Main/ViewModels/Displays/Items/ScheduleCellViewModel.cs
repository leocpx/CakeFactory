using Core.Interfaces;
using CoreCake;
using DBManager;
using DBManager.Tables;
using GongSolutions.Wpf.DragDrop;
using Main.ViewModels.MenuItems;
using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays.Items;
using Main.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.Displays.Items
{
    public class ScheduleCellViewModel : GUIEntity, IDropTarget
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _timeFrame;
        public string TimeFrame
        {
            get { return _timeFrame; }
            set { _timeFrame = value; RaisePropertyChanged(TimeFrame); }
        }

        private UserControl _nestedItem;
        public UserControl NestedItem
        {
            get { return _nestedItem; }
            set { _nestedItem = value; RaisePropertyChanged(nameof(NestedItem)); }
        }

        #region -- ICOMMANDS --
        #endregion
        #endregion
        #region -- CORE --
        public Users Worker { get; set; }
        public DateTime StartTime { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public ScheduleCellViewModel(DateTime timeFrame, Users _worker) : base()
        {
            Worker = _worker;
            StartTime = timeFrame;

            var start = timeFrame.ToString("HH:mm");
            var end = timeFrame.AddMinutes(30).ToString("HH:mm");

            TimeFrame = $"{start} - {end}";

            _ea.GetEvent<CompleteOrderEvent>().Subscribe(
                order =>
                {
                    if (NestedItem == null) return;
                    var item = NestedItem.DataContext as IScheduleItem;
                    
                    if( item.Order.id == order.id)
                    {
                        NestedItem = null;
                    }    
                });

            _ea.GetEvent<RemoveFinishedGoodScheduleItemEvent>().Subscribe(
                _userControl =>
                {
                    if(NestedItem == _userControl)
                    {
                        var orderUserControl = NestedItem.DataContext as IScheduleItem;
                        NestedItem = null;

                        var orderToDelete = new AskOrderParams()
                        {
                            worker = Worker,
                            startTime = long.Parse(StartTime.ToString("HHmm"))
                        };


                        _ea.GetEvent<AskDeleteOrderEvent>().Publish(orderUserControl.Order);
                    }
                });


            _ea.GetEvent<ReplyTodayOrdersEvent>().Subscribe(
                askParam =>
                {
                    var completed = askParam.productionOrder != null ? askParam.productionOrder._completed : askParam.packagingOrder._completed;
                    if (askParam.startTime == long.Parse(StartTime.ToString("HHmm")) && askParam.worker.id == Worker.id && !completed)
                    {
                        NestedItem = new FinishedGoodScheduleItemView(askParam.FinishedGoodInfo, StartTime);
                        var _nestedVm = NestedItem.DataContext as IScheduleItem;
                        _nestedVm.Order = DbClient.GetOrder(askParam.FinishedGoodInfo, Worker.id, StartTime.ToString("HHmm"));
                    }
                });

            _ea.GetEvent<AskTodayOrdersEvent>().Publish(
                new AskOrderParams()
                {
                    worker = Worker,
                    startTime = long.Parse(StartTime.ToString("HHmm"))
                });
        }

        #region -- DRAG METHODS --
        public void DragEnter(IDropInfo dropInfo)
        {
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (NestedItem != null)
                dropInfo.Effects = DragDropEffects.None;
            else
            {
                dropInfo.Effects = DragDropEffects.Copy;
                var isTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter) && dropInfo.VisualTargetItem is TreeViewItem;
                dropInfo.DropTargetAdorner = isTreeViewItem ? DropTargetAdorners.Highlight : DropTargetAdorners.Insert;
            }
        }

        public void DragLeave(IDropInfo dropInfo)
        {
        }

        public void Drop(IDropInfo dropInfo)
        {
            IOrder newOrder = null;
            var productionData = dropInfo.Data as FinishedGoodItemView;
            var packagingData = dropInfo.Data as CompletedProductItemView;

            if (productionData!=null)
                newOrder = GetNewProductionOrder(productionData);

            if (packagingData != null)
            {
                newOrder = GetNewPackagingOrder(packagingData);
                _ea.GetEvent<PackagingOrderAssignedToWorkerEvent>().Publish(packagingData);
            }

            _ea.GetEvent<RegisterNewOrderEvent>().Publish(newOrder);
        }

        private IOrder GetNewPackagingOrder(CompletedProductItemView productionData)
        {
            IOrder newOrder;
            var vm = ((CompletedProductItemViewModel)productionData.DataContext);
            var _fgId = vm.Order._finishedGoodId;
            var fgi = DbClient.GetFinishedGoodInfo(_fgId);

            NestedItem = new FinishedGoodScheduleItemView(fgi, StartTime);

            newOrder = new PackagingOrders()
            {
                _completed = false,
                _finishedGoodId = fgi.id,
                _startTime = long.Parse(StartTime.ToString("HHmm")),
                _workerId = Worker.id,
                _productionOrderId = vm.ProductionOrder.id
            };

            var dataContext = NestedItem.DataContext as IScheduleItem;
            dataContext.Order = newOrder;

            return newOrder;
        }

        private IOrder GetNewProductionOrder(FinishedGoodItemView productionData)
        {
            IOrder newOrder;
            var fgi = ((FinishedGoodItemViewModel)productionData.DataContext)._FinishedGoodInfo;

            NestedItem = new FinishedGoodScheduleItemView(fgi, StartTime);

            newOrder = new ProductionOrders()
            {
                _completed = false,
                _finishedGoodId = fgi.id,
                _startTime = long.Parse(StartTime.ToString("HHmm")),
                _workerId = Worker.id
            };
            return newOrder;
        }
        #endregion
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #endregion
        #endregion
    }
}
