using RPGGame.Control;
using RPGGame.Data;
using RPGGame.Models;
using Xunit;

namespace RPGGame.Tests;

// Testfälle für die Spielregeln (Aufbau: Arrange – Act – Assert)
public class GameRulesTests
{
    // TF1 – Äquivalenzklasse: Angriff größer als Verteidigung
    [Fact]
    public void Attack_NormalHit_DealsAttackMinusDefense()
    {
        var player = new Player("Held");                       // Angriff 10
        var goblin = new Monster("Goblin", 50, 9, 3, 40, 10);

        int damage = GameRules.Attack(player, goblin);

        Assert.Equal(7, damage);
        Assert.Equal(43, goblin.Health);
    }

    // TF2 – Grenzwerte: Verteidigung knapp unter / gleich / über dem Angriff
    [Theory]
    [InlineData(10, 9, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(10, 11, 1)]
    public void CalculateDamage_IsAtLeastOne(int attack, int defense, int expected)
    {
        Assert.Equal(expected, GameRules.CalculateDamage(attack, defense));
    }

    // TF3 – Grenzwerte: Level-Up bei genau 100 XP, nicht bei 99 XP
    [Theory]
    [InlineData(99, 1)]
    [InlineData(100, 2)]
    public void GainExperience_LevelUpBoundary(int experience, int expectedLevel)
    {
        var player = new Player("Held");

        GameRules.GainExperience(player, experience);

        Assert.Equal(expectedLevel, player.Level);
    }

    // TF4 – Grenzwerte: Gold reicht genau / 1 Gold zu wenig
    [Theory]
    [InlineData(80, true, 0)]
    [InlineData(79, false, 79)]
    public void TryBuy_GoldBoundary(int gold, bool expectedSuccess, int expectedGold)
    {
        var player = new Player("Held", gold);

        bool success = GameRules.TryBuy(player, GameData.Sword());   // Preis 80

        Assert.Equal(expectedSuccess, success);
        Assert.Equal(expectedGold, player.Gold);
    }

    // TF5 – Äquivalenzklassen: Schloss ohne / mit Schlüssel
    [Fact]
    public void CanEnter_Castle_OnlyWithKey()
    {
        var player = new Player("Held");
        Location castle = GameData.CreateWorld().Single(l => l.RequiredItemName is not null);

        bool before = GameRules.CanEnter(player, castle);
        player.Inventory.Add(GameData.Key());
        bool after = GameRules.CanEnter(player, castle);

        Assert.False(before);
        Assert.True(after);
    }

    // TF6 – Äquivalenzklasse: Sieg zahlt Belohnung, Beute und Quest-Fortschritt
    [Fact]
    public void GrantVictory_PaysRewardsAndCountsQuest()
    {
        var player = new Player("Held", gold: 0);
        player.AddQuest(new Quest("Jagd", "Besiege 1 Skelett.", "Skelett", 1, 10, 50));
        Monster skeleton = GameData.CreateMonster("Skelett");
        skeleton.TakeDamage(skeleton.MaxHealth);

        VictoryReward reward = GameRules.GrantVictory(player, skeleton);

        Assert.Equal(30 + 50, player.Gold);                     // Monster-Gold + Quest-Gold
        Assert.Single(reward.CompletedQuests);
        Assert.True(player.Inventory.ContainsItemNamed(GameData.KeyName));
    }

    // TF7 – Ungültige Aktion: Belohnung für ein lebendes Monster
    [Fact]
    public void GrantVictory_MonsterAlive_Throws()
    {
        Assert.Throws<InvalidOperationException>(
            () => GameRules.GrantVictory(new Player("Held"), GameData.CreateMonster("Goblin")));
    }
}
