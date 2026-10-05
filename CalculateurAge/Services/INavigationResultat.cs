using CalculateurAge.Models;

namespace CalculateurAge.Services;

public interface INavigationResultat
{
    Task AfficherAsync(ResultatCalcul resultat);
}
