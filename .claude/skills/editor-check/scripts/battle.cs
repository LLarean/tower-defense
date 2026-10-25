var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
var scene = UnityEngine.Object.FindFirstObjectByType<Game.Stage.BattleScene>();
var type = typeof(Game.Stage.BattleScene);
var battle = (Game.Core.Battles.Battle)type.GetField("_battle", flags).GetValue(scene);
var routes = (Game.Stage.Map.RoutePath[])type.GetField("_routes", flags).GetValue(scene);
var bastion = type.GetField("_bastion", flags).GetValue(scene);
var armory = (Game.Stage.Towers.Armory)bastion.GetType().GetField("_armory", flags).GetValue(bastion);
var treasury = (Game.Core.Economy.Treasury)typeof(Game.Core.Towers.Garrison).GetField("_treasury", flags).GetValue(battle.Garrison);
treasury.Wallet(0).Earn(5000);
var route = routes[0].Route();
var built = 0;
for (var distance = 8f; distance < route.Length && built < 14; distance += 6f)
{
    var point = route.At(distance);
    foreach (var side in new[] { 5f, -5f })
    {
        var spot = point + new System.Numerics.Vector3(side, 0f, side * 0.5f);
        if (battle.Garrison.TryBuild(0, armory.Specs[built % armory.Specs.Count], spot, out _))
        {
            built++;
        }
    }
}
battle.Begin();
var rig = UnityEngine.Object.FindFirstObjectByType<Game.Stage.Controls.Rig>();
rig.enabled = false;
var focus = route.At(30f);
rig.transform.position = new UnityEngine.Vector3(focus.X, focus.Y, focus.Z) - rig.transform.forward * 45f;
return "built " + built + " towers along a route of " + route.Length + " m";
