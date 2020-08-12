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

    void OnEnable()
    {
        m_occludedVol = serializedObject.FindProperty("m_occludedVol");
        m_volFadeTime = serializedObject.FindProperty("m_volFadeTime");
        m_fqHalfOccluded = serializedObject.FindProperty("m_fqHalfOccluded");
        m_fqFullOccluded = serializedObject.FindProperty("m_fqFullOccluded");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("Occlusion Settings", EditorStyles.boldLabel);
        EditorGUILayout.Slider(m_occludedVol, -48.0f, 0.0f);
        EditorGUILayout.PropertyField(m_volFadeTime);
        EditorGUILayout.Slider(m_fqHalfOccluded, 50.0f, 15000.0f);
        EditorGUILayout.Slider(m_fqFullOccluded, 50.0f, 15000.0f);

        serializedObject.ApplyModifiedProperties();
    }
}
