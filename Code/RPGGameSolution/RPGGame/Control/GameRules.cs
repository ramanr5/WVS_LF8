using RPGGame.Models;

namespace RPGGame.Control;

// Ergebnis eines Sieges (für die Anzeige)
public record VictoryReward(int Gold, int Experience, Item? Drop, int LevelsGained, List<Quest> CompletedQuests);

// SPIELSTEUERUNG – Spielregeln.
// Alle Regeln an einer Stelle, ohne Console-Zugriff -> automatisch testbar.
public static class GameRules
{
    public const int FleeChance = 50;   // Prozent
    public const int LevelUpHealth = 20;
    public const int LevelUpAttack = 3;
    public const int LevelUpDefense = 2;

    public static int CalculateDamage(int attack, int defense) => Math.Max(1, attack - defense);

    public static int RequiredExperience(int level) => 100 * level;

    // Angriff ausführen, liefert den Schaden
    public static int Attack(Character attacker, Character defender)
    {
        return defender.TakeDamage(CalculateDamage(attacker.Attack, defender.Defense));
    }

    public static bool TryFlee(Random random) => random.Next(1, 101) <= FleeChance;

    public static int UsePotion(Player player, Item potion)
    {
        if (potion.Type != ItemType.Heiltrank || !player.Inventory.Remove(potion))
        {
            throw new ArgumentException("Nur ein Heiltrank aus dem Inventar kann benutzt werden.");
        }

        return player.Heal(potion.Power);
    }

    // Liefert die Anzahl gewonnener Level
    public static int GainExperience(Player player, int amount)
    {
        player.AddExperience(amount);
        int levels = 0;

        while (player.Experience >= RequiredExperience(player.Level))
        {
            player.LevelUp(RequiredExperience(player.Level), LevelUpHealth, LevelUpAttack, LevelUpDefense);
            levels++;
        }

        return levels;
    }

    public static VictoryReward GrantVictory(Player player, Monster monster)
    {
        if (monster.IsAlive)
        {
            throw new InvalidOperationException("Belohnung gibt es erst nach dem Sieg.");
        }

        player.AddGold(monster.GoldReward);

        if (monster.Drop is not null)
        {
            player.Inventory.Add(monster.Drop);
        }

        List<Quest> completed = player.Quests.Where(q => q.RegisterKill(monster.Name)).ToList();
        int experience = monster.ExperienceReward + completed.Sum(q => q.RewardExperience);
        player.AddGold(completed.Sum(q => q.RewardGold));

        int levels = GainExperience(player, experience);
        return new VictoryReward(monster.GoldReward, monster.ExperienceReward, monster.Drop, levels, completed);
    }

    // Nach einer Niederlage wacht der Spieler mit halben Lebenspunkten auf
    public static void Revive(Player player) => player.Heal(player.MaxHealth / 2);

    public static bool CanEnter(Player player, Location location)
    {
        return location.RequiredItemName is null
            || player.Inventory.ContainsItemNamed(location.RequiredItemName);
    }

    public static bool TryBuy(Player player, Item offer)
    {
        if (!player.TrySpendGold(offer.Price))
        {
            return false;
        }

        player.Inventory.Add(offer.Copy());
        return true;
    }

    // Taverne: kostenlos vollständig heilen
    public static int Rest(Player player) => player.Heal(player.MaxHealth);
}
