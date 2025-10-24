using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class Stone : MonoBehaviour, IBulletInteractable
{
    public void OnBulletHit(Bullet bullet, Collision2D collision)
    {
        Destroy(bullet.gameObject);
    }
}
