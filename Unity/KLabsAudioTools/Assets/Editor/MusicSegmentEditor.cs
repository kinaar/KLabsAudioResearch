using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicSegment))]
public class MusicSegmentEditor : Editor
{
    SerializedProperty m_triggerObject;
    SerializedProperty m_musicalSegment;
    SerializedProperty m_segmentBarLength;
    SerializedProperty m_transition;
    SerializedProperty m_crossfadeTime;
    SerializedProperty m_fadeOutTime;

    void OnEnable()
    {
        m_triggerObject = serializedObject.FindProperty("m_triggerObject");
        m_musicalSegment = serializedObject.FindProperty("m_musicalSegment");
        m_segmentBarLength = serializedObject.FindProperty("m_segmentBarLength");
        m_transition = serializedObject.FindProperty("m_transition");
        m_crossfadeTime = serializedObject.FindProperty("m_crossfadeTime");
        m_fadeOutTime = serializedObject.FindProperty("m_fadeOutTime");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.LabelField("Segment Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_triggerObject);
        EditorGUILayout.PropertyField(m_musicalSegment);
        EditorGUILayout.PropertyField(m_segmentBarLength);
        EditorGUILayout.PropertyField(m_transition);

        if(m_transition.intValue == 1) EditorGUILayout.Slider(m_crossfadeTime, 0.1f, 5.0f);
        if(m_transition.intValue == 2) EditorGUILayout.Slider(m_fadeOutTime, 0.1f, 20.0f);

        serializedObject.ApplyModifiedProperties();
    }
}
