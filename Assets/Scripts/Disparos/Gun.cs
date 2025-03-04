using UnityEngine;
using System.Collections;
using Cinemachine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float bulletLifeTime = 5f; // Tiempo de vida configurable desde el inspector
    private bool canShoot = true;

    //public CinemachinefreeLook  FreeLook;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        Vector3 ScreenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;
        Vector3 TargetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            TargetPoint = hit.point;
        }
        else
        {
            TargetPoint = ray.GetPoint(100);
        }
        
        canShoot = false;
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.Activate(firePoint.position, firePoint.rotation, bulletLifeTime);
        Vector3 direction = (TargetPoint - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction * bullet.speed;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}