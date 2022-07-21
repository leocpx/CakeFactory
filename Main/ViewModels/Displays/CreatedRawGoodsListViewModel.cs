using DBManager.Tables;
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

namespace Main.ViewModels.Displays
{
    internal class CreatedRawGoodsListViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        public ObservableCollection<UserControl> RawGoodsListViewItems { get; set; }
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CreatedRawGoodsListViewModel() : base()
        {
            _ea.GetEvent<ReplyRawGoodsInfoEvent>().Subscribe(
                rawGoods =>
                {
                    RawGoodsListViewItems = GenerateRawGoodsControls(rawGoods);
                    RaisePropertyChanged(nameof(RawGoodsListViewItems));
                });

            _ea.GetEvent<AskRawGoodsInfoEvent>().Publish();
        }

        private ObservableCollection<UserControl> GenerateRawGoodsControls(List<RawGoodsInfo> rawGoods)
        {
            var usersList = rawGoods.Select(rg => new RawGoodItemView(rg._rawgoodname));
            return new ObservableCollection<UserControl>(usersList);
        }
        #endregion
    }
}
