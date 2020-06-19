using UnityEditor;
using UnityEngine;

public class EditScriptSignatureWindow : EditorWindow
{
    private string signature = "";
    private Vector2 scroll;

    private void OnEnable()
    {
        signature = EditorPrefs.GetString("ScriptSignature");
    }

    private void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        signature = EditorGUILayout.TextArea(signature, GUILayout.Height(250f));
        EditorGUILayout.EndScrollView();

        if(GUILayout.Button("Save"))
        {
            EditorPrefs.SetString("ScriptSignature", signature);
            Close();
        }
    }
}
