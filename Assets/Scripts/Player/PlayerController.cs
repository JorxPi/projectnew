using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Aim();

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    private void Aim()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetInitialVelocity(shootPoint.up);
    }
}

