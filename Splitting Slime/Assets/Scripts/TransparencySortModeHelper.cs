using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TransparencySortModeHelper : MonoBehaviour
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(TransparencySortModeHelper))]
public class TransparencySortModeHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var camera = (target as TransparencySortModeHelper).GetComponent<Camera>();
        EditorGUI.BeginChangeCheck();
        var sortMode = (TransparencySortMode)EditorGUILayout.EnumPopup("Sort Mode", camera.transparencySortMode);
        var sortAxis = EditorGUILayout.Vector3Field("Sort Axis", camera.transparencySortAxis);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(camera, "Change Sort");
            camera.transparencySortMode = sortMode;
            camera.transparencySortAxis = sortAxis;
        }
    }
}
#endif
