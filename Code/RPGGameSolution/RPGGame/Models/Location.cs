namespace RPGGame.Models;

// Ein Ort. Hat er Ladenware, ist er eine Stadt (Laden + Taverne).
// Mit RequiredItemName ist er nur mit diesem Item betretbar.
public sealed class Location
{
    private readonly List<string> _monsterNames;
    private readonly List<Item> _shopItems;

    public string Name { get; }
    public string Description { get; }
    public string? RequiredItemName { get; }

    public IReadOnlyList<string> MonsterNames => _monsterNames.AsReadOnly();
    public IReadOnlyList<Item> ShopItems => _shopItems.AsReadOnly();
    public bool IsTown => _shopItems.Count > 0;

    public Location(string name, string description,
                    IEnumerable<string>? monsterNames = null,
                    IEnumerable<Item>? shopItems = null,
                    string? requiredItemName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        _monsterNames = monsterNames?.ToList() ?? new List<string>();
        _shopItems = shopItems?.ToList() ?? new List<Item>();

        if (_monsterNames.Count == 0 && _shopItems.Count == 0)
        {
            throw new ArgumentException("Ein Ort braucht Monster oder einen Laden.");
        }

        Name = name.Trim();
        Description = description.Trim();
        RequiredItemName = requiredItemName;
    }
}
