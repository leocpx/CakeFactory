using Core.Interfaces;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.MenuItems
{
    public class CompletedProductItemViewModel : GUIEntity, IScheduleItem
    {
        #region -- PROPERTIES --
        #region -- BINDED --
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; RaisePropertyChanged(nameof(ProductName)); }
        }

        #endregion
        #region -- CORE --
        public ProductionOrders ProductionOrder { get; set; }
        public IOrder Order { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CompletedProductItemViewModel(DBManager.Tables.ProductionOrders po) : base()
        {
            Order = po;
            ProductName = po._FinishedGoodsInfo.First()._finishedGoodName;
            ProductionOrder = po;
        }
        #endregion
    }
}
