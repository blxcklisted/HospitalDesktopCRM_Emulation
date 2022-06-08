using System;
using System.Windows.Input;

namespace Project.UI.Infrastructure
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Predicate<object> predicate;

        public RelayCommand(Action<object> action, Predicate<object> predicate = null)
        {
            this.action = action;
            this.predicate = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return predicate == null || predicate(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
