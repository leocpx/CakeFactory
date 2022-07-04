using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public WorkerProductionPlanningViewModel() : base()
        {
            WorkerPlanningContent = GenerateWorkerPlanningItem();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private UserControl GenerateWorkerPlanningItem()
        {
            return new WorkerScheduleView(CurrentUser);
        }
        #endregion
        #endregion
    }
}
