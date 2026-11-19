using RPGGame.Models;

namespace RPGGame.Data;

// Spielinhalte: jedes Item und Monster ist genau EINMAL definiert.
public static class GameData
{
    public const string KeyName = "Alter Schlüssel";

    public static Item Potion() => new("Heiltrank", "Heilt 30 HP.", ItemType.Heiltrank, 20, 30);
    public static Item Dagger() => new("Dolch", "Ein einfacher Dolch.", ItemType.Waffe, 30, 3);
    public static Item Sword() => new("Eisenschwert", "Eine robuste Waffe.", ItemType.Waffe, 80, 8);
    public static Item LeatherArmor() => new("Lederrüstung", "Leichter Schutz.", ItemType.Rüstung, 40, 2);
    public static Item ChainMail() => new("Kettenrüstung", "Starker Schutz.", ItemType.Rüstung, 70, 6);
    public static Item Key() => new(KeyName, "Öffnet das Tor des Schlosses.", ItemType.Schlüssel, 0);

    public static Monster CreateMonster(string name) => name switch
    {
        "Goblin" => new Monster("Goblin", 40, 9, 2, 40, 10, Potion()),
        "Wolf" => new Monster("Wolf", 55, 12, 3, 55, 12),
        "Bandit" => new Monster("Bandit", 70, 15, 5, 75, 25, Sword()),
        "Skelett" => new Monster("Skelett", 80, 17, 7, 100, 30, Key()),
        "Dunkler Ritter" => new Monster("Dunkler Ritter", 200, 24, 10, 300, 150),
        _ => throw new ArgumentException($"Unbekanntes Monster: {name}", nameof(name))
    };

    public static List<Location> CreateWorld() => new()
    {
        new Location("Verwunschener Wald", "Ein dichter Wald voller wilder Kreaturen.",
            monsterNames: new[] { "Goblin", "Wolf" }),

        new Location("Eldenruh", "Eine sichere Handelsstadt mit Laden und Taverne.",
            shopItems: new[] { Potion(), Sword(), ChainMail() }),

        new Location("Dunkle Höhle", "Banditen und Skelette hausen hier. Ein Skelett soll einen alten Schlüssel tragen.",
            monsterNames: new[] { "Bandit", "Skelett" }),

        new Location("Altes Schloss", "Die Ruine des vergessenen Königreichs. Hier wartet der Dunkle Ritter.",
            monsterNames: new[] { "Skelett", "Dunkler Ritter" }, requiredItemName: KeyName)
    };

    public static Player CreatePlayer(string name)
    {
        var player = new Player(string.IsNullOrWhiteSpace(name) ? "Held" : name);

        Item dagger = Dagger();
        Item armor = LeatherArmor();
        player.Inventory.Add(Potion());
        player.Inventory.Add(dagger);
        player.Inventory.Add(armor);
        player.Inventory.Equip(dagger);
        player.Inventory.Equip(armor);

        player.AddQuest(new Quest("Goblin-Jäger", "Besiege 3 Goblins.", "Goblin", 3, 100, 50));
        player.AddQuest(new Quest("Banditenplage", "Besiege 2 Banditen.", "Bandit", 2, 150, 80));
        player.AddQuest(new Quest("Das Ende der Dunkelheit", "Besiege den Dunklen Ritter.", "Dunkler Ritter", 1, 0, 200));
        return player;
    }

    public const string Intro =
        "Du bist ein Reisender im vergessenen Königreich. Man erzählt sich, ein Skelett in der " +
        "Dunklen Höhle trage den alten Schlüssel zum Schloss – dort wartet der Dunkle Ritter.";

    public const string Victory =
        "Der Dunkle Ritter ist gefallen! Die Sonne erhebt sich über dem Königreich. Du hast das Spiel gewonnen.";
}
