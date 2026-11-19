namespace RPGGame.Models;

// Der Spieler. Kein öffentlicher Setter – Werte ändern sich nur über Methoden.
public sealed class Player : Character
{
    private readonly List<Quest> _quests = new();

    public int Level { get; private set; } = 1;
    public int Experience { get; private set; }
    public int Gold { get; private set; }

    public Inventory Inventory { get; } = new();
    public IReadOnlyList<Quest> Quests => _quests.AsReadOnly();

    // Angriff/Verteidigung = Grundwert + Ausrüstung (wird immer neu berechnet)
    public override int Attack => BaseAttack + (Inventory.Weapon?.Power ?? 0);
    public override int Defense => BaseDefense + (Inventory.Armor?.Power ?? 0);

    public Player(string name, int gold = 50)
        : base(name, maxHealth: 100, attack: 10, defense: 5)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(gold);
        Gold = gold;
    }

    public void AddGold(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Gold += amount;
    }

    public bool TrySpendGold(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        if (amount > Gold)
        {
            return false;
        }

        Gold -= amount;
        return true;
    }

    public void AddExperience(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Experience += amount;
    }

    // Wie viel XP nötig sind und welche Boni es gibt, entscheidet GameRules.
    public void LevelUp(int usedExperience, int healthBonus, int attackBonus, int defenseBonus)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(usedExperience);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(usedExperience, Experience);

        Level++;
        Experience -= usedExperience;
        IncreaseStats(healthBonus, attackBonus, defenseBonus);
    }

    public bool AddQuest(Quest quest)
    {
        ArgumentNullException.ThrowIfNull(quest);

        if (_quests.Exists(q => q.Name == quest.Name))
        {
            return false;
        }

        _quests.Add(quest);
        return true;
    }
}
