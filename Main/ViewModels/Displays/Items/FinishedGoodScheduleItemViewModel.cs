using CoreCake;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region -- ICOMMANDS --
        public ICommand RemoveCommand => new DefaultCommand(RemoveAction, () => true);

        #endregion
        #endregion

        #region -- CORE --
        public DateTime StartTime { get; set; }
        public FinishedGoodsInfo _FinishedGoodInfo { get; set; }
        public UserControl ownerControl { get; set; }
        #endregion
        #endregion
        #endregion
        #region -- CONSTRUCTOR --
        public FinishedGoodScheduleItemViewModel(FinishedGoodsInfo fgi, DateTime startTime) : base()
        {
            StartTime = startTime;
            _FinishedGoodInfo = fgi;
            FinishedGoodName = fgi._finishedGoodName;

            var start = StartTime.ToString("HH:mm");
            var end = StartTime.AddMinutes(30).ToString("HH:mm");

            TimeFrame = $"{start} - {end}";
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMAND ACITONS --
        private void RemoveAction()
        {
            _ea.GetEvent<RemoveFinishedGoodScheduleItemEvent>().Publish(ownerControl);
        }
        #endregion
        #endregion
        #endregion
    }
}
