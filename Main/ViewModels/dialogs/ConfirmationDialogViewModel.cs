using CoreCake;
using Main.ViewModels.Menus.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.ViewModels.dialogs
{
    public class ConfirmationDialogViewModel : GUIEntity
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        private string _dialogText;
        public string DialogText
        {
            get { return _dialogText; }
            set { _dialogText = value; RaisePropertyChanged(nameof(DialogText)); }
        }

        #region -- ICOMMANDS --
        public ICommand ConfirmCommand => new DefaultCommand(ConfirmAction, () => true);
        public ICommand CancelCommand => new DefaultCommand(CancelAction, () => true);
        #endregion
        #endregion
        #region -- PRIVATE --
        private Action ExecuteOnConfirmation { get; set; }
        private Action CloseAction { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public ConfirmationDialogViewModel(string dialogText, Action executeAction, Action closeAction)
        {
            DialogText = dialogText;
            ExecuteOnConfirmation = executeAction;
            CloseAction = closeAction;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PUBLIC --

        #endregion
        #region -- PRIVATE --
        #region -- ICOMMANDS ACTIONS --
        private void ConfirmAction()
        {
            ExecuteOnConfirmation?.Invoke();
            CloseAction?.Invoke();
        }

        private void CancelAction()
        {
            CloseAction?.Invoke();
        }
        #endregion
        #endregion
        #endregion
    }
}
