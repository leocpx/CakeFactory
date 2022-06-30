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
    /// Interaction logic for WorkerScheduleView.xaml
    /// </summary>
    public partial class WorkerScheduleView : UserControl
    {
        public WorkerScheduleView(Users user)
        {
            InitializeComponent();
            DataContext = new WorkerScheduleViewModel(user);
        }
    }
}
