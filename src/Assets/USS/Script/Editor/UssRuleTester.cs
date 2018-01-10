using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UssRuleTester : EditorWindow
{
    private string query;

    [MenuItem("USS/SelectorTester")]
    public static void ShowRuleTester()
    {
        var window = new UssRuleTester();
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        query = GUILayout.TextField(query, GUI.skin.FindStyle("ToolbarSeachTextField"));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (GUI.changed && string.IsNullOrEmpty(query) == false)
        {
            var conditions = UssParser.ParseConditions(query);
            Debug.Log(conditions.Length);
            Selection.objects = UssStyleModifier.FindObjects(UssRoot.FindRootInScene(), conditions);
        }
    }
}
