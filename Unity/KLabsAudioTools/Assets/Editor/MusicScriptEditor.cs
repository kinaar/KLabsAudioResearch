using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicScript))]
public class MusicScriptEditor : Editor
{
    SerializedProperty m_outputMixerGroup;
    SerializedProperty m_muteGeneral;
    SerializedProperty m_userBpm;

    SerializedProperty m_playType;
    SerializedProperty m_triggerObject;
    SerializedProperty m_musicalSegment;
    SerializedProperty m_segmentBarLength;
    SerializedProperty m_fadeInTime;

    void OnEnable()
    {
        m_outputMixerGroup = serializedObject.FindProperty("m_outputMixerGroup");
        m_muteGeneral = serializedObject.FindProperty("m_muteGeneral");
        m_userBpm = serializedObject.FindProperty("m_userBpm");

        m_playType = serializedObject.FindProperty("m_playType");
        m_triggerObject = serializedObject.FindProperty("m_triggerObject");
        m_musicalSegment = serializedObject.FindProperty("m_musicalSegment");
        m_segmentBarLength = serializedObject.FindProperty("m_segmentBarLength");
        m_fadeInTime = serializedObject.FindProperty("m_fadeInTime");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_outputMixerGroup);
        EditorGUILayout.PropertyField(m_muteGeneral);
        EditorGUILayout.PropertyField(m_userBpm);
        
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("First Segment Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_playType);

        if(m_playType.intValue == 0)
        {
            EditorGUILayout.PropertyField(m_triggerObject);
        }

        EditorGUILayout.PropertyField(m_musicalSegment);
        EditorGUILayout.PropertyField(m_segmentBarLength);
        EditorGUILayout.Slider(m_fadeInTime, 0.0f, 20.0f);

        serializedObject.ApplyModifiedProperties();
    }
}
