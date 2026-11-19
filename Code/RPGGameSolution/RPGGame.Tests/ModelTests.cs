using RPGGame.Models;
using RPGGame.View;
using Xunit;

namespace RPGGame.Tests;

// Testfälle für Konstruktoren und Datenkapselung
public class ModelTests
{
    // TF8 – Ungültige Eingaben: Quest ohne Pflicht-Siege oder ohne Namen
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quest_RequiredKillsBelowOne_Throws(int requiredKills)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Quest("Jagd", "Beschreibung", "Goblin", requiredKills, 10, 10));
    }

    [Fact]
    public void Quest_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Quest(" ", "Beschreibung", "Goblin", 1, 10, 10));
    }

    // TF9 – Ungültige Eingaben: Item mit negativem Preis / Waffe ohne Stärke
    [Fact]
    public void Item_InvalidValues_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Item("Stein", "Ein Stein.", ItemType.Schlüssel, -1));
        Assert.Throws<ArgumentException>(() => new Item("Stock", "Ein Stock.", ItemType.Waffe, 5, power: 0));
    }

    // TF10 – Grenzwert: Quest zählt nach Erfüllung nicht weiter
    [Fact]
    public void Quest_RegisterKill_StopsAtRequiredKills()
    {
        var quest = new Quest("Jagd", "Beschreibung", "Goblin", 2, 10, 10);

        bool first = quest.RegisterKill("Goblin");
        bool second = quest.RegisterKill("Goblin");
        bool third = quest.RegisterKill("Goblin");

        Assert.False(first);
        Assert.True(second);                                     // genau hier erfüllt
        Assert.False(third);
        Assert.Equal(2, quest.CurrentKills);
    }

    // TF11 – Grenzwert: Leben fällt nie unter 0
    [Fact]
    public void TakeDamage_MoreThanHealth_StopsAtZero()
    {
        var player = new Player("Held");

        player.TakeDamage(500);

        Assert.Equal(0, player.Health);
        Assert.False(player.IsAlive);
    }

    // TF12 – Menüeingabe: gültig, Grenzwerte, ungültig
    [Theory]
    [InlineData("2", 2)]                                         // Äquivalenzklasse
    [InlineData("1", 1)]                                         // untere Grenze
    [InlineData("4", 4)]                                         // obere Grenze
    [InlineData("0", null)]                                      // knapp darunter
    [InlineData("5", null)]                                      // knapp darüber
    [InlineData("abc", null)]                                    // ungültig
    [InlineData("", null)]                                       // ungültig
    public void ParseChoice_Works(string input, int? expected)
    {
        Assert.Equal(expected, ConsoleView.ParseChoice(input, optionCount: 4));
    }
}
