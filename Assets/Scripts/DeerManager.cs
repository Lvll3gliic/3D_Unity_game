using UnityEngine;
using TMPro;

public class DeerManager : MonoBehaviour
{
    private int deadDeers = 0;
    private int maxDeerCount = 0;
    public TextMeshProUGUI goal;
    public GameOver gameOver; 

    // Reference to the DeerSpawner script
    public DeerSpawner deerSpawner;

    private void Start()
    {
        deadDeers = 0; // Initialize the count to 0
        maxDeerCount = deerSpawner.maxDeerCount; // Retrieve the max deer count from DeerSpawner
        UpdateGoalText();
    }

    public void IncrementDeadDeers()
    {
        deadDeers++;
        UpdateGoalText();

        if (deadDeers >= maxDeerCount)
        {
            GameOver();
        }
    }

    private void UpdateGoalText()
    {
        goal.text = "Goal: " + deadDeers.ToString() + "/" + maxDeerCount.ToString();
    }

    private void GameOver()
    {
        // Perform game over actions here, such as displaying a game over screen or stopping gameplay
        Debug.Log("Game Over");
        gameOver.OpenGameOver(); 
    }
}
