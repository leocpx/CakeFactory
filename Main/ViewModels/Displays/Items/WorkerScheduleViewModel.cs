using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Main.ViewModels.Displays.Items
{
    public class WorkerScheduleViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _workerName;
        public string WorkerName
        {
            get { return _workerName; }
            set { _workerName = value; RaisePropertyChanged(nameof(WorkerName)); }
        }

        public ObservableCollection<UserControl> ScheduleCells { get; set; }
        #endregion
        #region -- CORE --
        public Users User { get; set; }
        #endregion
        #endregion
        #region -- PRIVATE --

        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public WorkerScheduleViewModel(Users user) :base()
        {
            User = user;
            WorkerName = user._fullname;

            ScheduleCells = GenerateScheduleCells();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GenerateScheduleCells()
        {
            var start = new DateTime(1, 1, 1, 8, 00, 00);
            var end = new DateTime(1, 1, 1, 19, 0, 0);
            var result = new ObservableCollection<UserControl>();

            while(start<=end)
            {
                var newCell = new ScheduleCellView(start ,User);
                result.Add(newCell);
                start = start.AddMinutes(30);
            }

            return result;
        }
        #endregion
        #endregion
    }
}
