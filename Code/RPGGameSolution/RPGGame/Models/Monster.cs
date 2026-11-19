namespace RPGGame.Models;

// Ein Gegner. Alle Werte (inkl. Beute) werden im Konstruktor gesetzt.
public sealed class Monster : Character
{
    public int ExperienceReward { get; }
    public int GoldReward { get; }
    public Item? Drop { get; }

    public Monster(string name, int maxHealth, int attack, int defense,
                   int experienceReward, int goldReward, Item? drop = null)
        : base(name, maxHealth, attack, defense)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(experienceReward);
        ArgumentOutOfRangeException.ThrowIfNegative(goldReward);

        ExperienceReward = experienceReward;
        GoldReward = goldReward;
        Drop = drop;
    }
}
