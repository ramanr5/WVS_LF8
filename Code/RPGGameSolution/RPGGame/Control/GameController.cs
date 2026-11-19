using RPGGame.Data;
using RPGGame.Models;
using RPGGame.View;

namespace RPGGame.Control;

// SPIELSTEUERUNG – Hauptablauf: Menüs, Orte, Stadt, Inventar, Quests.
// Kämpfe übernimmt der BattleController, Regeln stehen in GameRules.
public sealed class GameController
{
    private readonly ConsoleView _view;
    private readonly Random _random;
    private readonly BattleController _battle;
    private readonly List<Location> _world = GameData.CreateWorld();

    public GameController(ConsoleView view, Random random)
    {
        _view = view;
        _random = random;
        _battle = new BattleController(view, random);
    }

    public void Run()
    {
        _view.ShowTitle("Das vergessene Königreich");
        Player player = GameData.CreatePlayer(_view.ReadText("Name deines Helden: "));
        _view.Show();
        _view.Show(GameData.Intro);
        _view.Pause();

        while (true)
        {
            _view.ShowTitle("Hauptmenü");
            _view.ShowStatus(player);

            switch (_view.Menu("Orte erkunden", "Inventar", "Quests", "Beenden"))
            {
                case 1: ChooseLocation(player); break;
                case 2: ShowInventory(player); break;
                case 3: ShowQuests(player); break;
                case 4: return;
            }
        }
    }

    private void ChooseLocation(Player player)
    {
        _view.ShowTitle("Wohin gehst du?");
        Location? location = _view.Choose(_world, place => place.Name);

        if (location is null)
        {
            return;
        }

        if (!GameRules.CanEnter(player, location))
        {
            _view.Show($"Das Tor ist verschlossen. Du brauchst: {location.RequiredItemName}.");
            _view.Pause();
        }
        else if (location.IsTown)
        {
            VisitTown(player, location);
        }
        else
        {
            Explore(player, location);
        }
    }

    private void Explore(Player player, Location location)
    {
        while (true)
        {
            _view.ShowTitle(location.Name);
            _view.Show(location.Description);
            _view.ShowStatus(player);

            if (_view.Menu("Nach Gegnern suchen", "Zurück") != 1)
            {
                return;
            }

            string name = location.MonsterNames[_random.Next(location.MonsterNames.Count)];
            _view.Show($"Ein {name} greift an!");
            _view.Pause();
            _battle.Fight(player, GameData.CreateMonster(name));
        }
    }

    private void VisitTown(Player player, Location town)
    {
        while (true)
        {
            _view.ShowTitle(town.Name);
            _view.Show(town.Description);
            _view.ShowStatus(player);

            switch (_view.Menu("Laden", "Taverne (kostenlos ausruhen)", "Zurück"))
            {
                case 1: Shop(player, town); break;
                case 2:
                    _view.Show($"Du schläfst gut und heilst {GameRules.Rest(player)} HP.");
                    _view.Pause();
                    break;
                case 3: return;
            }
        }
    }

    private void Shop(Player player, Location town)
    {
        _view.ShowTitle($"Laden – dein Gold: {player.Gold}");
        Item? offer = _view.Choose(town.ShopItems, item => $"{item.Name} ({item.Description}) – {item.Price} Gold");

        if (offer is not null)
        {
            _view.Show(GameRules.TryBuy(player, offer) ? $"Gekauft: {offer.Name}" : "Nicht genug Gold.");
            _view.Pause();
        }
    }

    private void ShowInventory(Player player)
    {
        _view.ShowTitle("Inventar – Item zum Ausrüsten wählen");
        Inventory inventory = player.Inventory;

        Item? item = _view.Choose(inventory.Items, entry =>
            $"{entry.Name} ({entry.Type}, Stärke {entry.Power})" +
            (entry == inventory.Weapon || entry == inventory.Armor ? " [ausgerüstet]" : ""));

        if (item is not null)
        {
            _view.Show(inventory.Equip(item) ? $"{item.Name} ausgerüstet." : "Das kann man nicht ausrüsten.");
            _view.Pause();
        }
    }

    private void ShowQuests(Player player)
    {
        _view.ShowTitle("Quests");

        foreach (Quest quest in player.Quests)
        {
            string status = quest.IsCompleted ? "ERFÜLLT" : $"{quest.CurrentKills}/{quest.RequiredKills}";
            _view.Show($"{quest.Name}: {quest.Description} [{status}] – {quest.RewardExperience} XP, {quest.RewardGold} Gold");
        }

        _view.Pause();
    }
}
