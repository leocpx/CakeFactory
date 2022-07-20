using Main.Views.dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main.Models
{
    public class ConfirmationDialogCommand : ICommand
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        public event EventHandler CanExecuteChanged;

        #endregion
        #region -- PRIVATE --
        private Action _executingAction;
        private string _dialogText;
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public ConfirmationDialogCommand(string dialogText, Action executingAction)
        {
            _executingAction = executingAction;
            _dialogText = dialogText;
        }
        #endregion

        #region -- FUNCTIONS --
        #region -- PUBLIC --

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            new ConfirmationDialogView(_dialogText, _executingAction).Show();
        }
        #endregion
        #region -- PRIVATE --

        #endregion
        #endregion

    }
}
