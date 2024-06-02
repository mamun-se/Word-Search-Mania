using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button reloadSceneButton;
    [SerializeField] private Button quitApplicationButton;
    [SerializeField] private Transform gameWinpanel;
    [SerializeField] private LineRenderer linePrefab; // Prefab for the line renderer
    [SerializeField] private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (reloadSceneButton != null)
        {
            reloadSceneButton.onClick.AddListener(ReloadCurrentScene);
        }

        if (quitApplicationButton != null)
        {
            quitApplicationButton.onClick.AddListener(QuitApplication);
        }
    }

    // Method to reload the current scene
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to quit the application
    private void QuitApplication()
    {
        Application.Quit();
    }

    public void ShowGameWinPopUP()
    {
        if (gameWinpanel != null)
        {
            gameWinpanel.localScale = Vector3.one;
        }
    }

    public void CreatePermanentLineRenderer(List<Vector3> selectedPositions)
    {
        LineRenderer newLine = Instantiate(linePrefab, transform);
        newLine.positionCount = selectedPositions.Count;
        newLine.startWidth = 0.5f; // Set start width to 1
        newLine.endWidth = 0.5f; // Set end width to 1
        newLine.useWorldSpace = true;
        Color randomColor = new Color(Random.value, Random.value, Random.value, 0.75f); // Set alpha to 0.5
        newLine.startColor = randomColor;
        newLine.endColor = randomColor;

        for (int i = 0; i < selectedPositions.Count; i++)
        {
            newLine.SetPosition(i, selectedPositions[i]);
        }
    }

    public void UpdateLineRenderer(List<Vector3> selectedPositions)
    {
        lineRenderer.positionCount = selectedPositions.Count;
        lineRenderer.startWidth = 0.5f; // Set start width to 1
        lineRenderer.endWidth = 0.5f; // Set end width to 1
        Color randomColor = Color.white;
        randomColor.a = 0.75f;
        lineRenderer.startColor = randomColor;
        lineRenderer.endColor = randomColor;
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, selectedPositions[i]);
        }
    }

    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
    }
}
