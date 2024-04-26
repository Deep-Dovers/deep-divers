using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefs : EditorWindow
{
    [MenuItem("Custom Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}