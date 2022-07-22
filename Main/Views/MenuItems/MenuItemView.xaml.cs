using CoreCake;
using MahApps.Metro.IconPacks;
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
    /// Interaction logic for MenuItemView.xaml
    /// </summary>
    public partial class MenuItemView : UserControl
    {
        public MenuItemView(CoreCake.MenuItems displayName, PackIconBoxIconsKind kind)
        {
            InitializeComponent();
            DataContext = new MenuItemViewModel(displayName, kind);
        }

    }
}
