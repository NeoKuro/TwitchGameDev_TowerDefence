using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectCreator
{
    private static string ability_Asset_Directory = @"Assets/ScriptableObjects/Abilities/";
    private static string combat_Asset_Directory = @"Assets/ScriptableObjects/Combat/";

    #region - Create Assets -  
    [MenuItem("Assets/Twitch/Scriptable Objects/Combat/Abilities/Create Offensive Ability")]
    public static void CreateOffensiveAbility()
    {
        Offensive_Ability trait = CreateGenericAsset<Offensive_Ability>(ability_Asset_Directory + @"Offensive/", "NewOffensiveAbility.asset");
    }

    [MenuItem("Assets/Twitch/Scriptable Objects/Combat/Create Combat Type")]
    public static void CreateCombatType()
    {
        CombatTypeBase trait = CreateGenericAsset<CombatTypeBase>(ability_Asset_Directory + @"CombatTypes/", "NewCombatType.asset");
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
