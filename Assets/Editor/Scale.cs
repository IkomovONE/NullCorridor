using UnityEngine;
using UnityEditor;

public class ScaleSelected
{
    [MenuItem("Tools/Scale Selected +5%")]
    static void ScaleObjects()
    {
        foreach (Transform t in Selection.transforms)
        {
            Undo.RecordObject(t, "Scale Selected");
            t.localScale *= 1.05f;
        }
    }
}