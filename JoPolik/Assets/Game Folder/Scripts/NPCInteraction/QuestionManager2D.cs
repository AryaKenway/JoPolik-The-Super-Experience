using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using SojaExiles;

public class QuestionManager2D : MonoBehaviour
{
    public GameObject questionPanel;
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public Button submitButton;

    public static bool IsQuestionActive = false;

    public Player01Movement player;
    public GameObject invisibleBarrier;

    private class Question { public string text; public string answer; }
    private List<Question> questions = new List<Question>();

    private Question currentQuestion;
    private System.Random rng = new System.Random();

    void Start()
    {
        questions.Add(new Question { text = "What data structure is used in the implementation of recursion?", answer = "Stack" });
        questions.Add(new Question { text = "What is the time complexity of binary search?", answer = "O(log n)" });
        questions.Add(new Question { text = "Which sorting algorithm is based on divide and conquer?", answer = "Quick Sort" });
        questions.Add(new Question { text = "What data structure uses FIFO principle?", answer = "Queue" });
        questions.Add(new Question { text = "Which algorithm is used to find the shortest path in a graph?", answer = "Dijkstra" });

        submitButton.onClick.AddListener(CheckAnswer);
        questionPanel.SetActive(false);
    }

    public void ShowQuestion()
    {
        if (questions.Count == 0) return;

        int index = rng.Next(questions.Count);
        currentQuestion = questions[index];

        questionText.text = currentQuestion.text;
        answerInput.text = "";
        questionPanel.SetActive(true);
        IsQuestionActive = true;

        if (player != null) player.canMove = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        answerInput.ActivateInputField();
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();

        if (playerAnswer.Equals(currentQuestion.answer, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct! Unlocking barrier.");
            if (invisibleBarrier != null)
                invisibleBarrier.SetActive(false);
        }
        else
        {
            Debug.Log("Incorrect! Barrier stays.");
        }

        questionPanel.SetActive(false);
        IsQuestionActive = false;

        if (player != null) player.canMove = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
