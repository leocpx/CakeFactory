using DBManager.Tables;
using Main.ViewModels.Displays.Items;
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

namespace Main.Views.Displays.Items
{
    /// <summary>
    /// Interaction logic for FinishedGoodScheduleItemView.xaml
    /// </summary>
    public partial class FinishedGoodScheduleItemView : UserControl
    {
        public FinishedGoodScheduleItemView(FinishedGoodsInfo fgi, DateTime startTime)
        {
            InitializeComponent();
            DataContext = new FinishedGoodScheduleItemViewModel(fgi, startTime) { ownerControl = this };
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var command = ((FinishedGoodScheduleItemViewModel)DataContext).RemoveCommand;

            command.Execute(null);
        }
    }
}
