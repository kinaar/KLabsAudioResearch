using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterAudioManager))]
public class CharacterEditor : Editor
{

    SerializedProperty m_audioMixer;
    SerializedProperty m_audioVolume;
    SerializedProperty m_mute;
    SerializedProperty m_triggerChoice;
    SerializedProperty m_audioClips;
    SerializedProperty m_customKey;
    SerializedProperty m_randomPitch;


    void OnEnable()
    {
        m_audioMixer = serializedObject.FindProperty("m_audioMixer");
        m_audioVolume = serializedObject.FindProperty("m_volume");
        m_mute = serializedObject.FindProperty("m_mute");
        m_triggerChoice = serializedObject.FindProperty("m_triggerChoice");
        m_audioClips = serializedObject.FindProperty("m_audioClips");
        m_customKey = serializedObject.FindProperty("m_customKey");
        m_randomPitch = serializedObject.FindProperty("m_randomPitch");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_audioMixer);
        EditorGUILayout.Slider(m_audioVolume, -48.0f, 0.0f);
        EditorGUILayout.PropertyField(m_mute);
        //EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_audioClips);
        EditorGUILayout.PropertyField(m_triggerChoice);

        switch(m_triggerChoice.intValue)
        {
            case 3: EditorGUILayout.PropertyField(m_customKey);
            break;
        }
        
        EditorGUILayout.Slider(m_randomPitch, 0.0f, 100.0f);

        serializedObject.ApplyModifiedProperties();
    }
}
