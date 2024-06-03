using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class WordChecker : MonoBehaviour
{
    public GameData currentgameData; // Reference to the current game data
    private string word; // The word being formed by the player's selection

    private int assignedPoints = 0; // Tracks the number of squares selected
    private int completedWords = 0; // Tracks the number of words completed
    private Ray rayUp, rayDown; // Rays for vertical directions
    private Ray rayLeft, rayRight; // Rays for horizontal directions
    private Ray rayDiagonalLeftUp, rayDiagonalLeftDown; // Rays for left diagonal directions
    private Ray rayDiagonalRightUp, rayDiagonalRightDown; // Rays for right diagonal directions
    private Ray currentRay = new Ray(); // The current ray being checked

    private Vector3 rayStartPosition; // The starting position of the ray
    private List<int> correctSquareList = new List<int>(); // List of correctly selected squares
    private List<Vector3> selectedPositions = new List<Vector3>(); // List of positions of selected squares

    private GameManager gameManager; // Reference to the GameManager

    // Register event listeners when the object is enabled
    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected; // Subscribe to the OnCheckSquare event
        GameEvents.OnClearSelection += ClearSelection; // Subscribe to the OnClearSelection event
    }

    // Unregister event listeners when the object is disabled
    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected; // Unsubscribe from the OnCheckSquare event
        GameEvents.OnClearSelection -= ClearSelection; // Unsubscribe from the OnClearSelection event
    }

    // Initialize the game data and find the GameManager
    private void Start()
    {
        currentgameData.selectedLevelData.ClearData(); // Clear the game data
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager instance in the scene
    }

    // Method called when a square is selected
    private void SquareSelected(string letter, Vector3 squarePos, int squareIndex)
    {
        if (assignedPoints == 0)
        {
            gameManager.StartNewSelection(); // Start a new selection in the game manager
            rayStartPosition = squarePos; // Set the starting position for the ray
            correctSquareList.Add(squareIndex); // Add the square index to the list of correct squares
            selectedPositions.Add(squarePos); // Add the position to the list of selected positions
            word += letter; // Add the letter to the current word
            // Initialize rays in all directions
            rayUp = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(0f, 1f));
            rayDown = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(0f, -1f));
            rayLeft = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(-1f, 0f));
            rayRight = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(1f, 0f));
            rayDiagonalLeftUp = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(-1f, 1f));
            rayDiagonalLeftDown = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(-1f, -1f));
            rayDiagonalRightUp = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(1f, 1f));
            rayDiagonalRightDown = new Ray(new Vector2(squarePos.x, squarePos.y), new Vector2(1f, -1f));
        }
        else if (assignedPoints == 1)
        {
            correctSquareList.Add(squareIndex); // Add the square index to the list of correct squares
            selectedPositions.Add(squarePos); // Add the position to the list of selected positions
            currentRay = SelectRay(rayStartPosition, squarePos); // Select the appropriate ray based on direction
            GameEvents.SelectSquareMethod(squarePos); // Notify other components about the selected square
            word += letter; // Add the letter to the current word
        }
        else
        {
            if (IsPointOnRay(currentRay, squarePos)) // Check if the point is on the current ray
            {
                correctSquareList.Add(squareIndex); // Add the square index to the list of correct squares
                selectedPositions.Add(squarePos); // Add the position to the list of selected positions
                GameEvents.SelectSquareMethod(squarePos); // Notify other components about the selected square
                word += letter; // Add the letter to the current word
            }
        }
        gameManager.UpdateLineRenderer(selectedPositions); // Update the line renderer with the selected positions
        assignedPoints++; // Increment the assigned points
    }

    // Check if the formed word is correct
    public void CheckWord()
    {
        foreach (var searchingWord in currentgameData.selectedLevelData.SearchableWordList)
        {
            if (word == searchingWord.word && searchingWord.found == false)
            {
                searchingWord.found = true; // Mark the word as found
                GameEvents.CorrectWordMethod(word, correctSquareList); // Notify other components about the correct word
                // Create a new line renderer to mark the correct word
                gameManager.CreatePermanentLineRenderer(selectedPositions);
                completedWords++; // Increment the completed words count
                word = string.Empty; // Clear the current word
                correctSquareList.Clear(); // Clear the list of correct squares
                selectedPositions.Clear(); // Clear the list of selected positions
                CheckBoardCompleted(); // Check if the board is completed
                return;
            }
        }
        // If the word is incorrect, clear the line renderer
        ClearSelection();
    }

    // Select the appropriate ray based on the direction between two positions
    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2 direction = (secondPosition - firstPosition).normalized; // Calculate the direction
        float tolerance = 0.1f; // Tolerance for direction comparison
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - 1) < tolerance)
        {
            return rayUp;
        }
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - (-1)) < tolerance)
        {
            return rayDown;
        }
        if (Math.Abs(direction.x - (-1)) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return rayLeft;
        }
        if (Math.Abs(direction.x - 1) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return rayRight;
        }

        if (direction.x < 0 && direction.y > 0)
        {
            return rayDiagonalLeftUp;
        }
        if (direction.x < 0 && direction.y < 0)
        {
            return rayDiagonalLeftDown;
        }
        if (direction.x > 0 && direction.y > 0)
        {
            return rayDiagonalRightUp;
        }
        if (direction.x < 0 && direction.y > 0)
        {
            return rayDiagonalRightDown;
        }
        return rayDown;
    }

    // Check if a point is on the given ray
    private bool IsPointOnRay(Ray ray, Vector3 point)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f); // Perform a raycast
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point) // Check if the hit point matches the given point
            {
                return true;
            }
        }
        return false;
    }

    // Clear the current selection
    private void ClearSelection()
    {
        assignedPoints = 0; // Reset assigned points
        correctSquareList.Clear(); // Clear the list of correct squares
        selectedPositions.Clear(); // Clear the list of selected positions
        word = string.Empty; // Clear the current word
        gameManager.ClearLineRenderer(); // Clear the line renderer in the game manager
    }

    // Check if the board is completed
    private void CheckBoardCompleted()
    {
        if (currentgameData.selectedLevelData.SearchableWordList.Count == completedWords)
        {
            gameManager.ShowGameWinPopUP(); // Show the game win popup if all words are found
        }
    }

    // Find the first letter of an unfound word for a hint
    public string FindHintWord()
    {
        foreach (var searchingWord in currentgameData.selectedLevelData.SearchableWordList)
        {
            if (searchingWord.found == false)
            {
                return searchingWord.word; // Return the first unfound word
            }
        }
        return string.Empty; // Return an empty string if all words are found
    }
}
