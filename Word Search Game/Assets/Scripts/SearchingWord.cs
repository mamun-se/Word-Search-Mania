using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public TMP_Text displayText;
    public Image crossLine;
    private string word;

    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWord(string _word)
    {
        word = _word;
        displayText.text = word;
    }

    private void CorrectWord(string _word , List<int> squareIndexes)
    {
        if (word == _word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}
