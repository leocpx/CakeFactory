using Main.ViewModels.Displays;
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

namespace Main.Views.Displays
{
    /// <summary>
    /// Interaction logic for CreatedRawGoodsList.xaml
    /// </summary>
    public partial class CreatedRawGoodsList : UserControl
    {
        public CreatedRawGoodsList()
        {
            InitializeComponent();
            DataContext = new CreatedRawGoodsListViewModel();
        }
    }
}
