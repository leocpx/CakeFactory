using CoreCake;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.MenuItems
{
    public class CustomizableIngredientItemViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _ingredientName;
        public string IngredientName
        {
            get { return _ingredientName; }
            set { _ingredientName = value; RaisePropertyChanged(nameof(IngredientName)); }
        }


        private string _quantity = "1";
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

        private int _selectedUnitIndex = 0;
        public int SelectedUnitIndex
        {
            get { return _selectedUnitIndex; }
            set { _selectedUnitIndex = value; RaisePropertyChanged(nameof(SelectedUnitIndex)); }
        }


        public ICommand RemoveItemCommand => new DefaultCommand(RemoveItemAction, () => true);
        #endregion
        #region -- CORE --
        public RawGoodsInfo _RawGoodsInfo { get; set; }
        #endregion
        #endregion
        #region -- PRIVATE --
        public UserControl _userControl { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CustomizableIngredientItemViewModel(RawGoodsInfo rgi) : base()
        {
            _RawGoodsInfo = rgi;

            IngredientName = rgi._rawgoodname;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATRE --
        #region -- ICOMMANDS ACTIONS --
        private void RemoveItemAction()
        {
            _ea.GetEvent<RemoveIngredientItemFromRecipeEvent>().Publish(_userControl);
        }
        #endregion
        #endregion
        #endregion
    }
}
