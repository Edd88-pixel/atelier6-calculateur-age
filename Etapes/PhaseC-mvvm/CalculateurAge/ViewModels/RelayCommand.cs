using System.Windows.Input;

namespace CalculateurAge.ViewModels;

public sealed class RelayCommand(Action executer, Func<bool>? peutExecuter = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => peutExecuter?.Invoke() ?? true;

    public void Execute(object? parameter)
    {
        if (CanExecute(parameter)) executer();
    }

    public void Rafraichir() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
