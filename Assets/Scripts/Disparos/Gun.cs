using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 3f;
    public float bulletLifeTime = 5f;
    public int maxBullets = 20;
    private int bulletsLeft;
    public LevelManager levelManager;
    private int currentPhase = 0;

    private bool canShoot = true;
    private Camera mainCamera;
    private bool allWormsDeactivated = false;

    public ParticleSystem fireEffect;

    public AudioClip shootSound; // Clip de sonido de disparo
    private AudioSource audioSource;

    private void Start()
    {
        mainCamera = Camera.main;
        bulletsLeft = maxBullets;

        if (fireEffect != null)
            fireEffect.Stop();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogWarning("No se encontró AudioSource en el objeto con el script Gun.");
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && canShoot && bulletsLeft > 0)
        {
            StartCoroutine(Shoot());
        }
        else if (bulletsLeft == 0 || allWormsDeactivated)
        {
            if (currentPhase < 2)
            {
                currentPhase++;
                bulletsLeft = maxBullets;
                allWormsDeactivated = false;
            }
        }
    }

    private IEnumerator Shoot()
    {
        Vector3 ScreenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;
        Vector3 TargetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(100);

        canShoot = false;

        if (fireEffect != null)
        {
            fireEffect.transform.position = firePoint.position;
            fireEffect.Play();
        }

        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.Activate(firePoint.position, firePoint.rotation, bulletLifeTime);
        bullet.GetComponent<Rigidbody>().velocity = (TargetPoint - firePoint.position).normalized * bullet.speed;

        bulletsLeft--;
        LevelManager.Instance.RegisterShot();

        yield return new WaitForSeconds(fireRate);

        if (fireEffect != null && fireEffect.isPlaying)
            fireEffect.Stop();

        canShoot = true;
    }
}