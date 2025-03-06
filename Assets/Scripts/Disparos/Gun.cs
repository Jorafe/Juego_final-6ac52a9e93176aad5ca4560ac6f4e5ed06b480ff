using UnityEngine;
using System.Collections;
using TMPro; // Asegúrate de importar TextMeshPro

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float bulletLifeTime = 5f; // Tiempo de vida configurable desde el inspector
    private bool canShoot = true;
    private Camera mainCamera;
    private int bulletsFired = 0;
    private int maxBullets = 25; // Nueva variable para limitar los disparos

    // Usamos TMP_Text en lugar de Text
    public TMP_Text ammoText; // Referencia al texto en UI (TMP_Text de Unity)
    public TMP_Text wormsText; // Referencia al texto en UI para los gusanos
    public Worm[] allWorms; // Referencia a todos los gusanos en la escena
    private int wormsDeactivated = 0; // Contador de gusanos desactivados
    private int maxWorms = 20; // Máximo de gusanos que pueden ser desactivados

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateAmmoUI(); // Actualizar UI al inicio
        UpdateWormsUI(); // Actualizar UI de gusanos al inicio
        allWorms = FindObjectsOfType<Worm>(); // Encuentra todos los gusanos en la escena
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && canShoot && bulletsFired < maxBullets)
        {
            StartCoroutine(Shoot());
        }

        if (bulletsFired >= maxBullets && canShoot)
        {
            ReactivateWorms(); // Reactivar gusanos si el jugador ya no puede disparar más
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        Vector3 targetPoint = GetAimPoint();

        // Obtener bala del pool
        Bullet bullet = BulletPool.Instance.GetBullet();
        if (bullet == null)
        {
            canShoot = true; // Permitir volver a disparar si hay balas en el pool
            yield break;
        }

        bullet.Activate(firePoint.position, firePoint.rotation, bulletLifeTime);

        // Calcular dirección y aplicar velocidad
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bullet.speed;
        }

        bulletsFired++; // Contar la bala disparada
        UpdateAmmoUI(); // Actualizar la UI de balas restantes

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private Vector3 GetAimPoint()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return ray.GetPoint(100);
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            int bulletsRemaining = maxBullets - bulletsFired;
            ammoText.text = bulletsRemaining + "/" + maxBullets; // Mostrar balas restantes
        }
    }

    private void UpdateWormsUI()
    {
        if (wormsText != null)
        {
            wormsText.text = wormsDeactivated + "/" + maxWorms; // Mostrar gusanos desactivados
        }
    }

    // Función para reactivar todos los gusanos
    private void ReactivateWorms()
    {
        foreach (var worm in allWorms)
        {
            worm.gameObject.SetActive(true); // Reactivar cada gusano
        }
    }

    // Función para aumentar el contador de gusanos desactivados
    public void DeactivateWorm()
    {
        wormsDeactivated++; // Aumentar el contador de gusanos desactivados
        UpdateWormsUI(); // Actualizar el UI de gusanos
    }
}