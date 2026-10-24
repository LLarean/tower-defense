using Game.Stage.Map;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Editor
{
    public static class MapForge
    {
        [MenuItem("Game/Forge Terrain")]
        public static void Forge()
        {
            var landscape = Object.FindFirstObjectByType<Landscape>();
            var terrain = Object.FindFirstObjectByType<Terrain>();
            var paths = Object.FindObjectsByType<RoutePath>(FindObjectsSortMode.None);
            var corridor = new Corridor(paths);
            var origin = terrain.transform.position;
            var size = terrain.terrainData.size;
            var relief = new Relief(landscape, corridor, new Rect(origin.x, origin.z, size.x, size.z));
            Undo.RegisterCompleteObjectUndo(terrain.terrainData, "Forge Terrain");
            new Heightmap(relief).Apply(terrain);
            new Splatmap(relief, landscape).Apply(terrain);
            new Flora(relief, corridor, landscape).Apply(terrain);
            foreach (var path in paths)
            {
                foreach (Transform point in path.transform)
                {
                    var position = point.position;
                    position.y = terrain.SampleHeight(position) + origin.y;
                    Undo.RecordObject(point, "Forge Terrain");
                    point.position = position;
                }
            }
            EditorUtility.SetDirty(terrain.terrainData);
            EditorSceneManager.MarkSceneDirty(terrain.gameObject.scene);
        }
    }
}
