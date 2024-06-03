using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class WordChecker : MonoBehaviour
{
    public GameData currentgameData;
    private string word;

    private int assignedPoints = 0;
    private int completedWords = 0;
    private Ray rayUp, rayDown;
    private Ray rayLeft, rayRight;
    private Ray rayDiagonalLeftUp, rayDiagonalLeftDown;
    private Ray rayDiagonalRightUp, rayDiagonalRightDown;
    private Ray currentRay = new Ray();

    private Vector3 rayStartPosition;
    private List<int> correctSquareList = new List<int>();
    private List<Vector3> selectedPositions = new List<Vector3>(); // For storing positions of selected squares

    private GameManager gameManager;

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
    }

    private void Start()
    {
        currentgameData.selectedLevelData.ClearData();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void SquareSelected(string letter, Vector3 squarePos, int squareIndex)
    {
        if (assignedPoints == 0)
        {
            gameManager.StartNewSelection();
            rayStartPosition = squarePos;
            correctSquareList.Add(squareIndex);
            selectedPositions.Add(squarePos); // Add position to the list
            word += letter;
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
            correctSquareList.Add(squareIndex);
            selectedPositions.Add(squarePos); // Add position to the list
            currentRay = SelectRay(rayStartPosition, squarePos);
            GameEvents.SelectSquareMethod(squarePos);
            word += letter;
        }
        else
        {
            if (IsPointOnRay(currentRay, squarePos))
            {
                correctSquareList.Add(squareIndex);
                selectedPositions.Add(squarePos); // Add position to the list
                GameEvents.SelectSquareMethod(squarePos);
                word += letter;
            }
        }
        gameManager.UpdateLineRenderer(selectedPositions);
        assignedPoints++;
    }

    public void CheckWord()
    {
        foreach (var searchingWord in currentgameData.selectedLevelData.SearchableWordList)
        {
            if (word == searchingWord.word && searchingWord.found == false)
            {
                searchingWord.found = true;
                GameEvents.CorrectWordMethod(word, correctSquareList);
                // Create a new line renderer to mark the correct word
                gameManager.CreatePermanentLineRenderer(selectedPositions);
                completedWords++;
                word = string.Empty;
                correctSquareList.Clear();
                selectedPositions.Clear(); // Clear selected positions
                CheckBoardCompleted();
                return;
            }
        }
        // If the word is incorrect, clear the line renderer
        ClearSelection();
    }

    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2 direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.1f;
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

    private bool IsPointOnRay(Ray ray, Vector3 point)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
            {
                return true;
            }
        }
        return false;
    }

    private void ClearSelection()
    {
        assignedPoints = 0;
        correctSquareList.Clear();
        selectedPositions.Clear(); // Clear selected positions
        word = string.Empty;
        gameManager.ClearLineRenderer(); // Clear the line renderer
    }

    private void CheckBoardCompleted() 
    {
        if (currentgameData.selectedLevelData.SearchableWordList.Count == completedWords)
        {
            gameManager.ShowGameWinPopUP();   
        }
    }

    public string FindHintWord()
    {
        foreach (var searchingWord in currentgameData.selectedLevelData.SearchableWordList)
        {
            if (searchingWord.found == false)
            {
                
                return searchingWord.word;
            }
        }
        return string.Empty;
    }
}
