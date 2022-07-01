using CoreCake;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.MenuItems
{
    public class CategoryFinishedGoodItemViewModel : GUIEntity
    {
        #region -- PROPETIES --
        #region -- BINDED --
        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; RaisePropertyChanged(nameof(CategoryName)); }
        }

        #region -- ICOMMANDS --
        public ICommand ClickedCommand => new DefaultCommand(ClickedAction, () => true);
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CategoryFinishedGoodItemViewModel(string CategoryName) : base()
        {
            this.CategoryName = CategoryName;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMAND ACTIONS --
        private void ClickedAction()
        {
            _ea.GetEvent<FinishedGoodCategoryItemClickedEvent>().Publish(CategoryName);
        }
        #endregion
        #endregion
        #endregion
    }
}
