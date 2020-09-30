using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectCreator
{
    private static string ability_Asset_Directory = @"Assets/ScriptableObjects/Abilities/";

    #region - Create Assets -  
    [MenuItem("Assets/Twitch/Scriptable Objects/Abilities/Create Offensive Ability")]
    public static void CreateEarTraitAsset()
    {
        Offensive_Ability trait = CreateGenericAsset<Offensive_Ability>(ability_Asset_Directory + @"Offensive/", "NewOffensiveAbility.asset");
    }
    #endregion - Create Assets - 



    private static T CreateGenericAsset<T>(string savePath, string defaultName = "NewItem") where T : ScriptableObject
    {
        if (!defaultName.ToLower().Contains(".asset"))
        {
            defaultName += ".asset";
        }

        if (savePath[savePath.Length - 1] != '/')
        {
            savePath += '/';
        }

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        T newAsset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(newAsset, savePath + defaultName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newAsset;
        return newAsset;
    }
}
