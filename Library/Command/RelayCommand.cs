using System;
using System.Windows.Input;

namespace Library.Command
{
    public sealed class RelayCommand : ICommand
    {
        public delegate bool CanExecuteDelegate(object parameter);

        private readonly Action<object> _action;
        private readonly CanExecuteDelegate _canExecuteDelegate;

        public RelayCommand(Action<object> action, CanExecuteDelegate canExecuteDelegate = null)
        {
            _action = action;
            _canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteDelegate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
