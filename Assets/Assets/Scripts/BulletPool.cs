using System.Collections.Generic;
using UnityEngine;

//Relacionado con el punto 13 del documento de correcciones
public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab; // Este es el prefab de la bala
    public int poolSize = 10; // Capacidad del pooling

    private Queue<GameObject> bulletPool = new Queue<GameObject>(); // Este es el pool

    void Start()
    {
        // Creamos todas las balas y las ponemos en la piscina
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab); // Creamos una bala
            bullet.SetActive(false); // La guardamos en la piscina y se desactiva
            bulletPool.Enqueue(bullet); // La añadimos a la piscina
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue(); // Sacamos una bala de la piscina
            bullet.SetActive(true); // Activamos la bala para que se pueda usar
            return bullet; // Devolvemos la bala para que se use
        }
        else
        {
            // Si la piscina está vacía, podemos crear una nueva bala
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false); // Desactivamos la bala
        bulletPool.Enqueue(bullet); // La devolvemos a la piscina
    }
}
