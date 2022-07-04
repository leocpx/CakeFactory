using CoreCake;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
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
    public class FinishedGoodScheduleItemViewModel : GUIEntity
    {

        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _finishedGoodName;
        public string FinishedGoodName
        {
            get { return _finishedGoodName; }
            set { _finishedGoodName = value; RaisePropertyChanged(nameof(FinishedGoodName)); }
        }

        private string _timeFrame;
        public string TimeFrame
        {
            get { return _timeFrame; }
            set { _timeFrame = value; }
        }


        private Visibility _canDelete = Visibility.Visible;
        public Visibility CanDelete
        {
            get { return _canDelete; }
            set { _canDelete = value; RaisePropertyChanged(nameof(CanDelete)); }
        }


        #region -- ICOMMANDS --
        public ICommand RemoveCommand => new DefaultCommand(RemoveAction, () => true);
        public ICommand ClickedCommand => new DefaultCommand(ClickedAction, () => true);
        #endregion
        #endregion

        #region -- CORE --
        public DateTime StartTime { get; set; }
        public FinishedGoodsInfo _FinishedGoodInfo { get; set; }
        public UserControl ownerControl { get; set; }
        public ProductionOrders ProductionOrder { get; set; }
        #endregion
        #endregion
        #endregion
        #region -- CONSTRUCTOR --
        public FinishedGoodScheduleItemViewModel(FinishedGoodsInfo fgi, DateTime startTime) : base()
        {

            if (CurrentUser._level > 1)
                CanDelete = Visibility.Hidden;

            StartTime = startTime;
            _FinishedGoodInfo = fgi;
            FinishedGoodName = fgi._finishedGoodName;

            var start = StartTime.ToString("HH:mm");
            var end = StartTime.AddMinutes(30).ToString("HH:mm");

            TimeFrame = $"{start} - {end}";


            if (CurrentUser._level > 1)
            {
                _ea.GetEvent<ReplyProductionOrderEvent>().Subscribe(
                    order =>
                    {
                        if (order._finishedGoodId == _FinishedGoodInfo.id &&
                            order._startTime == long.Parse(StartTime.ToString("HHmm")))
                        {
                            ProductionOrder = order;
                        }
                    }
                    );

                _ea.GetEvent<AskForProductionOrderEvent>().Publish(
                    new AskOrderParams()
                    {
                        OrderRecipe = fgi,
                        startTime = long.Parse(StartTime.ToString("HHmm")),
                        worker = CurrentUser
                    });
            }
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMAND ACITONS --
        private void RemoveAction()
        {
            _ea.GetEvent<RemoveFinishedGoodScheduleItemEvent>().Publish(ownerControl);
        }
        private void ClickedAction()
        {
            if (CurrentUser._level > 1)
                _ea.GetEvent<OrderClickedEvent>().Publish(ProductionOrder);
        }
        #endregion
        #endregion
        #endregion
    }
}
