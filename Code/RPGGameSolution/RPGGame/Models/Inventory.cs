namespace RPGGame.Models;

// Inventar inkl. Ausrüstung. Ein ausgerüstetes Item liegt immer auch im Inventar.
public sealed class Inventory
{
    private readonly List<Item> _items = new();

    public IReadOnlyList<Item> Items => _items.AsReadOnly();
    public Item? Weapon { get; private set; }
    public Item? Armor { get; private set; }

    public void Add(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public bool Remove(Item item)
    {
        if (!_items.Remove(item))
        {
            return false;
        }

        if (item == Weapon) Weapon = null;
        if (item == Armor) Armor = null;
        return true;
    }

    public bool Equip(Item item)
    {
        if (!_items.Contains(item))
        {
            return false;
        }

        if (item.Type == ItemType.Waffe) Weapon = item;
        else if (item.Type == ItemType.Rüstung) Armor = item;
        else return false;

        return true;
    }

    public bool Contains(Item item) => _items.Contains(item);

    public bool ContainsItemNamed(string name) => _items.Exists(item => item.Name == name);

    public List<Item> GetItemsOfType(ItemType type) => _items.FindAll(item => item.Type == type);
}
