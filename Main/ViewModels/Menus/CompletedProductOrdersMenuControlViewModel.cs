using Main.ViewModels.Menus.abstracts;
using Main.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private ObservableCollection<UserControl> GetCompletedProducts()
        {
            var completedProducts = DBManager.DbClient.GetCompletedProductOrders().Select(fg=>new CompletedProductItemView(fg));
            return new ObservableCollection<UserControl>(completedProducts);
        }
        #endregion
        #endregion
    }
}
