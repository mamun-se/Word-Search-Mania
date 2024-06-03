using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordList : MonoBehaviour
{
    public GameData currentgameData;
    public GameObject searchingWordPrefab;
    public float offSet = 0.0f;
    public int maxColumn = 5;
    public int maxRows = 4;
    private int columns = 2;
    private int rows = 0;
    private int wordNumber = 0;
    private List<GameObject> words = new List<GameObject>();
    void Start()
    {
        wordNumber = currentgameData.selectedLevelData.SearchableWordList.Count;
        if (wordNumber < columns)
        {
            rows = 1;
        }
        else
        {
            CalCulateColumnAndRow();
        }
        CreateWordObjects();
        SetWordPosition();
    }

    private void CalCulateColumnAndRow()
    {
        do
        {
            columns++;
            rows = wordNumber / columns;
        }while (rows >= maxRows);

        if (columns > maxColumn)
        {
            columns = maxColumn;
            rows = wordNumber / columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        columns++;
        rows = wordNumber / columns;
        if (columns > maxColumn)
        {
            columns = maxColumn;
            rows = wordNumber / columns;
            return false;
        }

        if (wordNumber % columns > 0)
        {
            rows++;
        }
        return true;
    }

    private void CreateWordObjects()
    {
        for (int i = 0; i < wordNumber; i++)
        {
            words.Add(Instantiate((searchingWordPrefab) as GameObject));
            words[i].transform.SetParent(transform);
            words[i].GetComponent<RectTransform>().localScale = new Vector3(1f,1f,0.1f);
            words[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
            words[i].GetComponent<SearchingWord>().SetWord(currentgameData.selectedLevelData.SearchableWordList[i].word);
        }
    }

    private void SetWordPosition()
    {
        var squareRect = words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offSet,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offSet
        };
        int columnNumber = 0;
        int rowNumber = 0;
        rows = 0;
        var startPosition = GetFirstSquarePosition();
        foreach (var word in words)
        {
            if (columnNumber + 1 > columns)
            {
                columnNumber = 0;
                rowNumber++;
            }
            var positionX = startPosition.x + wordOffset.x * columnNumber;
            var positionY = startPosition.y - wordOffset.y * rowNumber;
            word.GetComponent<RectTransform>().transform.localPosition = new Vector2(positionX, positionY);
            columnNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = words[0].GetComponent<RectTransform>();
        var parentRect = GetComponent<RectTransform>();
        var squareSize = new Vector2(0f, 0f);
        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + offSet;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + offSet;

        var shiftBy = (parentRect.rect.width - (squareSize.x * columns)) / 2;
        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = ((parentRect.rect.height - squareSize.y) / 2);
        return startPosition;

    }
}
