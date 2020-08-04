using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AmbienceRandomLayer))]
public class AmbienceRandomLayerEditor : Editor
{

    SerializedProperty m_randomVolume;
    SerializedProperty m_mute;
    SerializedProperty m_randomClips;
    SerializedProperty m_spatialize;
    SerializedProperty m_randomZoneMin;
    SerializedProperty m_randomZoneMax;
    SerializedProperty m_minAttenuationDistance;
    SerializedProperty m_maxAttenuationDistance;
    SerializedProperty m_spread;
    SerializedProperty m_Rolloff;
    SerializedProperty m_triggerTime;
    SerializedProperty m_randomTime;
    SerializedProperty m_panRandomization;
    SerializedProperty m_volRandomization;

    void OnEnable()
    {
        m_randomVolume = serializedObject.FindProperty("m_randomVolume");
        m_mute = serializedObject.FindProperty("m_muteRandom");
        m_randomClips = serializedObject.FindProperty("m_randomClips");
        m_spatialize = serializedObject.FindProperty("m_spatialize");
        m_randomZoneMin = serializedObject.FindProperty("m_randomZoneMin");
        m_randomZoneMax = serializedObject.FindProperty("m_randomZoneMax");
        m_minAttenuationDistance = serializedObject.FindProperty("m_minAttenuationDistance");
        m_maxAttenuationDistance = serializedObject.FindProperty("m_maxAttenuationDistance");
        m_spread = serializedObject.FindProperty("m_spread");
        m_Rolloff = serializedObject.FindProperty("m_Rolloff");
        m_triggerTime = serializedObject.FindProperty("m_triggerTime");
        m_randomTime = serializedObject.FindProperty("m_randomTime");
        m_panRandomization = serializedObject.FindProperty("m_panRandomization");
        m_volRandomization = serializedObject.FindProperty("m_volRandomization");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("Layer Settings", EditorStyles.boldLabel);
        EditorGUILayout.Slider(m_randomVolume, -48.0f, 0.0f);
        EditorGUILayout.PropertyField(m_mute);
        EditorGUILayout.PropertyField(m_spatialize);
        if(m_spatialize.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Spatial Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_randomZoneMin);
            EditorGUILayout.PropertyField(m_randomZoneMax);
            EditorGUILayout.PropertyField(m_minAttenuationDistance);
            EditorGUILayout.PropertyField(m_maxAttenuationDistance);
            EditorGUILayout.Slider(m_spread, 0.0f, 1.0f);
            EditorGUILayout.PropertyField(m_Rolloff);
        }
        EditorGUILayout.Space();        
        EditorGUILayout.LabelField("AudioClips Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_randomClips);
        EditorGUILayout.PropertyField(m_triggerTime);
        EditorGUILayout.PropertyField(m_randomTime);
        EditorGUILayout.Slider(m_panRandomization, 0.0f, 100.0f);
        if(!m_spatialize.boolValue)
        {
            EditorGUILayout.Slider(m_volRandomization, 0.0f, 100.0f);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
