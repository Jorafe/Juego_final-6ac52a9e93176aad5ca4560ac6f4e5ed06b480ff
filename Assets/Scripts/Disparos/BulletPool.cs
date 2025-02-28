using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 20;
    private Queue<Bullet> bullets = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bullets.Enqueue(obj.GetComponent<Bullet>());
        }
    }

    public Bullet GetBullet()
    {
        Bullet bullet = bullets.Dequeue();
        bullets.Enqueue(bullet); // Lo reinsertamos para que sea reutilizable
        return bullet;
    }
}
