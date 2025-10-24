using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class Slime : MonoBehaviour, IBulletInteractable
{
    [SerializeField] private float postSeparation = 0.01f;

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        var rb = bullet.Rigidbody;

        Vector2 v = rb.linearVelocity;                 
        Vector2 n = collision.GetContact(0).normal;    
        Vector2 t = new Vector2(-n.y, n.x);            

        Vector2 tangentDir = (Vector2.Dot(v, t) >= 0f) ? t : -t;

        rb.linearVelocity = tangentDir * v.magnitude;

        rb.position += n * postSeparation;
    }
}
