namespace RPGGame.Models;

// Ein Gegenstand. Unveränderlich: alle Werte nur über den Konstruktor.
public sealed class Item
{
    public string Name { get; }
    public string Description { get; }
    public ItemType Type { get; }
    public int Price { get; }

    // Waffe: Angriff | Rüstung: Verteidigung | Heiltrank: Heilung
    public int Power { get; }

    public Item(string name, string description, ItemType type, int price, int power = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfNegative(power);

        if (type != ItemType.Schlüssel && power == 0)
        {
            throw new ArgumentException("Waffen, Rüstungen und Tränke brauchen eine Stärke > 0.", nameof(power));
        }

        Name = name.Trim();
        Description = description.Trim();
        Type = type;
        Price = price;
        Power = power;
    }

    // Neues, gleiches Exemplar (z. B. beim Kauf im Laden).
    public Item Copy()
    {
        return new Item(Name, Description, Type, Price, Power);
    }
}