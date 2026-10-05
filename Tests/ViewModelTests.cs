using CalculateurAge.Models;
using CalculateurAge.Services;
using CalculateurAge.ViewModels;

namespace CalculateurAge.Tests;

internal static class ViewModelTests
{
    public static async Task ExecuterAsync()
    {
        var navigation = new NavigationTest();
        DateTime aujourdHui = new(2026, 10, 5);
        var vm = new CalculateurViewModel(navigation, () => aujourdHui);
        var notifications = new List<string?>();
        vm.PropertyChanged += (_, e) => notifications.Add(e.PropertyName);
        int refresh = 0;
        vm.CalculerCommand.CanExecuteChanged += (_, _) => refresh++;
        Verifier.Egal(false, vm.CalculerCommand.CanExecute(null), "Calcul initial désactivé");
        Verifier.Egal(true, vm.HistoriqueVide, "Historique initial vide");
        vm.Nom = "Alice";
        Verifier.Egal(true, vm.CalculerCommand.CanExecute(null), "Activation à la première lettre");
        Verifier.Egal(true, notifications.Contains("Nom"), "Notification de binding");
        int compte = notifications.Count;
        vm.Nom = "Alice";
        Verifier.Egal(compte, notifications.Count, "Aucune notification inutile");
        Verifier.Egal(1, refresh, "Une notification de commande");
        vm.DateNaissance = new(2008, 10, 5);
        vm.CalculerCommand.Execute(null);
        Verifier.Egal("Alice, vous avez 18 ans", vm.Resultat, "Résultat MVVM");
        Verifier.Egal(true, vm.ResultatVisible, "Résultat visible");
        Verifier.Egal("Majeur", vm.Message, "Statut MVVM");
        Verifier.Egal(false, vm.HistoriqueVide, "Historique actualisé");
        await vm.VoirResultatCommand.ExecuterAsync();
        Verifier.Egal(vm.Calcul, navigation.Dernier, "Navigation avec objet exact sans URL");
        vm.Nom = "Bob";
        Verifier.Egal(false, vm.ResultatVisible, "Modification invalide le résultat précédent");
        Verifier.Egal(false, vm.VoirResultatCommand.CanExecute(null), "Détail invalide désactivé");
        vm.DateNaissance = aujourdHui.AddDays(1);
        Verifier.Egal(false, vm.CalculerCommand.CanExecute(null), "Date future désactivée");
        Verifier.Egal(true, vm.ErreurVisible, "Erreur future affichée");
        vm.CalculerCommand.Execute(null);
        Verifier.Egal(1, vm.Historique.Count, "Pas de calcul invalide dans l'historique");
        vm.DateNaissance = null;
        Verifier.Egal("Choisissez une date de naissance.", vm.Erreur, "Date absente signalée");
        vm.DateNaissance = aujourdHui;
        vm.Nom = "   ";
        Verifier.Egal(false, vm.CalculerCommand.CanExecute(null), "Espaces seuls refusés");
        vm.Nom = null;
        Verifier.Egal(false, vm.CalculerCommand.CanExecute(null), "Nom nul refusé");
        vm.EffacerCommand.Execute(null);
        Verifier.Egal(string.Empty, vm.Nom, "Effacement du nom");
        Verifier.Egal<DateTime?>(aujourdHui.AddYears(-20), vm.DateNaissance, "Date initiale restaurée");
        Verifier.Egal(false, vm.ResultatVisible, "Effacement du résultat");
        Verifier.Egal(false, vm.ErreurVisible, "Effacement de l'erreur");
        Verifier.Egal(1, vm.Historique.Count, "Historique préservé lors du reset");
        Verifier.Egal(false, vm.CalculerCommand.CanExecute(null), "Reset désactive le calcul");
        for (int i = 0; i < 12; i++)
        {
            vm.Nom = $"Nom {i}";
            vm.CalculerCommand.Execute(null);
        }
        Verifier.Egal(10, vm.Historique.Count, "Historique limité à dix");
        Verifier.Egal("Nom 11", vm.Historique[0].Nom, "Ordre du plus récent au plus ancien");
        Verifier.Egal("Nom 2", vm.Historique[9].Nom, "Éviction du plus ancien");
        navigation.Echec = true;
        await vm.VoirResultatCommand.ExecuterAsync();
        Verifier.Egal("Navigation impossible : route indisponible", vm.Erreur, "Échec de navigation visible");
        Verifier.Egal(true, vm.VoirResultatCommand.CanExecute(null), "Nouvel essai disponible après l'échec");

        var detail = new ResultatViewModel(() => Task.FromException(new InvalidOperationException("retour refusé")));
        detail.Afficher(vm.Calcul!);
        Verifier.Egal(vm.Calcul, detail.Calcul, "Binding du détail");
        await detail.RetourCommand.ExecuterAsync();
        Verifier.Egal("Retour impossible : retour refusé", detail.Erreur, "Erreur de retour visible");
        detail.Afficher(vm.Calcul!);
        Verifier.Egal(false, detail.ErreurVisible, "Nouvelle navigation efface l'ancienne erreur");
    }

    private sealed class NavigationTest : INavigationResultat
    {
        public ResultatCalcul? Dernier { get; private set; }
        public bool Echec { get; set; }
        public Task AfficherAsync(ResultatCalcul resultat)
        {
            if (Echec) throw new InvalidOperationException("route indisponible");
            Dernier = resultat;
            return Task.CompletedTask;
        }
    }
}
