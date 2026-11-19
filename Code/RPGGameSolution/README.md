# Das vergessene KÃ¶nigreich â€“ LS2 Textadventure (C#, .NET 10)

```
RPGGame/
â”œâ”€ Program.cs                Einstiegspunkt
â”œâ”€ Models/                   Modelklassen (keine Spiellogik)
â”‚    Character, Player, Monster, Item, Inventory, Quest, Location
â”œâ”€ Control/                  Spielsteuerung
â”‚    GameController, BattleController, GameRules
â”œâ”€ View/ConsoleView.cs       einzige Klasse mit Console-Zugriff
â””â”€ Data/GameData.cs          Items, Monster, Orte, Startspieler
RPGGame.Tests/               xUnit-Tests
docs/                        Klassendiagramm
```

**Starten:** `RPGGameSolution.slnx` in Visual Studio Ã¶ffnen, RPGGame als Startprojekt, F5.
**Tests:** *Test â†’ Alle Tests ausfÃ¼hren* oder `dotnet test`.

**Spielablauf:** Wald (Goblins, WÃ¶lfe) â†’ Stadt Eldenruh (Laden, Taverne) â†’ Dunkle HÃ¶hle (ein Skelett trÃ¤gt den
Alten SchlÃ¼ssel) â†’ Altes Schloss (nur mit SchlÃ¼ssel) â†’ Dunkler Ritter.
