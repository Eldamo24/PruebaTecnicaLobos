using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUps;
    private bool spawned;

    public bool Spawned { get => spawned; set => spawned = value; }

    private void Start()
    {
        spawned = false;
        InvokeRepeating("SpawnAnObject", 15f, 25f);
    }

    void SpawnAnObject()
    {
        if (!spawned)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-5, 5), Random.Range(-2, 3));
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], spawnPos, Quaternion.identity);
            spawned = true;
        }
    }
}
