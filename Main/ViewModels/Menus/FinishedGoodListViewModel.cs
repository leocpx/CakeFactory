using Main.ViewModels.Menus.abstracts;
using Main.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.ViewModels.MenuItems
{
    public class FinishedGoodListViewModel : GUIEntity
    {

        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> FinishedGoodItems { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public FinishedGoodListViewModel() :base()
        {
            FinishedGoodItems = GenerateFinishedGoodItemControls();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GenerateFinishedGoodItemControls()
        {
            var result = new ObservableCollection<UserControl>();
            _ea.GetEvent<ReplyFinishedGoodInfoEvent>().Subscribe(
                finishedGoods =>
                {
                    var finishedGoodList = finishedGoods.Select(fg => new FinishedGoodItemView(fg));
                    result = new ObservableCollection<UserControl>(finishedGoodList);
                });

            _ea.GetEvent<AskFInishedGoodInfoEvent>().Publish();
            return result;
        }
        #endregion
        #endregion
    }
}
