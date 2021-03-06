using DBManager.Tables;
using Main.ViewModels;
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

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var testUser = new Users()
            { 
                id = 1,
                _level = 1,
                _user = "admin"
            };

            DataContext = new MainWindowViewModel(testUser);
        }
        public MainWindow(Users user)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(user);
        }
    }
}
