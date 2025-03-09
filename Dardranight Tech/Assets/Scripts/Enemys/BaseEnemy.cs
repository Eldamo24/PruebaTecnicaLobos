using System.Collections;
using UnityEngine;


public class BaseEnemy : MonoBehaviour, IEnemy, IHealth
{
    [Header("Movement")]
    [SerializeField] private Transform playerPosition;
    private float speedMovement = 3f;
    private float stopDistance = 5f;
    private float offset = 90f;
    private Rigidbody2D rb;
    private Vector2 moveDir;

    [Header("Shoot")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] public Transform firePoint;
    public float projectileSpeed = 3f;
    public float shootDistance = 5f;
    public float fireRate = 1f; 
    public int shotsPerWave = 5;
    public float shotDelay = 0.2f;
    private bool canShoot = true;

    [Header("Dead Effect")]
    [SerializeField] private GameObject explosion;

    [Header("Sounds")]
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip explosionSound;

    private float maxHealth;
    private float health;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float Health { get => health; set => health = value; }

    private void Start()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        MaxHealth = 100f;
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        playerPosition = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Movement();
        LookAtPlayer();
        Attack();
    }

    public void Movement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);

        if (distanceToPlayer > stopDistance)
        {
            moveDir = (playerPosition.position - transform.position).normalized;
            rb.linearVelocity = moveDir * speedMovement;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; 
        }
    }

    private void LookAtPlayer()
    {
        Vector2 direction = (playerPosition.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Attack()
    {
        if (Vector2.Distance(transform.position, playerPosition.position) <= shootDistance && canShoot)
        {
            StartCoroutine(ShootWave());
        }
    }

    private IEnumerator ShootWave()
    {
        canShoot = false;

        for (int i = 0; i < shotsPerWave; i++)
        {
            Shoot();
            yield return new WaitForSeconds(shotDelay);
        }

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

        Vector2 direction = (playerPosition.position - firePoint.position).normalized;
        rbProjectile.linearVelocity = direction * projectileSpeed;
        enemyAudioSource.resource = shootSound;
        enemyAudioSource.Play();
    }

    public void Die()
    {
        EnemySpawn.instance.SetDeadEnemies();
        Instantiate(explosion, transform.position, Quaternion.identity);
        sfx.resource = explosionSound;
        sfx.Play();
        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        sfx.resource = hit;
        sfx.Play();
        if(Health <= 0)
        {
            Health = 0;
            Die();
        }
    }
}
