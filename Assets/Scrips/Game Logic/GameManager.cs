using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int roundTime = 60; // Seconds per round
    private float currentRoundTime;
    private bool roundActive = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        currentRoundTime = roundTime;
        roundActive = true;
        StartCoroutine(RoundTimer());
    }

    IEnumerator RoundTimer()
    {
        while (currentRoundTime > 0 && roundActive)
        {
            currentRoundTime -= Time.deltaTime;
            yield return null;
        }

        if (roundActive)
            RoundLost(); // If time runs out, the attackers lose
    }

    public void RoundWon(int winningTeam)
    {
        roundActive = false;
        Debug.Log("Team " + winningTeam + " won the round!");
        StartCoroutine(RestartRound());
    }

    public void RoundLost()
    {
        roundActive = false;
        Debug.Log("Defenders won the round!");
        StartCoroutine(RestartRound());
    }

    IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(5); // Wait before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the scene
    }
}
