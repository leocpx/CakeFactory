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
    /// Interaction logic for RawGoodItemView.xaml
    /// </summary>
    public partial class RawGoodItemView : UserControl
    {
        public RawGoodsInfo _RawGoodsInfo => ((RawGoodItemViewModel)DataContext)._RawGoodsInfo;

        public RawGoodItemView(string rawGoodName)
        {
            InitializeComponent();
            DataContext = new RawGoodItemViewModel(rawGoodName);
        }

        public RawGoodItemView(RawGoodsInfo rgi)
        {
            InitializeComponent();
            DataContext = new RawGoodItemViewModel(rgi);
        }
    }
}
