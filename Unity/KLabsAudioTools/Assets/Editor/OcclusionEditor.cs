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

        serializedObject.ApplyModifiedProperties();
    }
}
