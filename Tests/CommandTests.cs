using CalculateurAge.ViewModels;

namespace CalculateurAge.Tests;

internal static class CommandTests
{
    public static async Task ExecuterAsync()
    {
        var fin = new TaskCompletionSource();
        int appels = 0;
        int notifications = 0;
        Exception? erreur = null;
        var command = new AsyncRelayCommand(async () => { appels++; await fin.Task; }, e => erreur = e);
        command.CanExecuteChanged += (_, _) => notifications++;
        Task execution = command.ExecuterAsync();
        Verifier.Egal(false, command.CanExecute(null), "Commande désactivée pendant la navigation");
        await command.ExecuterAsync();
        Verifier.Egal(1, appels, "Double clic ne lance pas deux navigations");
        fin.SetResult();
        await execution;
        Verifier.Egal(true, command.CanExecute(null), "Commande réactivée après navigation");
        Verifier.Egal(2, notifications, "Début et fin notifiés");
        Verifier.Egal<Exception?>(null, erreur, "Pas de faux échec");
    }
}
