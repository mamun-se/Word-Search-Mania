using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button reloadSceneButton;
    [SerializeField] private Button quitApplicationButton;
    [SerializeField] private Transform gameWinpanel;
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private LineRenderer lineRenderer;

    private Color currentLineColor;

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

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    public void ShowGameWinPopUP()
    {
        if (gameWinpanel != null)
        {

            gameWinpanel.DOScale(Vector3.one , 0.5f).SetEase(Ease.InBounce);
        }
    }

    public void CreatePermanentLineRenderer(List<Vector3> selectedPositions)
    {
        LineRenderer newLine = Instantiate(linePrefab, transform);
        newLine.positionCount = selectedPositions.Count;
        newLine.startWidth = 0.5f;
        newLine.endWidth = 0.5f;
        newLine.useWorldSpace = true;
        newLine.startColor = currentLineColor;
        newLine.endColor = currentLineColor;

        for (int i = 0; i < selectedPositions.Count; i++)
        {
            newLine.SetPosition(i, selectedPositions[i]);
        }

        // Animate the alpha value to 0.5f using DOTween
        Color targetColor = new Color(currentLineColor.r, currentLineColor.g, currentLineColor.b, 0.65f);
        DOTween.To(() => newLine.startColor, x => newLine.startColor = x, targetColor, 0.25f);
        DOTween.To(() => newLine.endColor, x => newLine.endColor = x, targetColor, 0.25f);
    }

    public void UpdateLineRenderer(List<Vector3> selectedPositions)
    {
        lineRenderer.positionCount = selectedPositions.Count;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        lineRenderer.startColor = currentLineColor;
        lineRenderer.endColor = currentLineColor;

        for (int i = 0; i < selectedPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, selectedPositions[i]);
        }
    }

    public void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;
    }

    public void StartNewSelection()
    {
        // Pick a random color and store it in the class-level variable
        currentLineColor = new Color(Random.value, Random.value, Random.value, 0.9f);
        ClearLineRenderer();
    }
}
