using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PragmaManager : EditorWindow
{
    private static PragmaScriptableObject pragmaData;
    private static string newSymbolName = string.Empty;
    private static string newSymbolSign = string.Empty;

    [MenuItem("Tools/PragmaManager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PragmaManager));
        LoadPragmaData();
        UpdatePragmaData();
    }

    private static void UpdatePragmaData()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> symbols = definesString.Split(';').ToList();
        foreach (PragmaScriptableObject.PragmaKeyValue keyValue in pragmaData.pragmaKeys)
        {
            keyValue.IsEnabled = symbols.Contains(keyValue.PragmaPattern);
        }
    }

    private static void LoadPragmaData()
    {
        string path = "Assets/Editor/PragmaManager/PragmaData.asset";
        pragmaData = AssetDatabase.LoadAssetAtPath<PragmaScriptableObject>(path);

    }

    private static void setLayout()
    {
        EditorGUILayout.LabelField("Set Pragmas", EditorStyles.boldLabel, GUILayout.MinHeight(25));
        foreach (PragmaScriptableObject.PragmaKeyValue keyValue in pragmaData.pragmaKeys)
        {
            string toggleName = string.Format("{0} [{1}]", keyValue.Key, keyValue.PragmaPattern);
            bool enable = EditorGUILayout.ToggleLeft(toggleName, keyValue.IsEnabled, GUILayout.MinHeight(25));
            if (keyValue.IsEnabled != enable)
            {
                keyValue.IsEnabled = enable;
            }
        }
        if (GUILayout.Button("Apply"))
        {
            UpdateDefineSymbols();
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        newSymbolName = EditorGUILayout.TextField("Name: ", newSymbolName).Trim();
        newSymbolSign = EditorGUILayout.TextField("Symbol: ", newSymbolSign).Trim();
        EditorGUILayout.EndVertical();
        if (GUILayout.Button("+"))
        {
            AddSymbol();

        }
        if (GUILayout.Button("-"))
        {
            RemoveSymbol();
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void AddSymbol()
    {
        if (string.IsNullOrEmpty(newSymbolName) || string.IsNullOrEmpty(newSymbolSign)) return;
        pragmaData.AddPragmaKey(new PragmaScriptableObject.PragmaKeyValue { Key = newSymbolName, PragmaPattern = newSymbolSign });
        newSymbolName = string.Empty;
        newSymbolSign = string.Empty;
    }

    private static void RemoveSymbol()
    {
        if (string.IsNullOrEmpty(newSymbolName) || string.IsNullOrEmpty(newSymbolSign)) return;
        List<PragmaScriptableObject.PragmaKeyValue> pragmaKeys = pragmaData.pragmaKeys;

        foreach (PragmaScriptableObject.PragmaKeyValue keyValue in pragmaKeys)
        {
            if (keyValue.PragmaPattern.Equals(newSymbolSign) && keyValue.Key.Equals(newSymbolName))
            {
                pragmaKeys.Remove(keyValue);
                newSymbolName = string.Empty;
                newSymbolSign = string.Empty;
                UpdateDefineSymbols();
            }
        }
    }

    private static void UpdateDefineSymbols()
    {
        List<string> symbols = new List<string>();
        foreach (PragmaScriptableObject.PragmaKeyValue keyValue in pragmaData.pragmaKeys)
        {
            if (keyValue.IsEnabled) symbols.Add(keyValue.PragmaPattern);
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", symbols.ToArray()));
    }

    void OnGUI()
    {
        if (pragmaData == null) LoadPragmaData();
        setLayout();
    }
}
