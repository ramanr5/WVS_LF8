using RPGGame.Control;
using RPGGame.View;

namespace RPGGame;

// Nur der Einstiegspunkt – die Spielsteuerung liegt im GameController.
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            new GameController(new ConsoleView(), new Random()).Run();
        }
        catch (EndOfStreamException)
        {
            // Eingabe beendet – Spiel endet regulär.
        }
    }
}
