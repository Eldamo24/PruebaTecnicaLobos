using TMPro;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn instance;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] enemies;
    private int enemyMaxAmountOnBattle;
    private int currentEnemyOnBattle;
    private int deadEnemies;
    [SerializeField] private int wave;
    [SerializeField] private TMP_Text waveText;

    public int Wave { get => wave; set => wave = value; }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        wave = 1;
        waveText.text = "Oleada: " + wave.ToString();
        deadEnemies = 0;
        enemyMaxAmountOnBattle = wave;
        currentEnemyOnBattle = 0;
        if(deadEnemies != wave)
        {
            GenerateEnemies();
        }
    }



    private void Update()
    {
        if(deadEnemies == wave)
        {
            deadEnemies = 0;
            currentEnemyOnBattle = 0;
            wave++;
            waveText.text = "Oleada: " + wave.ToString();
            enemyMaxAmountOnBattle++;
            GenerateEnemies();
        }
    }

    private void GenerateEnemies()
    {
        while (currentEnemyOnBattle < enemyMaxAmountOnBattle)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity);
            currentEnemyOnBattle++;
        }
    }

    public void SetDeadEnemies()
    {
        deadEnemies++;
        currentEnemyOnBattle--;
    }

}
