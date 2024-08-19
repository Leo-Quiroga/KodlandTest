using System.Collections.Generic;
using UnityEngine;

//Relacionado con el punto 13 del documento de correcciones
public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab; // Este es el prefab de la bala
    public int poolSize = 10; // Capacidad del pooling

    // Este es el pool
    // Queue es una especie de lista con la política, primero en entrar primero en salir.
    private Queue<GameObject> bulletPool = new Queue<GameObject>(); 

    void Start()
    {
        // Creamos todas las balas y las ponemos en la piscina Queue
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab); // Creamos una bala
            bullet.SetActive(false); // La guardamos en la piscina Queue  y se desactiva
            bulletPool.Enqueue(bullet); // La añadimos a la piscina Queue
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            // Sacamos una bala de la piscina
            //Dequeue saca el primero en haber entrado a la lista Queue
            GameObject bullet = bulletPool.Dequeue(); 
            bullet.SetActive(true); // Activamos la bala para que se pueda usar
            return bullet; // Devolvemos la bala para que se use
        }
        else
        {
            // Si la piscina Queue está vacía, podemos crear una nueva bala para que no se limiten los disparos
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false); // Desactivamos la bala
        // La devolvemos a la piscina Queue
        // Enqueue es la forma de volver a ingresar al final de la fila en la lista Queue
        bulletPool.Enqueue(bullet); 
    }
}
