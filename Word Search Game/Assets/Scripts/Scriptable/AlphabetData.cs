using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class AlphabetData : ScriptableObject
{
    [Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite letterSprite;
    }
    public List<LetterData> alphabetList = new List<LetterData>();
}
