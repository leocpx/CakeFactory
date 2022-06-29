using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.MenuItems
{
    public class RawGoodItemViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --

        #region -- BINDED --
        private string _rawGoodName;
        private RawGoodsInfo rgi;

        public string RawGoodName
        {
            get { return _rawGoodName; }
            set { _rawGoodName = value; RaisePropertyChanged(nameof(RawGoodName)); }
        }

        #endregion

        #region -- CORE --
        public RawGoodsInfo _RawGoodsInfo { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public RawGoodItemViewModel(string rawGoodName) :base()
        {
            RawGoodName = rawGoodName;
        }

        public RawGoodItemViewModel(RawGoodsInfo rgi)
        {
            this._RawGoodsInfo = rgi;
            RawGoodName = rgi._rawgoodname;
        }
        #endregion
    }
}
