using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public string catagoryName;
    public LevelData selectedLevelData;
}
