using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GridManager gridManager;
    public GridRenderer gridRenderer;

    //public BlockTrayManager trayManager;

    public GameObject lineClearPopup;
    public GameObject gameOverPopup;

    public int currentScore = 0;

    public TextMeshProUGUI scoreText;

    private List<int> pendingRows = new List<int>();
    private List<int> pendingColumns = new List<int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString();
    }

    public void OnBlockPlaced()
    {
        pendingRows = gridManager.GetCompletedRows();
        pendingColumns = gridManager.GetCompletedColumns();

        if (pendingRows.Count > 0 || pendingColumns.Count > 0)
        {
            lineClearPopup.SetActive(true);
        }
        else
        {
            CheckGameOver();
        }
    }

    //public void ConfirmLineClear()
    //{
    //    foreach (int row in pendingRows)
    //    {
    //        gridManager.ClearRow(row);
    //        gridRenderer.ClearRow(row);
    //    }

    //    foreach (int col in pendingColumns)
    //    {
    //        gridManager.ClearColumn(col);
    //        gridRenderer.ClearColumn(col);
    //    }

    //    lineClearPopup.SetActive(false);
    //    QuizManager.Instance.ShowRandomQuestion();
    //    //CheckGameOver();
    //}
    public void ConfirmLineClear()
    {
        int totalLinesCleared = 0;

        foreach (int row in pendingRows)
        {
            gridManager.ClearRow(row);
            gridRenderer.ClearRow(row);
            totalLinesCleared++;
        }

        foreach (int col in pendingColumns)
        {
            gridManager.ClearColumn(col);
            gridRenderer.ClearColumn(col);
            totalLinesCleared++;
        }

        if (totalLinesCleared > 0)
            AddScore(200 * totalLinesCleared);

        lineClearPopup.SetActive(false);

        QuizManager.Instance.ShowRandomQuestion();
    }

    void CheckGameOver()
    {
        var tray = Object.FindObjectOfType<BlockTrayManager>();

        foreach (var block in tray.GetActiveBlocks())
        {
            for (int x = 0; x < gridManager.width; x++)
            {
                for (int y = 0; y < gridManager.height; y++)
                {
                    if (PlacementValidator.CanPlace(block.blockData,
                                                    new Vector2Int(x, y),
                                                    gridManager))
                        return;
                }
            }
        }

        gameOverPopup.SetActive(true);
    }

    public void CheckGameOverAfterQuiz()
    {
        CheckGameOver();
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}