namespace CalculateurAge.Models;

public static class CalculAge
{
    public static ResultatCalcul Calculer(string nom, DateTime naissance, DateTime aujourdHui)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nom);
        naissance = naissance.Date;
        aujourdHui = aujourdHui.Date;
        if (naissance > aujourdHui)
            throw new ArgumentOutOfRangeException(nameof(naissance), "La date de naissance ne peut pas être future.");

        DateTime anniversaire = AnniversaireEn(naissance, aujourdHui.Year);
        int age = aujourdHui.Year - naissance.Year;
        if (anniversaire > aujourdHui) age--;
        DateTime prochain = anniversaire >= aujourdHui
            ? anniversaire : AnniversaireEn(naissance, aujourdHui.Year + 1);

        return new(nom.Trim(), naissance, aujourdHui, age, prochain,
            (prochain - aujourdHui).Days);
    }

    // Convention de l'exercice : le 29 février est fêté le 28 en année non bissextile.
    private static DateTime AnniversaireEn(DateTime naissance, int annee)
        => new(annee, naissance.Month,
            Math.Min(naissance.Day, DateTime.DaysInMonth(annee, naissance.Month)));
}
