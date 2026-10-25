var document = UnityEngine.Object.FindFirstObjectByType<UnityEngine.UIElements.UIDocument>();
var button = UnityEngine.UIElements.UQueryExtensions.Q<UnityEngine.UIElements.Button>(document.rootVisualElement, "BUTTON");
using (var submit = UnityEngine.UIElements.NavigationSubmitEvent.GetPooled())
{
    submit.target = button;
    button.SendEvent(submit);
}
return "clicked BUTTON, timeScale " + UnityEngine.Time.timeScale;
