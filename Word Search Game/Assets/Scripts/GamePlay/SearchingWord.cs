using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public TMP_Text displayText; // Text component to display the word
    public Image crossLine; // Image component to display a line crossing out the word
    private string word; // The word to be searched for

    // Register event listeners when the object is enabled
    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord; // Subscribe to the OnCorrectWord event
    }

    // Unregister event listeners when the object is disabled
    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord; // Unsubscribe from the OnCorrectWord event
    }

    // Set the word to be searched for and update the display text
    public void SetWord(string _word)
    {
        word = _word; // Set the word
        displayText.text = word; // Update the display text
    }

    // Method called when a correct word is found
    private void CorrectWord(string _word, List<int> squareIndexes)
    {
        // Check if the correct word matches the word assigned to this instance
        if (word == _word)
        {
            crossLine.gameObject.SetActive(true); // Show the cross line image
            transform.GetComponent<Image>().color = Color.green; // Change the background color to green
        }
    }
}
