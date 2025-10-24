using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Glass : MonoBehaviour, IBulletInteractable
{

    [SerializeField] private float postBounceSeparation = 0.01f;

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        Rigidbody2D rb = bullet.Rigidbody;

        Vector2 incoming = -collision.relativeVelocity.normalized;
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(incoming, normal);

        rb.linearVelocity = reflected.normalized * bullet.Speed;

        rb.position += normal * postBounceSeparation;
    }
}
