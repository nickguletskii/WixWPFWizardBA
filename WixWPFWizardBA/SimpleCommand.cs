namespace WixWPFWizardBA
{
    using System;
    using System.Windows.Input;

    public class SimpleCommand : ICommand
    {
        private readonly Func<object, bool> _canExecuteAction;
        private readonly Action<object> _executeAction;

        public SimpleCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            this._executeAction = executeAction;
            this._canExecuteAction = canExecuteAction;
        }

        public void Execute(object parameter)
        {
            this._executeAction(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}