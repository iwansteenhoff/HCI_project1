using UnityEditor;

[CustomEditor(typeof(MapStyleController))]
public class MapStyleControllerEditor : Editor {
    public override void OnInspectorGUI() {
        MapStyleController controller = (MapStyleController)target;
        base.OnInspectorGUI();
        controller.UpdateSettings();
    }
}
