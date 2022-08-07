using System;
using System.Windows.Input;

namespace VisualFindReferences.Core.Graph.Helper
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action action)
        {
            _action = action;
        }

        private readonly Action _action;

        public event EventHandler? CanExecuteChanged;

        private bool _canExecuteCommand = true;

        public bool CanExecuteCommand
        {
            get { return _canExecuteCommand; }
            set
            {
                if (value != _canExecuteCommand)
                {
                    _canExecuteCommand = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteCommand;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
