using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGrid : MonoBehaviour
{
    public GameData currentGameData; // Reference to the game data containing the grid information
    public GameObject gridSquirePrefab; // Prefab for the grid square
    public AlphabetData alphabetData; // Data for the alphabet letters
    public float squireOffset; // Offset for positioning squares

    private List<GameObject> squireList = new List<GameObject>(); // List to store created grid squares

    private void Start()
    {
        CreateGrid(); // Create the grid of squares
        SetSquarePositions(); // Set the positions of the squares
    }

    // Public method to get the list of squares
    public List<GameObject> GetSquareList()
    {
        return squireList;
    }

    // Method to create the grid of squares
    private void CreateGrid()
    {
        Vector3 gridScale = Vector3.one; // Default scale for the grid squares
        if (currentGameData != null) // Check if game data is available
        {
            // Iterate through the grid data and create squares
            foreach (var squares in currentGameData.selectedLevelData.level)
            {
                foreach (var squireLetter in squares.row)
                {
                    // Find the corresponding letter data from the alphabet list
                    var letterData = alphabetData.alphabetList.Find(data => data.letter == squireLetter);
                    // Instantiate the grid square prefab and add it to the list
                    squireList.Add(Instantiate(gridSquirePrefab));
                    // Set the sprite for the square
                    squireList[squireList.Count - 1].GetComponent<GridSquire>().SetSprite(letterData);
                    // Set the parent of the square to this transform
                    squireList[squireList.Count - 1].transform.SetParent(this.transform);
                    // Set the initial position of the square to zero
                    squireList[squireList.Count - 1].transform.position = Vector3.zero;
                    // Set the scale of the square
                    squireList[squireList.Count - 1].transform.localScale = gridScale;
                    // Set the index of the square
                    squireList[squireList.Count - 1].GetComponent<GridSquire>().SetIndex(squireList.Count - 1);
                }
            }
        }
    }

    // Method to set the positions of the squares
    private void SetSquarePositions()
    {
        // Get the rect and transform of the first square for calculations
        var squareRect = squireList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squireList[0].GetComponent<Transform>();
        // Calculate the offset for positioning squares
        var offSet = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x * squireOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y * squireOffset) * 0.01f
        };
        // Get the starting position for the first square
        var startPos = GetFirstSquarePosition();
        int columnNumber = 0; // Initialize column counter
        int rowNumber = 0; // Initialize row counter
        // Iterate through the squares and set their positions
        foreach (var square in squireList)
        {
            // Check if the row number exceeds the number of rows and reset if necessary
            if (rowNumber + 1 > currentGameData.selectedLevelData.rows)
            {
                columnNumber++;
                rowNumber = 0;
            }
            // Calculate the position of the square
            var positionX = startPos.x + offSet.x * columnNumber;
            var positionY = startPos.y - offSet.y * rowNumber;
            // Set the position of the square
            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++; // Increment the row counter
        }
    }

    // Method to get the starting position for the first square
    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y); // Initialize start position
        // Get the rect and transform of the first square for calculations
        var squareRect = squireList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squireList[0].GetComponent<Transform>();
        var squareSize = Vector2.zero; // Initialize square size
        // Calculate the size of the square
        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        // Calculate the mid positions to center the grid
        var midWidthPosition = (((currentGameData.selectedLevelData.columns - 1) * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = (((currentGameData.selectedLevelData.rows - 1) * squareSize.y) / 2) * 0.01f;

        // Adjust the start position based on the calculated mid positions
        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midWidthHeight;
        return startPosition; // Return the calculated start position
    }
}
