using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class GridSquire : MonoBehaviour
{
    public int squireIndex { get; set; }
    private AlphabetData.LetterData letterData;
    private SpriteRenderer displayImage;
    private bool selected;
    private bool clicked;
    private int index = -1;
    private bool isCorrect;
    private List<int> selectedSquares = new List<int>(); // Use instance variable for selectedSquares

    private WordChecker wordChecker;

    void Start()
    {
        displayImage = GetComponent<SpriteRenderer>();
        selected = false;
        clicked = false;
        isCorrect = false;
        wordChecker = FindObjectOfType<WordChecker>(); // Find the WordChecker instance in the scene
    }

    public void SetSprite(AlphabetData.LetterData _letterData)
    {
        letterData = _letterData;
        GetComponent<SpriteRenderer>().sprite = letterData.letterSprite;
    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += OnSelectSquare;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= OnSelectSquare;
    }

    public void OnEnableSquareSelection()
    {
        clicked = true;
        selected = false;
        selectedSquares.Clear();
    }

    public void OnDisableSquareSelection()
    {
        selected = false;
        clicked = false;
        if (isCorrect)
        {
            // Debug.Log("Correct letter data : " + letterData.letter);
        }
        else
        {
            // Debug.Log("Wrong letter data : " + letterData.letter);
        }
    }

    public void OnSelectSquare(Vector3 position)
    {
        if (transform.position == position)
        {
            // Debug.Log("Selected a letter");
        }
    }

    private void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvents.EnableSquireSelectionMethod();
        CheckSquare();
    }

    private void OnMouseEnter()
    {
        if (clicked)
        {
            CheckSquare();
        }
    }

    private void OnMouseUp()
    {
        wordChecker.CheckWord();
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquireSelectionMethod();
    }

    public void CheckSquare()
    {
        if (!selected && clicked)
        {
            selected = true;
            selectedSquares.Add(index);
            GameEvents.CheckSquareMethod(letterData.letter, transform.position, index);
        }
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }

    public int GetIndex() { return index; }
}
