using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using SojaExiles;
using System.Collections;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionPanel;
    public TMP_Text questionText;        
    public TMP_InputField answerInput;  
    public Button submitButton;

    public Transform npcTransform;
    public int coinCount = 5;          
    public float spawnRadius = 2f;     

    public PlayerMovement player;        
    public GameObject goldPrefab;      
    public Transform rewardSpawnPoint;  

    private class Question { public string text; public string answer; }
    private Dictionary<string, Question> questions = new Dictionary<string, Question>();

    private string currentQuestionID;

    void Start()
    {
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

        if (player != null) player.canMove = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

        if (player != null) player.canMove = true;

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
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = npcTransform.position + new Vector3(randomOffset.x, 2f, randomOffset.y);

            Instantiate(goldPrefab, spawnPos, Quaternion.identity);
        }
    }

}
