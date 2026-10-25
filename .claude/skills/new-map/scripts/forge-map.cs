const string Name = "NAME";
const int Seed = SEED;
var routes = new[]
{
ROUTES
};
var scenePath = "Assets/Game/Scenes/" + Name + ".unity";
var terrainFolder = "Assets/Game/Maps/" + Name;
var terrainPath = terrainFolder + "/" + Name + "Terrain.asset";
if (!UnityEditor.AssetDatabase.IsValidFolder(terrainFolder))
{
    UnityEditor.AssetDatabase.CreateFolder("Assets/Game/Maps", Name);
}
if (UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainData>(terrainPath) == null)
{
    UnityEditor.AssetDatabase.CopyAsset("Assets/Game/Maps/Meadow/MeadowTerrain.asset", terrainPath);
}
if (UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(scenePath) == null)
{
    UnityEditor.AssetDatabase.CopyAsset("Assets/Game/Scenes/Meadow.unity", scenePath);
}
var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
UnityEditor.Lightmapping.lightingDataAsset = null;
var data = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TerrainData>(terrainPath);
var terrain = UnityEngine.Object.FindFirstObjectByType<UnityEngine.Terrain>();
terrain.terrainData = data;
terrain.GetComponent<UnityEngine.TerrainCollider>().terrainData = data;
var template = UnityEngine.Object.FindFirstObjectByType<Game.Stage.Map.RoutePath>();
foreach (var extra in UnityEngine.Object.FindObjectsByType<Game.Stage.Map.RoutePath>(UnityEngine.FindObjectsSortMode.None))
{
    if (extra != template)
    {
        UnityEngine.Object.DestroyImmediate(extra.gameObject);
    }
}
var paths = new Game.Stage.Map.RoutePath[routes.Length];
for (var r = 0; r < routes.Length; r++)
{
    var path = r == 0 ? template : UnityEngine.Object.Instantiate(template);
    path.name = r == 0 ? "Route" : "Route " + (r + 1);
    while (path.transform.childCount > 0)
    {
        UnityEngine.Object.DestroyImmediate(path.transform.GetChild(0).gameObject);
    }
    for (var p = 0; p < routes[r].Length; p++)
    {
        var point = new UnityEngine.GameObject("Point " + p).transform;
        point.SetParent(path.transform, false);
        point.position = new UnityEngine.Vector3(routes[r][p].x, 0f, routes[r][p].y);
    }
    paths[r] = path;
}
var battle = new UnityEditor.SerializedObject(UnityEngine.Object.FindFirstObjectByType<Game.Stage.BattleScene>());
var list = battle.FindProperty("_routes");
list.arraySize = paths.Length;
for (var r = 0; r < paths.Length; r++)
{
    list.GetArrayElementAtIndex(r).objectReferenceValue = paths[r];
}
battle.FindProperty("_level").objectReferenceValue = UnityEditor.AssetDatabase.LoadAssetAtPath<Game.Stage.Config.LevelAsset>("Assets/Game/Config/Levels/" + Name + ".asset");
battle.ApplyModifiedPropertiesWithoutUndo();
var landscape = new UnityEditor.SerializedObject(UnityEngine.Object.FindFirstObjectByType<Game.Stage.Map.Landscape>());
landscape.FindProperty("_seed").intValue = Seed;
landscape.ApplyModifiedPropertiesWithoutUndo();
Game.Editor.MapForge.Forge();
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
var build = new System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene>(UnityEditor.EditorBuildSettings.scenes);
if (!build.Exists(entry => entry.path == scenePath))
{
    build.Add(new UnityEditor.EditorBuildSettingsScene(scenePath, true));
    UnityEditor.EditorBuildSettings.scenes = build.ToArray();
}
UnityEditor.AssetDatabase.SaveAssets();
return Name + " forged with " + paths.Length + " routes, level " + battle.FindProperty("_level").objectReferenceValue;
