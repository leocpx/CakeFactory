using Main.ViewModels.Menus.abstracts;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.ViewModels.Menus
{
    public class MainMenuControlViewModel : GUIEntity
    {

        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> MenuItems { get; set; }
        #endregion

        #endregion
        #region -- PRIVATE --
        private EventAggregator _ea { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainMenuControlViewModel()
        {
            _ea = UnityCake.Unity.EventAggregator;

            _ea.GetEvent<SetMainMenuItems>().Subscribe(
                items =>
                {
                    MenuItems = new ObservableCollection<UserControl>(items);
                    RaisePropertyChanged(nameof(MenuItems));
                });
        }
        #endregion
    }
}
