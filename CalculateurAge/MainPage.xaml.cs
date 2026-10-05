using System.Globalization;
using CalculateurAge.Views;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        pickerDate.Date = DateTime.Today.AddYears(-20);
    }

    private async void OnCalculerClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entryNom.Text))
        {
            await DisplayAlertAsync("Erreur", "Entrez un nom", "OK");
            return;
        }

        if (pickerDate.Date is not DateTime naissance)
        {
            await DisplayAlertAsync("Erreur", "Choisissez une date", "OK");
            return;
        }

        int age = DateTime.Today.Year - naissance.Year;
        if (naissance.Date > DateTime.Today.AddYears(-age)) age--;

        string nom = Uri.EscapeDataString(entryNom.Text);
        await Shell.Current.GoToAsync(
            $"{nameof(ResultatPage)}?nom={nom}&age={age.ToString(CultureInfo.InvariantCulture)}");
    }
}
