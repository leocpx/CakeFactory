using CoreCake;
using DBManager.Tables;
using Main.ViewModels.Menus.abstracts;
using Main.Views.dialogs;
using Main.Views.Displays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UnityCake.Events;

namespace Main.ViewModels.Displays
{
    public class CreateNewRawGoodsViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        #region -- BINDED --
        private string _rawGoodName;
        public string RawGoodName
        {
            get { return _rawGoodName; }
            set { _rawGoodName = value; RaisePropertyChanged(nameof(RawGoodName)); }
        }


        private string _barCode;
        public string Barcode
        {
            get { return _barCode; }
            set { _barCode = value; RaisePropertyChanged(nameof(Barcode)); }
        }


        private string _pricePerPiece;
        public string PricePerPiece
        {
            get { return _pricePerPiece; }
            set { _pricePerPiece = value; RaisePropertyChanged(nameof(PricePerPiece)); }
        }


        private string _orderFromCoName;
        public string OrderFromCoName
        {
            get { return _orderFromCoName; }
            set { _orderFromCoName = value; RaisePropertyChanged(nameof(OrderFromCoName)); }
        }


        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); }
        }


        private string _units;
        public string Units
        {
            get { return _units; }
            set { _units = value; RaisePropertyChanged(nameof(Units)); }
        }



        private UserControl _createdRawGoods;
        public UserControl CreatedRawGoods
        {
            get { return _createdRawGoods; }
            set { _createdRawGoods = value; RaisePropertyChanged(nameof(CreatedRawGoods)); }
        }


        #region -- ICOMMANDS --
        public ICommand CreateNewRawGoodCommand => new DefaultCommand(CreateNewRawGoodAction, () => true);
        #endregion
        #endregion
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public CreateNewRawGoodsViewModel() : base()
        {
            CreatedRawGoods = new CreatedRawGoodsList();
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PRIVATE --
        #region -- ICOMMAND ACTIONS --
        private void CreateNewRawGoodAction()
        {
            Action _executeAction = () =>
           {
               var newRawGood = new RawGoodsInfo()
               {
                   _barcode = Barcode,
                   _orderfromconame = OrderFromCoName,
                   _phonenumber = PhoneNumber,
                   _priceperpiece = PricePerPiece,
                   _rawgoodname = RawGoodName,
                   _units = Units.Split(':')[1],
               };

               _ea.GetEvent<RegisterNewRawGoodInfoEvent>().Publish(newRawGood);
               _ea.GetEvent<AskRawGoodsInfoEvent>().Publish();
           };

            new ConfirmationDialogView($"Please confirm registration of new raw ingredient\n{RawGoodName}", _executeAction).Show();
        }
        #endregion
        #endregion
        #endregion
    }
}
