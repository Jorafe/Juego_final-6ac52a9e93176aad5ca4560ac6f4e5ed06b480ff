using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float bulletLifeTime = 5f;
    private bool canShoot = true;

    private void Update()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.Activate(firePoint.position, firePoint.rotation, bulletLifeTime);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}

