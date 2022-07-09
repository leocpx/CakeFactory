using DBManager.Tables;
using Main.ViewModels.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Views.MenuItems
{
    /// <summary>
    /// Interaction logic for CompletedFinishedGoodItemView.xaml
    /// </summary>
    public partial class CompletedFinishedGoodItemView : UserControl
    {

        public CompletedFinishedGoodItemView(ProductionOrders good, DateTime startTime)
        {
            InitializeComponent();
            DataContext = new CompletedFinishedGoodItemViewModel(good, startTime);
        }

        public CompletedFinishedGoodItemView(ProductionOrders good)
        {
            InitializeComponent();
            DataContext = new CompletedFinishedGoodItemViewModel(good, startTime);
        }
    }
}
