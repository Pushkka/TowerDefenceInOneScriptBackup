using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boyeing : MonoBehaviour
{
    public bool[] Buttons = new bool[8];
    public bool[] Buttons1 = new bool[8];
    public bool[] Buttons2 = new bool[8];
    public bool[] Buttons3 = new bool[8];
    public bool[] Buttons4 = new bool[8];
    public bool[] Buttons5 = new bool[8];
    public bool[] Buttons6 = new bool[8];
    public bool[] Buttons7 = new bool[8];
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

[CustomEditor(typeof(Boyeing))]
[CanEditMultipleObjects]
public class BoyeingEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons").GetArrayElementAtIndex(i).boolValue;
            if(value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons1").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons1").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons1").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons2").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons2").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons2").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons3").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons3").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons3").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons4").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons4").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons4").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons5").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons5").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons5").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons6").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;

            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons6").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons6").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 8; i++)
        {
            GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Button"));
            bool value = serializedObject.FindProperty("Buttons7").GetArrayElementAtIndex(i).boolValue;
            if (value)
                style.normal.textColor = Color.green;
            else
                style.normal.textColor = Color.red;
            if (GUILayout.Button(value.ToString(), style, GUILayout.Width((EditorGUIUtility.currentViewWidth - 57) / 8)))
            {
                if (value)
                    serializedObject.FindProperty("Buttons7").GetArrayElementAtIndex(i).boolValue = false;
                else
                    serializedObject.FindProperty("Buttons7").GetArrayElementAtIndex(i).boolValue = true;
            }
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
