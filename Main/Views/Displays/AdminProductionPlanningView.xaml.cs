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
    /// Interaction logic for AdminProductionPlanningView.xaml
    /// </summary>
    public partial class AdminProductionPlanningView : UserControl
    {
        public AdminProductionPlanningView()
        {
            InitializeComponent();
            DataContext = new AdminProductionPlanningViewModel();
        }
    }
}
