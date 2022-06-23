using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Login.Models
{
    public class DefaultCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _a;

        public DefaultCommand(Action a)
        {
            _a = a;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _a?.Invoke();
        }
    }
}
