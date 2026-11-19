namespace RPGGame.Models;

// Eine Quest zählt nur ihren Fortschritt. Die Belohnung zahlt GameRules aus.
public sealed class Quest
{
    public string Name { get; }
    public string Description { get; }
    public string TargetMonster { get; }
    public int RequiredKills { get; }
    public int RewardExperience { get; }
    public int RewardGold { get; }
    public int CurrentKills { get; private set; }

    public bool IsCompleted => CurrentKills >= RequiredKills;

    public Quest(string name, string description, string targetMonster,
                 int requiredKills, int rewardExperience, int rewardGold)
    {
        // Pflichtwerte prüfen – ungültige Quests können nicht entstehen
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetMonster);
        ArgumentOutOfRangeException.ThrowIfLessThan(requiredKills, 1);
        ArgumentOutOfRangeException.ThrowIfNegative(rewardExperience);
        ArgumentOutOfRangeException.ThrowIfNegative(rewardGold);

        // Pflichtwerte setzen
        Name = name.Trim();
        Description = description.Trim();
        TargetMonster = targetMonster.Trim();
        RequiredKills = requiredKills;
        RewardExperience = rewardExperience;
        RewardGold = rewardGold;
    }

    // Zählt einen Sieg. Liefert true, wenn die Quest DADURCH abgeschlossen wurde.
    public bool RegisterKill(string monsterName)
    {
        if (IsCompleted || monsterName != TargetMonster)
        {
            return false;
        }

        CurrentKills++;
        return IsCompleted;
    }
}
