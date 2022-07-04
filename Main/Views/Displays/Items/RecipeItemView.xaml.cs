using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for RecipeItemView.xaml
    /// </summary>
    public partial class RecipeItemView : UserControl, INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        #region -- BINDED --
        private string _rawGoodName;

        public event PropertyChangedEventHandler PropertyChanged;
        public string RawGoodName
        {
            get { return _rawGoodName; }
            set { _rawGoodName = value; RaisePropertyChanged(nameof(RawGoodName)); }
        }

        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; RaisePropertyChanged(nameof(Quantity)); }
        }


        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; RaisePropertyChanged(nameof(Unit)); }
        }




        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public RecipeItemView()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
