using RPGGame.Data;
using RPGGame.Models;
using RPGGame.View;

namespace RPGGame.Control;

// SPIELSTEUERUNG – Ablauf eines Kampfes. Die Regeln stehen in GameRules.
public sealed class BattleController
{
    private readonly ConsoleView _view;
    private readonly Random _random;

    public BattleController(ConsoleView view, Random random)
    {
        _view = view;
        _random = random;
    }

    public void Fight(Player player, Monster monster)
    {
        while (player.IsAlive && monster.IsAlive)
        {
            _view.ShowTitle("Kampf");
            _view.Show($"{player.Name}: {player.Health}/{player.MaxHealth} HP");
            _view.Show($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP");
            _view.Show();

            int? action = _view.Menu("Angreifen", "Heiltrank trinken", "Fliehen");

            if (action == 3 && TryFlee())
            {
                return;
            }

            if (action is 1)
            {
                _view.Show($"Du verursachst {GameRules.Attack(player, monster)} Schaden.");
            }
            else if (action is 2)
            {
                if (!DrinkPotion(player)) continue;
            }
            else if (action is null)
            {
                continue;
            }

            if (monster.IsAlive)
            {
                _view.Show($"{monster.Name} verursacht {GameRules.Attack(monster, player)} Schaden.");
                _view.Pause();
            }
        }

        if (player.IsAlive) ShowVictory(player, monster);
        else ShowDefeat(player);
    }

    private bool TryFlee()
    {
        bool fled = GameRules.TryFlee(_random);
        _view.Show(fled ? "Du bist entkommen." : "Die Flucht misslingt!");

        if (fled) _view.Pause();
        return fled;
    }

    private bool DrinkPotion(Player player)
    {
        List<Item> potions = player.Inventory.GetItemsOfType(ItemType.Heiltrank);

        if (potions.Count == 0)
        {
            _view.Show("Du hast keinen Heiltrank.");
            _view.Pause();
            return false;
        }

        _view.Show($"Du heilst {GameRules.UsePotion(player, potions[0])} HP.");
        return true;
    }

    private void ShowVictory(Player player, Monster monster)
    {
        VictoryReward reward = GameRules.GrantVictory(player, monster);

        _view.ShowTitle("Sieg!");
        _view.Show($"{monster.Name} besiegt: +{reward.Experience} XP, +{reward.Gold} Gold");

        if (reward.Drop is not null) _view.Show($"Beute: {reward.Drop.Name}");
        foreach (Quest quest in reward.CompletedQuests)
        {
            _view.Show($"Quest erfüllt: {quest.Name} (+{quest.RewardExperience} XP, +{quest.RewardGold} Gold)");
        }
        if (reward.LevelsGained > 0) _view.Show($"LEVEL UP! Du bist jetzt Level {player.Level}.");
        if (monster.Name == "Dunkler Ritter") _view.Show(GameData.Victory);

        _view.Pause();
    }

    private void ShowDefeat(Player player)
    {
        GameRules.Revive(player);
        _view.ShowTitle("Besiegt");
        _view.Show($"Du wachst später mit {player.Health} HP wieder auf.");
        _view.Pause();
    }
}
