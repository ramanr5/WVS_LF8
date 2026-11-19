namespace RPGGame.Models;

// Basisklasse für Spieler und Monster.
// Enthält nur Zustand und schützt ihn (Leben nie < 0 oder > MaxHealth).
// Spielregeln (z. B. die Schadensformel) stehen in GameRules.
public abstract class Character
{
    public string Name { get; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public int BaseAttack { get; private set; }
    public int BaseDefense { get; private set; }

    public virtual int Attack => BaseAttack;
    public virtual int Defense => BaseDefense;
    public bool IsAlive => Health > 0;

    protected Character(string name, int maxHealth, int attack, int defense)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxHealth, 1);
        ArgumentOutOfRangeException.ThrowIfNegative(attack);
        ArgumentOutOfRangeException.ThrowIfNegative(defense);

        Name = name.Trim();
        MaxHealth = maxHealth;
        Health = maxHealth;
        BaseAttack = attack;
        BaseDefense = defense;
    }

    // Liefert den tatsächlich erlittenen Schaden.
    public int TakeDamage(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        int damage = Math.Min(amount, Health);
        Health -= damage;
        return damage;
    }

    // Liefert die tatsächliche Heilung.
    public int Heal(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        int healed = Math.Min(amount, MaxHealth - Health);
        Health += healed;
        return healed;
    }

    // Nur für abgeleitete Klassen (Level-Up des Spielers).
    protected void IncreaseStats(int maxHealth, int attack, int defense)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth);
        ArgumentOutOfRangeException.ThrowIfNegative(attack);
        ArgumentOutOfRangeException.ThrowIfNegative(defense);

        MaxHealth += maxHealth;
        BaseAttack += attack;
        BaseDefense += defense;
        Health = MaxHealth;
    }
}
