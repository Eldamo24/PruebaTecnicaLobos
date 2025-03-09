using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HighScoreTable;

public class PlayerController : MonoBehaviour, IHealth
{
    [SerializeField] private string namePlayer;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hit;

    [Header("References")]
    private InputReader reader;
    private Rigidbody2D rb;

    [Header("Movement variables")]
    [SerializeField] private float speedMovement;

    [Header("Shooting variables")]
    private float cooldown = .3f;
    private float timeToWait = 0f;
    [SerializeField] private GameObject shoot;
    [SerializeField] private Transform shootPosition;

    [Header("LifeBar")]
    [SerializeField] private Image lifeImage;

    private float maxHealth;
    private float health;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float Health { get => health; set => health = value; }
    public string NamePlayer { get => namePlayer; set => namePlayer = value; }

    private void Awake()
    {
        Time.timeScale = 0f;
        MaxHealth = 100f;
        Health = MaxHealth;
        reader = GetComponent<InputReader>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (reader.IsShooting)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 movement = rb.position + reader.MovementValue * speedMovement * Time.fixedDeltaTime;
        movement.x = Mathf.Clamp(movement.x, -8, 8);
        movement.y = Mathf.Clamp(movement.y, -3.4f, 5);
        rb.MovePosition(movement);
    }

    private void Shoot()
    {
        if (Time.time > timeToWait)
        {
            Instantiate(shoot, shootPosition.position, Quaternion.identity);
            playerAudioSource.resource = shootSound;
            playerAudioSource.Play();
            timeToWait = Time.time + cooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        sfx.resource = hit;
        sfx.Play();
        lifeImage.fillAmount -= damage / 100;
        if(Health <= 0)
        {
            Health = 0;
            lifeImage.fillAmount = 0;
            Die();
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        lifeImage.fillAmount += amount / 100;
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
            lifeImage.fillAmount = 1;
        }
    }

    public void Die()
    {
        AddHighscoreEntry(EnemySpawn.instance.Wave, namePlayer);
        SceneManager.LoadScene("Menu");
    }

    public void AddHighscoreEntry(int score, string name)
    {
        HighScoreEntry highscoreEntry = new HighScoreEntry() { score = score, name = name };
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList.Add(highscoreEntry);
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
}
