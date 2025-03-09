using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    private float speedMovement = 5f;
    private float damage = 30f;

    private void OnEnable()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += Vector3.up * speedMovement * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
