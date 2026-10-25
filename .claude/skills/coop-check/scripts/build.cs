var scenes = new System.Collections.Generic.List<string>();
foreach (var scene in UnityEditor.EditorBuildSettings.scenes) if (scene.enabled) scenes.Add(scene.path);
var report = UnityEditor.BuildPipeline.BuildPlayer(new UnityEditor.BuildPlayerOptions
{
    scenes = scenes.ToArray(),
    locationPathName = "Build/Windows/BattleOfTheElements.exe",
    target = UnityEditor.BuildTarget.StandaloneWindows64,
    options = UnityEditor.BuildOptions.Development,
});
return report.summary.result + " errors " + report.summary.totalErrors + " size " + (report.summary.totalSize / 1048576) + "MB time " + report.summary.totalTime;
