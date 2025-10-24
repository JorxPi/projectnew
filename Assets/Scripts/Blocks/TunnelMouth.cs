using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class TunnelMouth : MonoBehaviour, IBulletInteractable
{
    [SerializeField] private Transform exitMouth;
    [SerializeField] private float exitOffset = 0.06f;

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        var rb = bullet.Rigidbody;
        float s = rb.linearVelocity.magnitude;
        Vector2 d = (Vector2)exitMouth.right;

        rb.position = (Vector2)exitMouth.position + d * exitOffset;
        rb.linearVelocity = d * s;

        var bulletCol = collision.collider;      
        var mouthCol = collision.otherCollider; 
        Physics2D.IgnoreCollision(bulletCol, mouthCol, true);
        StartCoroutine(ReenableNextFixed(bulletCol, mouthCol));
    }

    private System.Collections.IEnumerator ReenableNextFixed(Collider2D a, Collider2D b)
    {
        yield return new WaitForFixedUpdate();
        if (a && b) Physics2D.IgnoreCollision(a, b, false);
    }
}

