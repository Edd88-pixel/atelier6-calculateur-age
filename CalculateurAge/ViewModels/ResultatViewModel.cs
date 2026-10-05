using CalculateurAge.Models;

namespace CalculateurAge.ViewModels;

public sealed class ResultatViewModel : BaseViewModel
{
    public ResultatViewModel(Func<Task> retour)
    {
        RetourCommand = new AsyncRelayCommand(retour,
            erreur => Erreur = $"Retour impossible : {erreur.Message}");
    }

    private ResultatCalcul? _calcul;
    private string _erreur = string.Empty;

    public ResultatCalcul? Calcul
    {
        get => _calcul;
        private set => SetField(ref _calcul, value);
    }

    public string Erreur
    {
        get => _erreur;
        private set
        {
            if (SetField(ref _erreur, value)) OnPropertyChanged(nameof(ErreurVisible));
        }
    }

    public bool ErreurVisible => Erreur.Length > 0;
    public AsyncRelayCommand RetourCommand { get; }

    public void Afficher(ResultatCalcul resultat)
    {
        Calcul = resultat;
        Erreur = string.Empty;
    }
}
