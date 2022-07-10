using DBManager.Tables;
using Main.ViewModels.Displays.Items;
using Main.ViewModels.MenuItems;
using Main.ViewModels.Menus.abstracts;
using Main.Views.Displays.Items;
using Main.Views.MenuItems;
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
    public class CompletedProductOrdersMenuControlViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> CompletedProductsList { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR  --
        public CompletedProductOrdersMenuControlViewModel() : base()
        {
            CompletedProductsList = GetCompletedProducts();

            _ea.GetEvent<PackagingOrderAssignedToWorkerEvent>().Subscribe(
                packageItemControl =>
                {
                    CompletedProductsList.Remove(packageItemControl);
                });

            _ea.GetEvent<RemoveFinishedGoodScheduleItemEvent>().Subscribe(
                userControl =>
                {
                    var data = userControl.DataContext as FinishedGoodScheduleItemViewModel;
                    var _order = data.Order as PackagingOrders;
                    var _productionOrder = DBManager.DbClient.GetCompletedProductOrders().Where(_p=>_p.id == _order._productionOrderId).FirstOrDefault();


                    Console.WriteLine();
                    var uc = new CompletedProductItemView(_productionOrder);
                    CompletedProductsList.Add(uc);
                });
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GetCompletedProducts()
        {

            var totalOrders = DBManager.DbClient.GetCompletedProductOrders();
            var assignedOrderIDs = DBManager.DbClient.GetPackagingOrders().Select(a=>a._productionOrderId);
            var result = new List<ProductionOrders>();

            foreach (var order in totalOrders)
                if(!assignedOrderIDs.Contains(order.id))
                    result.Add(order);


            var completedProducts = result.Select(fg => new CompletedProductItemView(fg));

            return new ObservableCollection<UserControl>(completedProducts);
        }
        #endregion
        #endregion
    }
}
