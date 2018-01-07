using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UssIndicator : MonoBehaviour
{
    static public void drawString(string text, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(
            view.camera.transform.position + view.camera.transform.forward);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            return;
        }
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 50;
        Vector2 size = style.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text, style);
        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI();
    }
    void OnDrawGizmos()
    {
        if (UssStyleModifier.hasError == false)
            return;

        drawString("Your .ucss has parsing error(s).\r\n" +
            "Please check console for details",
            Color.red);
    }
}
