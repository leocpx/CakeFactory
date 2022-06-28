using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoreCake
{
    public class DefaultCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _defaultAction { get; set; }
        private Func<bool> canExecute { get; set; }
        public DefaultCommand(Action _a, Func<bool> _canExecute)
        {
            _defaultAction = _a;
            canExecute = _canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool CanExecute(object parameter) => canExecute();

        public void Execute(object parameter)
        {
            _defaultAction?.Invoke();
        }
    }
}
