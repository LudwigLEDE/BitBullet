using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Points (Empty GameObjects)")]
    // One spawn point per team.
    public Transform team1SpawnPoint;
    public Transform team2SpawnPoint;

    [Header("Player Settings")]
    // Assign your player prefab here.
    public GameObject playerPrefab;

    [Header("Round Settings")]
    public float roundTime = 60f; // Optional: round timer in seconds

    private int roundNumber = 0;
    private bool roundActive = false;

    // Offsets to spawn additional players so they don't overlap
    public Vector3 team1Offset = new Vector3(1, 0, 0);
    public Vector3 team2Offset = new Vector3(1, 0, 0);

    void Start()
    {
        StartRound();
    }

    // Start a new round by spawning players and starting the timer.
    void StartRound()
    {
        roundNumber++;
        roundActive = true;
        Debug.Log("Round " + roundNumber + " starting!");
        SpawnPlayers();

        // If you want the round to also end on a timer, uncomment the next line.
        StartCoroutine(RoundTimer());
    }

    // Spawns two players for each team.
    void SpawnPlayers()
    {
        if (team1SpawnPoint == null || team2SpawnPoint == null)
        {
            Debug.LogError("Spawn points not assigned in the GameManager!");
            return;
        }

        // Spawn two players for Team 1
        Instantiate(playerPrefab, team1SpawnPoint.position, team1SpawnPoint.rotation);
        Instantiate(playerPrefab, team1SpawnPoint.position + team1Offset, team1SpawnPoint.rotation);

        // Spawn two players for Team 2
        Instantiate(playerPrefab, team2SpawnPoint.position, team2SpawnPoint.rotation);
        Instantiate(playerPrefab, team2SpawnPoint.position + team2Offset, team2SpawnPoint.rotation);
    }

    // Optional: Ends the round after a timer expires.
    IEnumerator RoundTimer()
    {
        float timer = roundTime;
        while (timer > 0 && roundActive)
        {
            Debug.Log("Round " + roundNumber + " timer: " + timer);
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }

        if (roundActive)
        {
            EndRound();
        }
    }

    // Ends the round (this could be triggered by objectives or team elimination instead)
    void EndRound()
    {
        roundActive = false;
        Debug.Log("Round " + roundNumber + " ended!");
        // Additional end-of-round logic (score calculation, cleanup, etc.) goes here.

        // Example: Restart round after a delay.
        StartCoroutine(RestartRound());
    }

    IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(3f);
        StartRound();
    }
}
