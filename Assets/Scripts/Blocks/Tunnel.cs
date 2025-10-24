using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class Tunnel : MonoBehaviour, IBulletInteractable
{
    [SerializeField] private Transform sideA;      
    [SerializeField] private Transform sideB;      
    [SerializeField] private float exitOffset = 0.06f; 

    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        var rb = bullet.Rigidbody;

        Vector2 n = collision.GetContact(0).normal;

        Vector2 aN = sideA.right.normalized;
        Vector2 bN = sideB.right.normalized;

        bool enteredA = Vector2.Dot(n, aN) >= Vector2.Dot(n, bN);

        Transform exitSide = enteredA ? sideB : sideA;
        Vector2 exitNormal = (enteredA ? bN : aN);

        float speed = rb.linearVelocity.magnitude;

        rb.position = (Vector2)exitSide.position + exitNormal * exitOffset;
        rb.linearVelocity = exitNormal * speed;
    }
}
