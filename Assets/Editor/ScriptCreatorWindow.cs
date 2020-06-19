using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptCreatorWindow : EditorWindow
{
    private bool _focused = false;
    private string _scriptName = "NewMonoBehaviour";
    private EditScriptSignatureWindow _editSigWindow;

    [MenuItem("Assets/Create New C# Script")]
    public static void ShowWindow()
    {
        //GetWindow<ScriptCreatorWindow>(true, "Create C# Script", true);
        GetWindowWithRect<ScriptCreatorWindow>(new Rect(Vector2.zero, new Vector2(400f, 75f)), true, "Create C# Script", true);
    }

    private void OnGUI()
    {
        if(Event.current.keyCode == KeyCode.Return)
        {
            CreateScript();
            OpenScript();
            Close();
            return;
        }

        EditorGUILayout.BeginVertical();
        GUI.SetNextControlName("scriptNameTxt");
        _scriptName = EditorGUILayout.TextField("Script Name", _scriptName);
        EditorGUILayout.BeginHorizontal();

        if(!_focused)
        {
            EditorGUI.FocusTextInControl("scriptNameTxt");
            _focused = true;
        }

        if (GUILayout.Button("Edit Signature"))
        {
            _editSigWindow = GetWindowWithRect<EditScriptSignatureWindow>(new Rect(Vector2.zero, new Vector2(400f, 300f)), true, "Edit Signature", true);
        }
        if (GUILayout.Button("Create Script"))
        {
            CreateScript();
            OpenScript();
            Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void CreateScript()
    {
        string scriptTxt =
        (EditorPrefs.GetString("ScriptSignature")) +

            "\n" +
            "\n" +
            "using System;\n" +
            "using System.Collections;\n" +
            "using System.Collections.Generic;\n" +
            "using UnityEngine;\n" +
            "\n" +
            "public class " + _scriptName + " : MonoBehaviour\n" +
            "{\n" +
            "   private void Start()\n" +
            "   {" +
            "       // Stuff In here ran on object creation\n" +
            "   }\n" +
            "\n" +
            "   private void Update()\n" +
            "   {\n" +
            "       // Stuff in here ran each frame\n" +
            "   }\n" +
            "}";
        File.WriteAllText(AssetDatabase.GetAssetPath(Selection.activeObject) + string.Format(@"\{0}.cs", _scriptName), scriptTxt);
        AssetDatabase.Refresh();
    }

    private void OpenScript()
    {
        UnityEngine.Object script = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GetAssetPath(Selection.activeObject) + string.Format(@"\{0}.cs", _scriptName));
        if (script == null)
            return;
        AssetDatabase.OpenAsset(script);
        Debug.Log("Successfully created '" + _scriptName + "'.");
    }
}
