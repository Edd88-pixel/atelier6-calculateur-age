using CalculateurAge.Models;
using CalculateurAge.ViewModels;
using CalculateurAge.Views;

namespace CalculateurAge.Services;

public sealed class NavigationResultat(ResultatViewModel resultatViewModel) : INavigationResultat
{
    public Task AfficherAsync(ResultatCalcul resultat)
    {
        resultatViewModel.Afficher(resultat);
        return Shell.Current.GoToAsync(nameof(ResultatPage));
    }
}
