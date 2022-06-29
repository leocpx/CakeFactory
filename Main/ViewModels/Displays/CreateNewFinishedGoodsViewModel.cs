using CoreCake;
using DBManager.Tables;
using GongSolutions.Wpf.DragDrop;
using Main.ViewModels.Menus.abstracts;
using Main.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels
{
    internal class CreateNewFinishedGoodsViewModel : GUIEntity, IDropTarget
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> ExistingIngredients { get; set; }
        public ObservableCollection<UserControl> CurrentRecipeListView { get; set; }

        private string _finishedGoodName = "NEW RECIPE NAME";
        public string FinishedGoodName
        {
            get { return _finishedGoodName; }
            set { _finishedGoodName = value; RaisePropertyChanged(nameof(FinishedGoodName)); }
        }



        private string _wholeSalePrice;
        public string WholeSalePrice
        {
            get { return _wholeSalePrice; }
            set { _wholeSalePrice = value; RaisePropertyChanged(nameof(WholeSalePrice)); }
        }



        private string _retailPrice;
        public string RetailPrice
        {
            get { return _retailPrice; }
            set { _retailPrice = value; RaisePropertyChanged(nameof(RetailPrice)); }
        }



        #region -- ICOMMANDS --
        public ICommand CreateFinishedGoodCommand => new DefaultCommand(CreateFinishedGoodAction, () => true);
        #endregion
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CreateNewFinishedGoodsViewModel() : base()
        {
            _ea.GetEvent<ReplyRawGoodsInfoEvent>().Subscribe(rg => GenerateRawGoodsItems(rg));
            _ea.GetEvent<RemoveIngredientItemFromRecipeEvent>().Subscribe(item => CurrentRecipeListView.Remove((UserControl)item));
            _ea.GetEvent<AskRawGoodsInfoEvent>().Publish();

            CurrentRecipeListView = new ObservableCollection<UserControl>();
        }


        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- UI BUILDER --
        private void GenerateRawGoodsItems(List<RawGoodsInfo> rg)
        {
            var rawGoodsItems = rg.Select(r => new RawGoodItemView(r));
            ExistingIngredients = new ObservableCollection<UserControl>(rawGoodsItems);
            RaisePropertyChanged(nameof(ExistingIngredients));
        }
        #endregion
        #region -- ICOMMANDS --
        private void CreateFinishedGoodAction()
        {

        }

        public void DragEnter(IDropInfo dropInfo)
        {

        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
            var isTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter) && dropInfo.VisualTargetItem is TreeViewItem;
            dropInfo.DropTargetAdorner = isTreeViewItem ? DropTargetAdorners.Highlight : DropTargetAdorners.Insert;
        }

        public void DragLeave(IDropInfo dropInfo)
        {
            Console.WriteLine();
        }

        public void Drop(IDropInfo dropInfo)
        {
            var droppedObject = dropInfo.Data as RawGoodItemView;
            var newIngredient = new CustomizableIngredientItem(droppedObject._RawGoodsInfo);
            CurrentRecipeListView.Add(newIngredient);
        }
        #endregion
        #endregion
        #endregion
    }
}
