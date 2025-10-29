using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class Slime : MonoBehaviour, IBulletInteractable
{
    [SerializeField] private float postSeparation = 0.01f; 

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        var rb = bullet.Rigidbody;
        Vector2 v = rb.linearVelocity;
        if (v.sqrMagnitude <= 1e-8f) return;

        Vector2 n = collision.GetContact(0).normal;

        if (Mathf.Abs(n.x) >= Mathf.Abs(n.y))
            n = new Vector2(Mathf.Sign(n.x), 0f);   
        else
            n = new Vector2(0f, Mathf.Sign(n.y));   

        Vector2 t;
        if (n.x != 0f)
        {
            t = Vector2.up * Mathf.Sign(v.y == 0f ? 1f : v.y);
        }
        else
        {
            t = Vector2.right * Mathf.Sign(v.x == 0f ? 1f : v.x);
        }

        rb.linearVelocity = t * bullet.Speed;

        rb.position += n * postSeparation;
    }
}
