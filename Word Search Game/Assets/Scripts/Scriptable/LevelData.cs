using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [Serializable]
    public class SearchWord
    {
        [HideInInspector]
        public bool found = false;
        public string word;
    }

    [Serializable]
    public class LevelRow
    {
        public int rowSize;
        public string[] row;

        public LevelRow()
        {
            // Empty Contructor
        }
        public LevelRow(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            rowSize = size;
            row = new string[rowSize];
            ClearRow();
        }
        public void ClearRow()
        {
            for (int i = 0; i < rowSize; i++)
            {
                row[i] = " ";
            }
        }
    }

    public float levelCompleteTime;
    public int rows;
    public int columns;
    public LevelRow[] level;

    public List<SearchWord> SearchableWordList = new List<SearchWord>();

    public void ClearData()
    {
        foreach (var word in SearchableWordList)
        {
            word.found = false;
        }
    }

    public void ClearLevel()
    {
        for (int i = 0; i < columns; i++)
        {
            level[i].ClearRow();
        }
    }

    public void CreateNewLevel()
    {
        level = new LevelRow[columns];
        for (int i = 0; i < columns; i++)
        {
            level[i] = new LevelRow(rows);
        }
    }
}
