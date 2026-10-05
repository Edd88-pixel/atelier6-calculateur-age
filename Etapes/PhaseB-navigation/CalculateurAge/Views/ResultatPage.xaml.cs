namespace CalculateurAge.Views;

[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
public partial class ResultatPage : ContentPage
{
    public string Nom { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;

    public ResultatPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
    }

    private async void OnRetourClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
