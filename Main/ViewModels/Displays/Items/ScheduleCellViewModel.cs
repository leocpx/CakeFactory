using CoreCake;
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


            _ea.GetEvent<RemoveFinishedGoodScheduleItemEvent>().Subscribe(
                item =>
                {
                    if(NestedItem == item)
                    {
                        NestedItem = null;

                        var orderToDelete = new AskOrderParams()
                        {
                            worker = Worker,
                            startTime = long.Parse(StartTime.ToString("HHmm"))
                        };

                        _ea.GetEvent<AskDeleteOrderEvent>().Publish(orderToDelete);
                    }
                });


            _ea.GetEvent<ReplyTodayOrdersEvent>().Subscribe(
                fgi =>
                {
                    if(fgi.startTime == long.Parse(StartTime.ToString("HHmm")) && fgi.worker.id == Worker.id)
                        NestedItem = new FinishedGoodScheduleItemView(fgi.OrderRecipe, StartTime);
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
            var data = dropInfo.Data as FinishedGoodItemView;
            var fgi = ((FinishedGoodItemViewModel)data.DataContext)._FinishedGoodInfo;

            NestedItem = new FinishedGoodScheduleItemView(fgi, StartTime);

            var newOrder = new ProductionOrders()
            {
                _completed = false,
                _finishedGoodId = fgi.id,
                _startTime = long.Parse(StartTime.ToString("HHmm")),
                _workerId = Worker.id
            };

            _ea.GetEvent<RegisterNewProductionOrderEvent>().Publish(newOrder);
        }
        #endregion
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #endregion
        #endregion
    }
}
