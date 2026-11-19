using RPGGame.Models;

namespace RPGGame.View;

// Die einzige Klasse mit Console-Zugriff (Ein- und Ausgabe).
public sealed class ConsoleView
{
    public void ShowTitle(string title)
    {
        try { Console.Clear(); } catch (IOException) { /* Ausgabe umgeleitet */ }

        Console.WriteLine("==========================================");
        Console.WriteLine($"  {title.ToUpper()}");
        Console.WriteLine("==========================================");
    }

    public void Show(string text = "") => Console.WriteLine(text);

    public void ShowStatus(Player player)
    {
        Show($"{player.Name} | Level {player.Level} | HP {player.Health}/{player.MaxHealth} | " +
             $"Angriff {player.Attack} | Verteidigung {player.Defense} | Gold {player.Gold}");
        Show();
    }

    // Zeigt nummerierte Optionen. Liefert die Wahl (1..n) oder null bei ungültiger Eingabe.
    public int? Menu(params string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Show($"{i + 1}. {options[i]}");
        }

        Console.Write("Auswahl: ");
        return ParseChoice(ReadLine(), options.Length);
    }

    // Lässt ein Element aus einer Liste wählen. "Zurück" oder ungültig -> null.
    public T? Choose<T>(IReadOnlyList<T> items, Func<T, string> describe) where T : class
    {
        int? choice = Menu(items.Select(describe).Append("Zurück").ToArray());
        return choice <= items.Count ? items[choice.Value - 1] : null;
    }

    public string ReadText(string prompt)
    {
        Console.Write(prompt);
        return ReadLine();
    }

    public void Pause()
    {
        Show();
        Show("Weiter mit Enter...");
        ReadLine();
    }

    public static int? ParseChoice(string? input, int optionCount)
    {
        bool isNumber = int.TryParse(input?.Trim(), out int choice);
        return isNumber && choice >= 1 && choice <= optionCount ? choice : null;
    }

    // Ende der Eingabe (z. B. umgeleitete Datei) beendet das Spiel sauber
    private static string ReadLine() => Console.ReadLine() ?? throw new EndOfStreamException();
}
