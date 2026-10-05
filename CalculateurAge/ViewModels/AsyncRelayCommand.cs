using System.Windows.Input;

namespace CalculateurAge.ViewModels;

public sealed class AsyncRelayCommand(
    Func<Task> executer, Action<Exception> signalerErreur, Func<bool>? peutExecuter = null) : ICommand
{
    private bool _enCours;
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
        => !_enCours && (peutExecuter?.Invoke() ?? true);

    public async void Execute(object? parameter) => await ExecuterAsync();

    public async Task ExecuterAsync()
    {
        if (!CanExecute(null)) return;
        _enCours = true;
        Rafraichir();
        try
        {
            await executer();
        }
        catch (Exception erreur)
        {
            // Frontière ICommand : l'échec est affiché dans le ViewModel, jamais ignoré.
            signalerErreur(erreur);
        }
        finally
        {
            _enCours = false;
            Rafraichir();
        }
    }

    public void Rafraichir() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
