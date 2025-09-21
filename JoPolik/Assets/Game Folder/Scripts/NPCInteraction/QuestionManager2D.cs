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

    public Player01Movement player;    // Your 2D player movement script
    public GameObject invisibleBarrier; // Assign the barrier GameObject in Inspector

    private class Question { public string text; public string answer; }
    private Dictionary<string, Question> questions = new Dictionary<string, Question>();

    private string currentQuestionID;

    void Start()
    {
        // Example question
        questions.Add("q1", new Question { text = "What data structure is used in the implementation of recursion?", answer = "Stack" });

        submitButton.onClick.AddListener(CheckAnswer);
        questionPanel.SetActive(false);
    }

    public void ShowQuestion(string id)
    {
        if (!questions.ContainsKey(id)) return;

        currentQuestionID = id;
        questionText.text = questions[id].text;
        answerInput.text = "";
        questionPanel.SetActive(true);

        // Disable player movement
        if (player != null) player.canMove = false;

        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Focus input
        answerInput.ActivateInputField();
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();

        if (playerAnswer.Equals(questions[currentQuestionID].answer, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct! Unlocking barrier.");
            if (invisibleBarrier != null)
                invisibleBarrier.SetActive(false); // disables the barrier
        }
        else
        {
            Debug.Log("Incorrect! Barrier stays.");
        }

        questionPanel.SetActive(false);

        // Re-enable player movement
        if (player != null) player.canMove = true;

        // Hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
