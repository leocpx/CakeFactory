using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.MenuItems
{
    public class CompletedProductItemViewModel : GUIEntity
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
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CompletedProductItemViewModel(DBManager.Tables.ProductionOrders fg) : base()
        {
            ProductionOrder = fg;
            ProductName = fg._FinishedGoodsInfo.First()._finishedGoodName;
        }
        #endregion
    }
}
