using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button reloadSceneButton; // Button to reload the scene
    [SerializeField] private Button quitApplicationButton; // Button to quit the application
    [SerializeField] private Transform gameWinpanel; // Reference to the game win panel
    [SerializeField] private LineRenderer linePrefab; // Prefab for the line renderer
    [SerializeField] private LineRenderer lineRenderer; // Reference to the line renderer
    [SerializeField] private Button hintButton; // Button to request a hint

    private Color currentLineColor; // Current color for the line renderer

    void Start()
    {
        // Add listeners to the reload and quit buttons
        if (reloadSceneButton != null)
        {
            reloadSceneButton.onClick.AddListener(ReloadCurrentScene);
        }

        if (quitApplicationButton != null)
        {
            quitApplicationButton.onClick.AddListener(QuitApplication);
        }

        // Add listener for the hint button
        if (hintButton != null)
        {
            hintButton.onClick.AddListener(ShowHint);
        }
    }

    // Reloads the current scene
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quits the application
    private void QuitApplication()
    {
        Application.Quit();
    }

    // Displays the game win popup
    public void ShowGameWinPopUP()
    {
        if (gameWinpanel != null)
        {
            // Animates the scale of the game win panel
            gameWinpanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBounce);
        }
    }

    // Creates a permanent line renderer for the completed word
    public void CreatePermanentLineRenderer(List<Vector3> selectedPositions)
    {
        // Instantiate a new line renderer from the prefab
        LineRenderer newLine = Instantiate(linePrefab, transform);
        newLine.positionCount = selectedPositions.Count;
        newLine.startWidth = 0.5f;
        newLine.endWidth = 0.5f;
        newLine.useWorldSpace = true;
        newLine.startColor = currentLineColor;
        newLine.endColor = currentLineColor;

        // Set positions of the line renderer
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            newLine.SetPosition(i, selectedPositions[i]);
        }

        // Animate the alpha value to 0.5f using DOTween
        Color targetColor = new Color(currentLineColor.r, currentLineColor.g, currentLineColor.b, 0.65f);
        DOTween.To(() => newLine.startColor, x => newLine.startColor = x, targetColor, 0.25f);
        DOTween.To(() => newLine.endColor, x => newLine.endColor = x, targetColor, 0.25f);
    }

    // Updates the line renderer with current selected positions
    public void UpdateLineRenderer(List<Vector3> selectedPositions)
    {
        lineRenderer.positionCount = selectedPositions.Count;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.startColor = currentLineColor;
        lineRenderer.endColor = currentLineColor;

        // Set positions of the line renderer
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, selectedPositions[i]);
        }
    }

    // Clears the line renderer
    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
    }

    // Starts a new selection by picking a random color
    public void StartNewSelection()
    {
        // Pick a random color and store it in the class-level variable
        currentLineColor = new Color(Random.value, Random.value, Random.value, 0.9f);
        ClearLineRenderer();
    }

    // Shows a hint for the current word
    private void ShowHint()
    {
        string hintWord = FindObjectOfType<WordChecker>().FindHintWord(); // Find the first unfound word
        Debug.Log(hintWord); // Output the hint word to the console
    }
}
