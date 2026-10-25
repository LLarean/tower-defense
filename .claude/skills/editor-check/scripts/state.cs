var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
var scene = UnityEngine.Object.FindFirstObjectByType<Game.Stage.BattleScene>();
var battle = (Game.Core.Battles.Battle)typeof(Game.Stage.BattleScene).GetField("_battle", flags).GetValue(scene);
var treasury = (Game.Core.Economy.Treasury)typeof(Game.Core.Towers.Garrison).GetField("_treasury", flags).GetValue(battle.Garrison);
return "time " + UnityEngine.Time.time + ", scale " + UnityEngine.Time.timeScale
    + ", wave " + battle.Invasion.Wave + "/" + battle.Invasion.Waves
    + ", monsters " + battle.Arena.Horde.Monsters.Count
    + ", lives " + battle.Lives.Left + ", gold " + treasury.Wallet(0).Gold
    + ", towers " + battle.Garrison.Towers.Count + ", outcome " + battle.Outcome;
