using DBManager;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.MenuItems
{
    public class CompletedFinishedGoodItemViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _finishedGoodItemName;
        private ProductionOrders good;

        public string FinishedGoodItemName
        {
            get { return _finishedGoodItemName; }
            set { _finishedGoodItemName = value; RaisePropertyChanged(nameof(FinishedGoodItemName)); }
        }

        #endregion
        #endregion
        #region -- CORE --
        public ProductionOrders _ProductionOrders { get; set; }
        public FinishedGoodsInfo _FinishedGoodsInfo { get; set; }   
        public DateTime _StartTime { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --

        public CompletedFinishedGoodItemViewModel(ProductionOrders order, DateTime startTime) : base()
        {
            _ProductionOrders = order;
            _StartTime = startTime;

            _FinishedGoodsInfo = DbClient.GetFinishedGoodInfo(order._finishedGoodId);
            FinishedGoodItemName = _FinishedGoodsInfo._finishedGoodName;
        }
        #endregion
    }
}
