using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.MenuItems
{
    public class FinishedGoodItemViewModel : GUIEntity
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

        #endregion
        #region -- CORE --
        public FinishedGoodsInfo _FinishedGoodInfo { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public FinishedGoodItemViewModel(FinishedGoodsInfo fgi) :base()
        {
            _FinishedGoodInfo = fgi;
            _finishedGoodName = fgi._finishedGoodName;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --

        #endregion
        #endregion
    }
}
