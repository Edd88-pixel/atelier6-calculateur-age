namespace CalculateurAge.Tests;

internal static class Verifier
{
    public static int Nombre { get; private set; }

    public static void Egal<T>(T attendu, T obtenu, string scenario)
    {
        if (!EqualityComparer<T>.Default.Equals(attendu, obtenu))
            throw new InvalidOperationException($"{scenario} : attendu {attendu}, obtenu {obtenu}");
        Nombre++;
    }

    public static void Exception<T>(Action action, string scenario) where T : Exception
    {
        try { action(); }
        catch (T) { Nombre++; return; }
        throw new InvalidOperationException($"{scenario} : exception {typeof(T).Name} attendue");
    }
}
