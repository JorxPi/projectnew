using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float initialSpeed = 10f;

    public float Speed => initialSpeed;
    public Rigidbody2D Rigidbody => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            LevelManager.Instance.WinLevel();
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.TryGetComponent<IBulletInteractable>(out var interactable))
            interactable.OnBulletHit(this, collision);
    }

    public void SetInitialVelocity(Vector2 dir)
    {
        rb.linearVelocity = dir.normalized * initialSpeed;
    }
}

