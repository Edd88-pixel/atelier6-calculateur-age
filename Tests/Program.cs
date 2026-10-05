using CalculateurAge.Tests;

CalculAgeTests.Executer();
await ViewModelTests.ExecuterAsync();
await CommandTests.ExecuterAsync();
Console.WriteLine($"{Verifier.Nombre} vérifications réussies.");
