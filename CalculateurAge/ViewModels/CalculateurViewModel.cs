using System.Collections.ObjectModel;
using CalculateurAge.Models;
using CalculateurAge.Services;

namespace CalculateurAge.ViewModels;

public sealed class CalculateurViewModel : BaseViewModel
{
    private readonly INavigationResultat _navigation;
    private readonly Func<DateTime> _aujourdhui;
    private string? _nom = string.Empty;
    private DateTime? _dateNaissance;
    private ResultatCalcul? _calcul;
    private string _erreur = string.Empty;

    public CalculateurViewModel(INavigationResultat navigation, Func<DateTime> aujourdhui)
    {
        _navigation = navigation;
        _aujourdhui = aujourdhui;
        _dateNaissance = aujourdhui().Date.AddYears(-20);
        CalculerCommand = new RelayCommand(Calculer, PeutCalculer);
        EffacerCommand = new RelayCommand(Effacer);
        VoirResultatCommand = new AsyncRelayCommand(VoirResultatAsync,
            erreur => Erreur = $"Navigation impossible : {erreur.Message}", () => _calcul is not null);
    }

    public string? Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value)) MettreAJourSaisie();
        }
    }

    public DateTime? DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value)) MettreAJourSaisie();
        }
    }

    public ResultatCalcul? Calcul
    {
        get => _calcul;
        private set
        {
            if (!SetField(ref _calcul, value)) return;
            OnPropertyChanged(nameof(Resultat));
            OnPropertyChanged(nameof(ResultatVisible));
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(Anniversaire));
            VoirResultatCommand.Rafraichir();
        }
    }

    public string Resultat => Calcul?.Texte ?? string.Empty;
    public bool ResultatVisible => Calcul is not null;
    public string Message => Calcul?.Message ?? string.Empty;
    public string Anniversaire => Calcul?.Anniversaire ?? string.Empty;

    public string Erreur
    {
        get => _erreur;
        private set
        {
            if (SetField(ref _erreur, value)) OnPropertyChanged(nameof(ErreurVisible));
        }
    }

    public bool ErreurVisible => Erreur.Length > 0;
    public bool HistoriqueVide => Historique.Count == 0;
    public ObservableCollection<ResultatCalcul> Historique { get; } = [];
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public AsyncRelayCommand VoirResultatCommand { get; }

    private bool PeutCalculer()
        => !string.IsNullOrWhiteSpace(Nom)
           && DateNaissance is DateTime naissance && naissance.Date <= _aujourdhui().Date;

    private void MettreAJourSaisie()
    {
        Calcul = null;
        Erreur = DateNaissance is null ? "Choisissez une date de naissance."
            : DateNaissance.Value.Date > _aujourdhui().Date
                ? "La date de naissance ne peut pas être future." : string.Empty;
        CalculerCommand.Rafraichir();
    }

    private void Calculer()
    {
        if (!PeutCalculer()) return;
        Calcul = CalculAge.Calculer(Nom!, DateNaissance!.Value, _aujourdhui());
        Erreur = string.Empty;
        Historique.Insert(0, Calcul);
        if (Historique.Count > 10) Historique.RemoveAt(10);
        OnPropertyChanged(nameof(HistoriqueVide));
    }

    private void Effacer()
    {
        Nom = string.Empty;
        DateNaissance = _aujourdhui().Date.AddYears(-20);
        Calcul = null;
        Erreur = string.Empty;
        CalculerCommand.Rafraichir();
    }

    private Task VoirResultatAsync() => _navigation.AfficherAsync(Calcul!);
}
