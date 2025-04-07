using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    private List<Transform> _spawnPoints = new List<Transform>();
    public static SpawnManager instance;
    private void Start()
    {
        SetupAllSpawns();
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetupAllSpawns()
    {
        _spawnPoints.Clear();

        foreach (Transform child in transform)
        {
            if (child == transform) continue;

            _spawnPoints.Add(child);

            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                if (childRenderer.material != null)
                {
                    Color materialColor = childRenderer.material.color;
                    materialColor.a = 0f;
                    childRenderer.material.color = materialColor;
                }
                else
                {
                    Debug.LogWarning($"Spawn point '{child.name}' has a Renderer but no Material assigned.", child);
                }
            }
        }
        Debug.Log($"SpawnManager setup complete. Found {_spawnPoints.Count} spawn points under '{gameObject.name}'.");
    }

    public Transform GetFurthest(Vector3 position)
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("Spawn points list is not initialized or empty. Call SetupAllSpawns() first (or ensure Start has run).", this);
            return null;
        }

        Transform furthestSpawn = null;
        float maxDistanceSquared = -1f;

        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (spawnPoint == null) continue;

            float distanceSquared = (spawnPoint.position - position).sqrMagnitude;

            if (distanceSquared > maxDistanceSquared)
            {
                maxDistanceSquared = distanceSquared;
                furthestSpawn = spawnPoint;
            }
        }
        return furthestSpawn;
    }

    public Transform GetFurthestFromList(List<GameObject> gameObjects)
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("Spawn points list is not initialized or empty. Call SetupAllSpawns() first (or ensure Start has run).", this);
            return null;
        }
        if (gameObjects == null || gameObjects.Count == 0)
        {
            Debug.LogError("Input gameObjects list is null or empty.", this);
            return _spawnPoints.Count > 0 ? _spawnPoints[0] : null;
        }

        Transform furthestOverallSpawn = null;
        float maxMinDistanceSquared = -1f;

        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (spawnPoint == null) continue;

            float minDistanceToAnyGameObjectSquared = float.MaxValue;

            foreach (GameObject go in gameObjects)
            {
                if (go == null) continue;

                float distanceSquared = (spawnPoint.position - go.transform.position).sqrMagnitude;
                if (distanceSquared < minDistanceToAnyGameObjectSquared)
                {
                    minDistanceToAnyGameObjectSquared = distanceSquared;
                }
            }

            if (minDistanceToAnyGameObjectSquared > maxMinDistanceSquared)
            {
                maxMinDistanceSquared = minDistanceToAnyGameObjectSquared;
                furthestOverallSpawn = spawnPoint;
            }
        }
        return furthestOverallSpawn;
    }

    public Transform GetRandomSpawn()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("Spawn points list is not initialized or empty. Call SetupAllSpawns() first (or ensure Start has run).", this);
            return null;
        }

        int randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomIndex];
    }
}
