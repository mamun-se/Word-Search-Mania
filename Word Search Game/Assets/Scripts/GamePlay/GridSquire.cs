using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class GridSquire : MonoBehaviour
{
    public int squireIndex { get; set; } // Public property for square index
    private AlphabetData.LetterData letterData; // Data for the letter on this square
    private SpriteRenderer displayImage; // Sprite renderer for displaying the letter image
    private bool selected; // Indicates if the square is selected
    private bool clicked; // Indicates if the square is currently being clicked
    private int index = -1; // Index of the square in the grid
    private List<int> selectedSquares = new List<int>(); // List to store selected square indices

    private WordChecker wordChecker; // Reference to the WordChecker instance

    void Start()
    {
        displayImage = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        selected = false; // Initialize selected state
        clicked = false; // Initialize clicked state
        wordChecker = FindObjectOfType<WordChecker>(); // Find the WordChecker instance in the scene
    }

    // Method to set the sprite of the square
    public void SetSprite(AlphabetData.LetterData _letterData)
    {
        letterData = _letterData;
        GetComponent<SpriteRenderer>().sprite = letterData.letterSprite; // Set the sprite to the letter image
    }

    // Register event listeners
    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += OnSelectSquare;
    }

    // Unregister event listeners
    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= OnSelectSquare;
    }

    // Event handler for enabling square selection
    public void OnEnableSquareSelection()
    {
        clicked = true; // Mark the square as clicked
        selected = false; // Reset selected state
        selectedSquares.Clear(); // Clear the selected squares list
    }

    // Event handler for disabling square selection
    public void OnDisableSquareSelection()
    {
        selected = false; // Reset selected state
        clicked = false; // Reset clicked state
    }

    // Event handler for selecting a square (currently not used)
    public void OnSelectSquare(Vector3 position)
    {
        // For Future Implementation
    }

    // Event handler for mouse down event
    private void OnMouseDown()
    {
        OnEnableSquareSelection(); // Enable square selection
        GameEvents.EnableSquireSelectionMethod(); // Trigger the event for enabling square selection
        CheckSquare(); // Check the current square
    }

    // Event handler for mouse enter event
    private void OnMouseEnter()
    {
        if (clicked)
        {
            CheckSquare(); // Check the current square if clicked
        }
    }

    // Event handler for mouse up event
    private void OnMouseUp()
    {
        wordChecker.CheckWord(); // Check if the word is correct
        GameEvents.ClearSelectionMethod(); // Trigger the event for clearing selection
        GameEvents.DisableSquireSelectionMethod(); // Trigger the event for disabling square selection
    }

    // Method to check the current square
    public void CheckSquare()
    {
        if (!selected && clicked)
        {
            selected = true; // Mark the square as selected
            selectedSquares.Add(index); // Add the square index to the selected list
            GameEvents.CheckSquareMethod(letterData.letter, transform.position, index); // Trigger the event for checking the square
        }
    }

    // Method to set the index of the square
    public void SetIndex(int _index)
    {
        index = _index;
    }

    // Method to get the index of the square
    public int GetIndex() { return index; }
}
