using CalculateurAge.Services;
using CalculateurAge.ViewModels;
using CalculateurAge.Views;
using Microsoft.Extensions.Logging;

namespace CalculateurAge;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        builder.Services.AddSingleton(_ =>
            new ResultatViewModel(() => Shell.Current.GoToAsync("..")));
        builder.Services.AddSingleton<INavigationResultat, NavigationResultat>();
        builder.Services.AddSingleton(provider =>
            new CalculateurViewModel(provider.GetRequiredService<INavigationResultat>(), () => DateTime.Today));
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<ResultatPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
