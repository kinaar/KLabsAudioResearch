using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OcclusionFilter))]
public class OcclusionEditor : Editor
{
    SerializedProperty m_occludedVol;
    SerializedProperty m_volFadeTime;
    SerializedProperty m_fqHalfOccluded;
    SerializedProperty m_fqFullOccluded;
    SerializedProperty m_listener;
    SerializedProperty m_freqFadeTime;
    SerializedProperty m_spreadHalfOccluded;
    SerializedProperty m_spreadFullOccluded;
    SerializedProperty m_volume;
    SerializedProperty m_audioMixer;
    SerializedProperty m_parameterName;
    SerializedProperty m_reverbLevel;
    SerializedProperty m_reverbLevelOccluded;
    SerializedProperty m_reverbFadeTime;
    SerializedProperty m_useReverb;

    void OnEnable()
    {
        m_occludedVol = serializedObject.FindProperty("m_occludedVol");
        m_volFadeTime = serializedObject.FindProperty("m_volFadeTime");
        m_volume = serializedObject.FindProperty("m_volume");
        m_fqHalfOccluded = serializedObject.FindProperty("m_fqHalfOccluded");
        m_fqFullOccluded = serializedObject.FindProperty("m_fqFullOccluded");
        m_freqFadeTime = serializedObject.FindProperty("m_freqFadeTime");
        m_spreadHalfOccluded = serializedObject.FindProperty("m_spreadHalfOccluded");
        m_spreadFullOccluded = serializedObject.FindProperty("m_spreadFullOccluded");
        m_listener = serializedObject.FindProperty("m_listener");
        m_useReverb = serializedObject.FindProperty("m_useReverb");
        m_audioMixer = serializedObject.FindProperty("m_audioMixer");
        m_parameterName = serializedObject.FindProperty("m_parameterName");
        m_reverbLevel = serializedObject.FindProperty("m_reverbLevel");
        m_reverbLevelOccluded = serializedObject.FindProperty("m_reverbLevelOccluded");
        m_reverbFadeTime = serializedObject.FindProperty("m_reverbFadeTime");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_listener);

        EditorGUILayout.LabelField("Volume Settings", EditorStyles.boldLabel);
        EditorGUILayout.Slider(m_volume, -48.0f, 0.0f);
        EditorGUILayout.Slider(m_occludedVol, -48.0f, 0.0f);
        EditorGUILayout.Slider(m_volFadeTime, 0.05f, 2.0f);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("EQ Settings", EditorStyles.boldLabel);
        EditorGUILayout.Slider(m_fqHalfOccluded, 50.0f, 15000.0f);
        EditorGUILayout.Slider(m_fqFullOccluded, 50.0f, 15000.0f);
        EditorGUILayout.Slider(m_freqFadeTime, 0.05f, 2.0f);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Spread Settings", EditorStyles.boldLabel);
        EditorGUILayout.Slider(m_spreadHalfOccluded, 0.0f, 100.0f);
        EditorGUILayout.Slider(m_spreadFullOccluded, 0.0f, 100.0f);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Reverb Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_useReverb);

        if (m_useReverb.boolValue)
        {
            EditorGUILayout.PropertyField(m_audioMixer);
            EditorGUILayout.PropertyField(m_parameterName);
            EditorGUILayout.Slider(m_reverbLevelOccluded, -48.0f, 0.0f);
            EditorGUILayout.Slider(m_reverbLevel, -48.0f, 0.0f);
            EditorGUILayout.Slider(m_reverbFadeTime, 0.05f, 2.0f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
