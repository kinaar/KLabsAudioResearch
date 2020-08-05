using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AmbienceObject))]
public class AmbienceObjectEditor : Editor
{

    SerializedProperty m_ambienceManager;
    SerializedProperty m_outputMixerGroup;
    SerializedProperty m_generalVolume;
    SerializedProperty m_muteGeneral;
    SerializedProperty m_fadeInTime;
    SerializedProperty m_fadeOutTime;
    SerializedProperty m_bed;
    SerializedProperty m_bedVolume;
    SerializedProperty m_muteBed;

    void OnEnable()
    {
        m_ambienceManager = serializedObject.FindProperty("m_ambienceManager");
        m_outputMixerGroup = serializedObject.FindProperty("m_outputMixerGroup");
        m_generalVolume = serializedObject.FindProperty("m_generalVolume");
        m_muteGeneral = serializedObject.FindProperty("m_muteGeneral");
        m_fadeInTime = serializedObject.FindProperty("m_fadeInTime");
        m_fadeOutTime = serializedObject.FindProperty("m_fadeOutTime");
        m_bed = serializedObject.FindProperty("m_bed");
        m_bedVolume = serializedObject.FindProperty("m_bedVolume");
        m_muteBed = serializedObject.FindProperty("m_muteBed");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_ambienceManager);
        EditorGUILayout.PropertyField(m_outputMixerGroup);
        EditorGUILayout.Slider(m_generalVolume, -48.0f, 0.0f);
        EditorGUILayout.PropertyField(m_muteGeneral);
        EditorGUILayout.PropertyField(m_fadeInTime);
        EditorGUILayout.PropertyField(m_fadeOutTime);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Bed Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_bed);
        EditorGUILayout.Slider(m_bedVolume, -48.0f, 0.0f);
        EditorGUILayout.PropertyField(m_muteBed);

        serializedObject.ApplyModifiedProperties();
    }
}
