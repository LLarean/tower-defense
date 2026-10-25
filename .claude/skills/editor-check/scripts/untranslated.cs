UnityEditor.AssetDatabase.Refresh();
var lexicon = new Game.Stage.Speech.Lexicon(System.IO.File.ReadAllText("Assets/Game/Config/Locales/ru.txt"));
var missing = new System.Collections.Generic.List<string>();
foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Game/Config" }))
{
    var asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
    if (asset is Game.Stage.Config.BreedAsset && lexicon.Say(asset.name) == asset.name) missing.Add(asset.name);
    var property = new UnityEditor.SerializedObject(asset).GetIterator();
    while (property.Next(true))
    {
        if (property.propertyType == UnityEditor.SerializedPropertyType.String && property.name != "m_EditorClassIdentifier" && property.name != "m_Name" && property.name != "_scene" && property.name != "_look" && property.name != "_title" || (property.propertyType == UnityEditor.SerializedPropertyType.String && property.name == "_title" && !(asset is Game.Stage.Config.LanguageAsset)))
        {
            var text = property.stringValue;
            if (text.Length > 0 && lexicon.Say(text) == text) missing.Add(text.Substring(0, System.Math.Min(60, text.Length)));
        }
    }
}
foreach (var uxml in new[] { "Assets/Game/UI/Hud.uxml", "Assets/Game/UI/Menu.uxml", "Assets/Game/UI/Codex.uxml" })
{
    var tree = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.UIElements.VisualTreeAsset>(uxml).Instantiate();
    UnityEngine.UIElements.UQueryExtensions.Query<UnityEngine.UIElements.TextElement>(tree).ForEach(element =>
    {
        if (element.text.Length > 0 && lexicon.Say(element.text) == element.text) missing.Add(uxml.Substring(15) + ": " + element.text.Substring(0, System.Math.Min(50, element.text.Length)));
    });
}
return "missing " + missing.Count + ": " + string.Join(" | ", missing);
