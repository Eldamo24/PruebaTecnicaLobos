using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private float recoveryHealth = 30f;
    private float speedRotation = 30f;
    private AudioSource sfx;
    [SerializeField] private AudioClip pickup;

    private void Start()
    {
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * speedRotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IHealth>().Heal(recoveryHealth);
            GameObject.Find("SpawnPowerUp").GetComponent<SpawnPowerUp>().Spawned = false;
            sfx.resource = pickup;
            sfx.Play();
            Destroy(gameObject);
        }
    }
}
