using CalculateurAge.Models;

namespace CalculateurAge.Tests;

internal static class CalculAgeTests
{
    public static void Executer()
    {
        var aujourdHui = new DateTime(2026, 10, 5);
        var jourJ = CalculAge.Calculer(" Alice ", new(2008, 10, 5), aujourdHui);
        Verifier.Egal(18, jourJ.Age, "Anniversaire aujourd'hui");
        Verifier.Egal("Alice", jourJ.Nom, "Nom normalisé");
        Verifier.Egal("Majeur", jourJ.Message, "Majorité à 18 ans");
        Verifier.Egal(0, jourJ.JoursRestants, "Anniversaire le jour même");
        Verifier.Egal(aujourdHui, jourJ.ProchainAnniversaire, "Date du prochain anniversaire");

        var demain = CalculAge.Calculer("Bob", new(2008, 10, 6), aujourdHui);
        Verifier.Egal(17, demain.Age, "Anniversaire pas encore passé");
        Verifier.Egal("Mineur", demain.Message, "Mineur avant 18 ans");
        Verifier.Egal(1, demain.JoursRestants, "Anniversaire demain");
        var hier = CalculAge.Calculer("Claire", new(2000, 10, 4), aujourdHui);
        Verifier.Egal(26, hier.Age, "Anniversaire passé");
        Verifier.Egal(new DateTime(2027, 10, 4), hier.ProchainAnniversaire, "Prochaine année");
        Verifier.Egal(364, hier.JoursRestants, "Compte à rebours annuel");

        var bebe = CalculAge.Calculer("Nouveau-né", aujourdHui, aujourdHui.AddHours(18));
        Verifier.Egal(0, bebe.Age, "Naissance aujourd'hui");
        Verifier.Egal("Nouveau-né, vous avez 0 an", bebe.Texte, "Singulier zéro");
        Verifier.Egal("A & B ? # +, vous avez 1 an",
            CalculAge.Calculer("A & B ? # +", new(2025, 10, 5), aujourdHui).Texte, "Nom avec caractères URL");

        Verifier.Exception<ArgumentException>(() => CalculAge.Calculer(" ", aujourdHui, aujourdHui), "Nom vide");
        Verifier.Exception<ArgumentOutOfRangeException>(
            () => CalculAge.Calculer("Alice", aujourdHui.AddDays(1), aujourdHui), "Date future");
        Verifier.Egal(25, CalculAge.Calculer("Alice", new(2000, 2, 29), new(2026, 2, 27)).Age, "Avant le 28 février");
        Verifier.Egal(26, CalculAge.Calculer("Alice", new(2000, 2, 29), new(2026, 2, 28)).Age, "29 février en année non bissextile");
        Verifier.Egal(0, CalculAge.Calculer("Alice", new(2000, 2, 29), new(2026, 2, 28)).JoursRestants, "Convention 28 février");
        Verifier.Egal(new DateTime(2028, 2, 29),
            CalculAge.Calculer("Alice", new(2000, 2, 29), new(2027, 3, 1)).ProchainAnniversaire, "Retour au 29 février");
        Verifier.Egal(23, CalculAge.Calculer("Alice", new(2000, 2, 29), new(2024, 2, 28)).Age, "Avant un anniversaire bissextile");
        Verifier.Egal(24, CalculAge.Calculer("Alice", new(2000, 2, 29), new(2024, 2, 29)).Age, "Anniversaire bissextile");
        Verifier.Egal(1, CalculAge.Calculer("Alice", new(2000, 1, 1), new(2026, 12, 31)).JoursRestants, "Passage du 31 décembre");
    }
}
