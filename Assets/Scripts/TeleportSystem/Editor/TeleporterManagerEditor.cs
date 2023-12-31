﻿using System.Linq;
using UnityEditor;

[CustomEditor(typeof(TeleporterManager))]
public class TeleporterManagerEditor : Editor
{
    public Teleporter[] teleporters;
    public string[] options;
    public int[] index;

    private void OnEnable()
    {
        teleporters = FindObjectsOfType<Teleporter>();

        // Order teleporters by name (ascending)
        teleporters = (from teleporter in teleporters 
                       orderby teleporter.teleporterName 
                       select teleporter).ToArray();

        options = new string[teleporters.Length + 1];
        options[0] = "NONE";
        index = new int[teleporters.Length];
        for (int i = 0; i < teleporters.Length; i++)
        {
            options[i + 1] = teleporters[i].teleporterName;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Teleporter", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Target", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        for (int i = 0; i < teleporters.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(teleporters[i].teleporterName);
            index[i] = EditorGUILayout.Popup(FindTeleporterIndex(teleporters[i].linkedTeleporter), options);
            if (index[i] != FindTeleporterIndex(teleporters[i].linkedTeleporter))
                EditorUtility.SetDirty(teleporters[i]);
            if (index[i] == 0)
                teleporters[i].linkedTeleporter = null;
            else
                teleporters[i].linkedTeleporter = teleporters[index[i] - 1];
            EditorGUILayout.EndHorizontal();
        }
    }
    
    private int FindTeleporterIndex(Teleporter teleporter)
    {
        for (int i = 0; i < teleporters.Length; i++)
        {
            if (teleporter == teleporters[i])
                return i + 1;
        }

        return 0;
    }
}