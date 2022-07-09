using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.ViewModels.Displays
{
    public class AdminProductionPlanningViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> WorkerScheduleList { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public AdminProductionPlanningViewModel() : base()
        {
            WorkerScheduleList = GenerateWorkerControls();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GenerateWorkerControls()
        {
            var result = new ObservableCollection<UserControl>();

            _ea.GetEvent<ReplyUsersListEvent>().Subscribe(
                users =>
                {
                    var userControls = users.Where(user=>user._active && user._level==2).Select(user => new WorkerScheduleView(user));
                    result = new ObservableCollection<UserControl>(userControls);
                });

            _ea.GetEvent<AskUsersListEvent>().Publish();

            return result;
        }
        #endregion
        #endregion
    }
}
