using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using SojaExiles;
using System.Collections;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionPanel;
    public TMP_Text questionText;        // TMP text for question
    public TMP_InputField answerInput;   // TMP input field
    public Button submitButton;

    public Transform npcTransform;
    public int coinCount = 5;           // number of coins to spawn
    public float spawnRadius = 2f;      // how far coins can spread around NPC


    public PlayerMovement player;        // reference to player script
    public GameObject goldPrefab;      // assign your gold prefab in Inspector
    public Transform rewardSpawnPoint;  // optional: where gold should appear

    private class Question { public string text; public string answer; }
    private Dictionary<string, Question> questions = new Dictionary<string, Question>();

    private string currentQuestionID;

    void Start()
    {
        // Example question
        questions.Add("q1", new Question { text = "What is the time complexity of binary search?", answer = "O(log n)" });

        submitButton.onClick.AddListener(CheckAnswer);
        questionPanel.SetActive(false);
    }

    public void ShowQuestion(string id)
    {
        Debug.Log("ShowQuestion called for ID: " + id);
        if (!questions.ContainsKey(id)) return;

        currentQuestionID = id;
        questionText.text = questions[id].text;
        answerInput.text = "";
        questionPanel.SetActive(true);

        // Disable player movement
        if (player != null) player.canMove = false;

        // Enable and unlock cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Focus input field
        answerInput.ActivateInputField();

        Debug.Log("Panel active: " + questionPanel.activeSelf);
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();

        if (playerAnswer.Equals(questions[currentQuestionID].answer, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Correct!");
            RewardPlayer();
        }
        else
        {
            Debug.Log("Incorrect!");
        }

        questionPanel.SetActive(false);

        // Re-enable player movement
        if (player != null) player.canMove = true;

        // Hide and lock cursor again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void RewardPlayer()
    {
        Debug.Log("Player gets rewarded!");
        StartCoroutine(SpawnGoldCoinsAfterDelay(2f));
    }

    IEnumerator SpawnGoldCoinsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < coinCount; i++)
        {
            // Random horizontal offset around NPC
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            // Spawn slightly above NPC to let gravity pull them down
            Vector3 spawnPos = npcTransform.position + new Vector3(randomOffset.x, 2f, randomOffset.y);

            Instantiate(goldPrefab, spawnPos, Quaternion.identity);
        }
    }

}
