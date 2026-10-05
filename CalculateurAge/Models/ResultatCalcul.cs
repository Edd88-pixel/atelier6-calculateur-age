namespace CalculateurAge.Models;

public sealed record ResultatCalcul(
    string Nom, DateTime DateNaissance, DateTime DateCalcul,
    int Age, DateTime ProchainAnniversaire, int JoursRestants)
{
    public string Texte => $"{Nom}, vous avez {Age} {(Age > 1 ? "ans" : "an")}";
    public string Message => Age >= 18 ? "Majeur" : "Mineur";
    public string Anniversaire => JoursRestants == 0
        ? "C'est votre anniversaire aujourd'hui !"
        : $"Prochain anniversaire dans {JoursRestants} {(JoursRestants > 1 ? "jours" : "jour")} ({ProchainAnniversaire:dd/MM/yyyy})";
    public string Detail => $"Né(e) le {DateNaissance:dd/MM/yyyy} · Calcul du {DateCalcul:dd/MM/yyyy}";
}
