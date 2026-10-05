namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string? _nom = string.Empty;
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = string.Empty;
    private bool _resultatVisible;

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(Calculer, () => !string.IsNullOrWhiteSpace(Nom));
    }

    public string? Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value)) CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        private set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        private set => SetField(ref _resultatVisible, value);
    }

    public RelayCommand CalculerCommand { get; }

    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) age--;
        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
    }
}
