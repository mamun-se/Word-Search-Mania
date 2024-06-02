using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelData),false)]
[CanEditMultipleObjects]
[Serializable]
public class LevelDataEditor : Editor
{
    private LevelData levelDataInstance => (LevelData)target;
    private ReorderableList levelDataList;

    private void OnEnable()
    {
        InitReorderableList(ref levelDataList, "SearchableWordList", "Search Words");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawFieldsForLevel();
        EditorGUILayout.Space();
        if (levelDataInstance.level != null && levelDataInstance.rows > 0 && levelDataInstance.columns > 0)
        {
            DrawLevelTable();
        }
        EditorGUILayout.Space();
        levelDataList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelDataInstance);
        }
    }

    private void DrawFieldsForLevel()
    {
        int tempColumn = levelDataInstance.columns;
        int tempRow = levelDataInstance.rows;
        levelDataInstance.columns = EditorGUILayout.IntField("Columns",levelDataInstance.columns);
        levelDataInstance.rows = EditorGUILayout.IntField("Rows", levelDataInstance.rows);
        if ((levelDataInstance.columns != tempColumn || levelDataInstance.rows != tempRow) && levelDataInstance.columns > 0 && levelDataInstance.rows > 0)
        {
            levelDataInstance.CreateNewLevel();
        }
    }

    private void DrawLevelTable()
    {
        GUIStyle tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10,10,10,10);
        tableStyle.margin.left = 32;
        GUIStyle columnHeaderStyle = new GUIStyle();
        columnHeaderStyle.fixedWidth = 35;
        GUIStyle columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;
        GUIStyle rowStyle = new GUIStyle();
        rowStyle.fixedWidth = 40;
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle textFieldStyle = new GUIStyle();
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);

        for (int i = 0; i < levelDataInstance.columns; i++)
        {
            EditorGUILayout.BeginVertical(i == -1 ? columnHeaderStyle : columnStyle);
            for (int j = 0;  j < levelDataInstance.rows;  j++)
            {
                if (i >= 0 && j >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    string charecter = EditorGUILayout.TextArea(levelDataInstance.level[i].row[j], textFieldStyle);

                    if (levelDataInstance.level[i].row[j].Length > 1)
                    {
                        charecter = levelDataInstance.level[i].row[j].Substring(0, 1);
                    }
                    levelDataInstance.level[i].row[j] = charecter;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

    }

    private void InitReorderableList(ref ReorderableList list, string propertyName, string listName)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listName);
        };
        ReorderableList tempList = list;
        list.drawElementCallback = (Rect rect, int index , bool isActive , bool isFocused) =>
        {
            SerializedProperty element = tempList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("word"),GUIContent.none);
        };
    }
}
