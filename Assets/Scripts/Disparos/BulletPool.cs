using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 25; // Cambiado a 25 balas
    private Queue<Bullet> bullets = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddBulletToPool();
        }
    }

    private void AddBulletToPool()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.SetActive(false);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullets.Enqueue(bullet);
    }

    public Bullet GetBullet()
    {
        // Si no hay balas disponibles, devolvemos null
        if (bullets.Count == 0)
        {
            return null;
        }

        Bullet bullet = bullets.Dequeue();
        bullets.Enqueue(bullet); // Lo reinsertamos para que sea reutilizable

        bullet.gameObject.SetActive(false); // Asegurar que esté desactivada antes de usarla
        return bullet;
    }
}