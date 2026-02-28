using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    public QuizDatabase database;

    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;

    public GameObject gameOverPopup;

    public int maxLives = 3;
    private int currentLives;

    public GameObject[] heartIcons;

    private QuizDatabase.Question currentQuestion;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }

    public void ShowRandomQuestion()
    {
        questionPanel.SetActive(true);

        int index = Random.Range(0, database.questions.Count);
        currentQuestion = database.questions[index];

        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int buttonIndex = i;

            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                currentQuestion.options[i];

            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        bool isCorrect = selectedIndex == currentQuestion.correctAnswerIndex;

        questionPanel.SetActive(false);

        if (isCorrect)
        {
            GameManager.Instance.AddScore(500);   // +500
        }
        else
        {
            LoseLife();
        }

        if (currentLives > 0)
            GameManager.Instance.CheckGameOverAfterQuiz();
    }

    void LoseLife()
    {
        currentLives--;

        UpdateLifeUI();

        questionPanel.SetActive(false);

        if (currentLives <= 0)
        {
            //questionPanel.SetActive(false);
            gameOverPopup.SetActive(true);
        }
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentLives);
        }
    }
}