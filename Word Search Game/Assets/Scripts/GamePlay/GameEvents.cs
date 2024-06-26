using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void EnableSquireSelection();
    public static event EnableSquireSelection OnEnableSquareSelection;
    public static void EnableSquireSelectionMethod()
    {
        if (OnEnableSquareSelection != null) OnEnableSquareSelection();
    }
    //***********************************************************************

    public delegate void DisableSquireSelection();
    public static event DisableSquireSelection OnDisableSquareSelection;
    public static void DisableSquireSelectionMethod()
    {
        if (OnDisableSquareSelection != null) OnDisableSquareSelection();
    }
    //***********************************************************************

    public delegate void SelectSquare(Vector3 position);
    public static event SelectSquare OnSelectSquare;
    public static void SelectSquareMethod(Vector3 position)
    {
        if (OnSelectSquare != null) OnSelectSquare(position);
    }
    //***********************************************************************

    public delegate void CheckSquare(string letter, Vector3 squarePos, int squareIndex);
    public static event CheckSquare OnCheckSquare;
    public static void CheckSquareMethod(string letter, Vector3 squarePos, int squareIndex)
    {
        if (OnCheckSquare != null) OnCheckSquare(letter, squarePos, squareIndex);
    }
    //***********************************************************************

    public delegate void ClearSelection();
    public static event ClearSelection OnClearSelection;
    public static void ClearSelectionMethod()
    {
        if (OnClearSelection != null) OnClearSelection();
    }
    //***********************************************************************

    public delegate void CorrectWord(string _word, List<int> squareIndexes);
    public static event CorrectWord OnCorrectWord;
    public static void CorrectWordMethod(string _word, List<int> squareIndexes)
    {
        if (OnCorrectWord != null)
        {
            OnCorrectWord(_word, squareIndexes);
        }
    }
}