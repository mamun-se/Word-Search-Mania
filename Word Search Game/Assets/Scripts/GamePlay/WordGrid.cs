using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquirePrefab;
    public AlphabetData alphabetData;
    public float squireOffset;

    private List<GameObject> squireList = new List<GameObject>();

    private void Start()
    {
        CreateGrid();
        SetSquarePositions();
    }

    public List<GameObject> GetSquareList()
    {
        return squireList;
    }

    private void CreateGrid()
    {
        Vector3 gridScale = Vector3.one;
        if (currentGameData != null)
        {
            foreach (var squares in currentGameData.selectedLevelData.level)
            {
                foreach (var squireLetter in squares.row)
                {
                    var letterData = alphabetData.alphabetList.Find(data => data.letter == squireLetter);
                    squireList.Add(Instantiate(gridSquirePrefab));
                    squireList[squireList.Count - 1].GetComponent<GridSquire>().SetSprite(letterData);
                    squireList[squireList.Count - 1].transform.SetParent(this.transform);
                    squireList[squireList.Count - 1].transform.position = Vector3.zero;
                    squireList[squireList.Count - 1].transform.localScale = gridScale;
                    squireList[squireList.Count - 1].GetComponent<GridSquire>().SetIndex(squireList.Count - 1);
                }
            }
        }
    }

    private void SetSquarePositions()
    {
        var squareRect = squireList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squireList[0].GetComponent<Transform>();
        var offSet = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x * squireOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y * squireOffset) * 0.01f
        };
        var startPos = GetFirstSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;
        foreach (var square in squireList)
        {
            if (rowNumber + 1 > currentGameData.selectedLevelData.rows)
            {
                columnNumber++;
                rowNumber = 0;
            }
            var positionX = startPos.x + offSet.x * columnNumber;
            var positionY = startPos.y - offSet.y * rowNumber;
            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f,transform.position.y);
        var squareRect = squireList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = squireList[0].GetComponent<Transform>();
        var squareSize = Vector2.zero;
        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midWidthPosition = (((currentGameData.selectedLevelData.columns - 1) * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = (((currentGameData.selectedLevelData.rows - 1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midWidthHeight;
        return startPosition;
    }
}
