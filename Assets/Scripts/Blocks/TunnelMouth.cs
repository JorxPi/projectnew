using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class TunnelMouth : MonoBehaviour, IBulletInteractable
{
    [SerializeField] private Transform exitMouth;            
    [SerializeField] private Collider2D exitMouthCollider;   
    [SerializeField] private Collider2D[] bodies;            
    [SerializeField] private float extraExitOffset = 0.02f;  

    private void Awake()
    {
        if (!exitMouthCollider && exitMouth)
            exitMouthCollider = exitMouth.GetComponent<Collider2D>();
    }

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        var rb = bullet.Rigidbody;

        Vector2 outDir = ((Vector2)exitMouth.right).normalized;
        float speed = bullet.Speed;

        float r = EstimateBulletRadius(collision.collider, outDir);
        rb.position = (Vector2)exitMouth.position + outDir * (r + extraExitOffset);
        rb.linearVelocity = outDir * speed;

        var bulletCol = collision.collider;
        var enteredMouthCol = collision.otherCollider;

        if (enteredMouthCol) Physics2D.IgnoreCollision(bulletCol, enteredMouthCol, true);
        if (exitMouthCollider) Physics2D.IgnoreCollision(bulletCol, exitMouthCollider, true);
        if (bodies != null)
            for (int i = 0; i < bodies.Length; i++)
                if (bodies[i]) Physics2D.IgnoreCollision(bulletCol, bodies[i], true);

        StartCoroutine(ReenableNextFixed(bulletCol, enteredMouthCol, exitMouthCollider, bodies));
    }

    private static float EstimateBulletRadius(Collider2D bulletCol, Vector2 dir)
    {
        if (bulletCol is CircleCollider2D c)
            return c.radius * Mathf.Max(bulletCol.transform.lossyScale.x, bulletCol.transform.lossyScale.y);

        Vector2 e = bulletCol.bounds.extents;
        return Mathf.Abs(e.x * dir.x) + Mathf.Abs(e.y * dir.y);
    }

    private System.Collections.IEnumerator ReenableNextFixed(
        Collider2D bulletCol, Collider2D entered, Collider2D exitCol, Collider2D[] wallCols)
    {
        yield return new WaitForFixedUpdate();
        if (bulletCol && entered) Physics2D.IgnoreCollision(bulletCol, entered, false);
        if (bulletCol && exitCol) Physics2D.IgnoreCollision(bulletCol, exitCol, false);
        if (bulletCol && wallCols != null)
            foreach (var c in wallCols)
                if (bulletCol && c) Physics2D.IgnoreCollision(bulletCol, c, false);
    }
}
