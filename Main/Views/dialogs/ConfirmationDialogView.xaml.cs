using Main.ViewModels.dialogs;
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
using System.Windows.Shapes;

namespace Main.Views.dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationDialogView.xaml
    /// </summary>
    public partial class ConfirmationDialogView : Window
    {
        public ConfirmationDialogView(string dialogText, Action executeAction)
        {
            InitializeComponent();
            DataContext = new ConfirmationDialogViewModel(dialogText, executeAction, CloseAction);
        }

        private void CloseAction()
        {
            Close();
        }
    }
}
