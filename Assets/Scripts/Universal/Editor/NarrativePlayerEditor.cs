using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NarrativePlayer))]
public class NarrativePlayerEditor : Editor
{
    NarrativePlayer player;

    void OnEnable()
    {
        player = (NarrativePlayer)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (GUILayout.Button("Assign Text & Clip"))
            AssignValues();
    }

    void AssignValues()
    {
        player.textComponent.text = player.text;
        player.audioSource.clip = player.clip;

        PrefabUtility.RecordPrefabInstancePropertyModifications(player.textComponent);
    }
}
