using CalculateurAge.Views;

namespace CalculateurAge;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();
        Items.Add(new ShellContent
        {
            Title = "Calculateur d'âge",
            Route = nameof(MainPage),
            Content = mainPage
        });
        Routing.RegisterRoute(nameof(ResultatPage), typeof(ResultatPage));
    }
}
