using DBManager;
using GongSolutions.Wpf.DragDrop;
using Main.ViewModels.Menus.abstracts;
using Main.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UnityCake.Events;

namespace Main.ViewModels.Menus
{
    public class CompletedFinishedGoodsListViewModel : GUIEntity, IDropTarget
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> CompletedFinishedGoodItems { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CompletedFinishedGoodsListViewModel() : base()
        {
            _ea.GetEvent<UpdateCompletedGoodsMenuItemsEvent>().Subscribe(UpdateGoodsItemsList);
            UpdateGoodsItemsList();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PUBLIC --
        #region -- DRAG DROP HANDLERS --
        public void DragEnter(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void DragLeave(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
        #region -- PRIVATE --
        #region -- CORE --
        private void UpdateGoodsItemsList()
        {
            var goodsItemControls = DbClient.GetCompletedFinishedGoods()
                                            .Select(_good => new CompletedFinishedGoodItemView(_good, _good._startTime));

            CompletedFinishedGoodItems = new ObservableCollection<UserControl>(goodsItemControls);
            RaisePropertyChanged(nameof(CompletedFinishedGoodItems));
        }
        #endregion
        #endregion
        #endregion
    }
}
