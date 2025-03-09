using UnityEngine;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float bulletLifeTime = 5f;
    public int maxBullets = 25;
    private int bulletsLeft;
    public TMP_Text ammoText;
    public int totalWormCount = 0;
    public TMP_Text wormText;
    public LevelManager levelManager;
    private int currentPhase = 0;

    private bool canShoot = true;
    private Camera mainCamera;
    private bool allWormsDeactivated = false;

    //int totalWorms = LevelManager.Instance.GetTotalWorms();

    private void Start()
    {
        mainCamera = Camera.main;
        bulletsLeft = maxBullets;
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && canShoot && bulletsLeft > 0)
        {
            StartCoroutine(Shoot());
        }
        else if (bulletsLeft == 0 || allWormsDeactivated) // Cambio de fase si se acaban las balas o si se destruyen todos los gusanos
        {
            if (currentPhase < 2)
            {
                currentPhase++;
                levelManager.NextPhase();
                bulletsLeft = maxBullets;
                UpdateUI();
                allWormsDeactivated = false; // Reiniciamos el estado de gusanos desactivados
            }
            else
            {
                Debug.Log("¡Juego terminado!");
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
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.SetGunReference(this);
        bullet.Activate(firePoint.position, firePoint.rotation, bulletLifeTime);
        bullet.GetComponent<Rigidbody>().velocity = (TargetPoint - firePoint.position).normalized * bullet.speed;

        bulletsLeft--;
        UpdateUI();

        LevelManager.Instance.RegisterShot();

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void IncrementWormCounter()
    {
        totalWormCount++;
        UpdateUI();
        if (totalWormCount == levelManager.GetTotalWorms()) // Verificamos si se han destruido todos los gusanos
        {
            allWormsDeactivated = true;
        }
    }

    private void UpdateUI()
    {
        //ammoText.text = bulletsLeft + "/25";
       
    }
}