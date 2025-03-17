using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    [Header("Spawn Points")]
    // Set up two spawn points per team in the Inspector.
    public Transform[] team1SpawnPoints;
    public Transform[] team2SpawnPoints;

    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Round Settings")]
    // Optional: use a timer if desired
    public float roundTime = 60f;

    private int roundNumber = 0;
    private int team1PlayerCount = 0;
    private int team2PlayerCount = 0;
    private bool roundActive = false;

    private void Start()
    {
        StartRound();
    }

    // Starts a new round: spawns players and resets state.
    public void StartRound()
    {
        roundNumber++;
        Debug.Log("Round " + roundNumber + " starting!");
        SpawnPlayers();
        roundActive = true;

        // Uncomment to use a timer as well:
        //StartCoroutine(RoundCountdown());
    }

    // Spawns players for each team at their designated spawn points.
    // Also initializes the player counts.
    void SpawnPlayers()
    {
        // Assume the number of spawn points equals the number of players per team.
        team1PlayerCount = team1SpawnPoints.Length;
        team2PlayerCount = team2SpawnPoints.Length;

        // Spawn Team 1 players
        for (int i = 0; i < team1SpawnPoints.Length; i++)
        {
            GameObject player = Instantiate(playerPrefab, team1SpawnPoints[i].position, team1SpawnPoints[i].rotation);
            // Example: assign team number and reference to this RoundManager
            // var playerScript = player.GetComponent<Player>();
            // playerScript.teamNumber = 1;
            // playerScript.roundManager = this;
        }

        // Spawn Team 2 players
        for (int i = 0; i < team2SpawnPoints.Length; i++)
        {
            GameObject player = Instantiate(playerPrefab, team2SpawnPoints[i].position, team2SpawnPoints[i].rotation);
            // Example: assign team number and reference to this RoundManager
            // var playerScript = player.GetComponent<Player>();
            // playerScript.teamNumber = 2;
            // playerScript.roundManager = this;
        }
    }

    // This method should be called by a player's death logic.
    // It reduces the count for the specified team and checks if the round should end.
    public void PlayerDied(int teamNumber)
    {
        if (!roundActive) return;

        if (teamNumber == 1)
        {
            team1PlayerCount--;
            Debug.Log("Team 1 player lost. Remaining: " + team1PlayerCount);
            if (team1PlayerCount <= 0)
            {
                Debug.Log("Team 1 has no players left!");
                EndRound(2); // Team 2 wins.
            }
        }
        else if (teamNumber == 2)
        {
            team2PlayerCount--;
            Debug.Log("Team 2 player lost. Remaining: " + team2PlayerCount);
            if (team2PlayerCount <= 0)
            {
                Debug.Log("Team 2 has no players left!");
                EndRound(1); // Team 1 wins.
            }
        }
    }

    // Ends the round, declaring the winning team.
    // A winningTeam value of 0 can represent a draw if needed.
    public void EndRound(int winningTeam)
    {
        if (!roundActive) return;
        roundActive = false;
        if (winningTeam == 0)
        {
            Debug.Log("Round " + roundNumber + " ended in a draw!");
        }
        else
        {
            Debug.Log("Round " + roundNumber + " ended! Team " + winningTeam + " wins!");
        }
        // Additional logic: update scores, display results, etc.
        StartCoroutine(RestartRound());
    }

    // Call this method when the objective condition is met.
    // objectiveWon: true if the objective was achieved; false if failed.
    // winningTeam: which team is declared the winner based on the objective.
    public void ObjectiveReached(bool objectiveWon, int winningTeam)
    {
        if (!roundActive) return;
        roundActive = false;
        if (objectiveWon)
        {
            Debug.Log("Objective completed! Team " + winningTeam + " wins the round!");
        }
        else
        {
            Debug.Log("Objective failed! Team " + winningTeam + " wins the round!");
        }
        StartCoroutine(RestartRound());
    }

    // Optional: use this coroutine if you want a round timer.
    IEnumerator RoundCountdown()
    {
        float timeRemaining = roundTime;
        while (timeRemaining > 0 && roundActive)
        {
            Debug.Log("Time remaining: " + timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
        if (roundActive)
        {
            Debug.Log("Time ended!");
            // Decide the outcome (or declare a draw) when time runs out.
            EndRound(0);
        }
    }

    // Optionally wait a few seconds before starting a new round.
    IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(3f);
        // Add any cleanup logic here (e.g., remove dead players, reset objectives)
        StartRound();
    }
}
