using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private BulletPool bulletPool; // Referenciamos al BulletPool para poder usarlo
    

    private void Start()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    // Se extraen los m�todos para cumplir el principio de responsabilidad �nica
    // y mejorar la legibilidad del c�digo.
    void FixedUpdate()
    {
        BulletMove();
        BulletCollision();
    }

    void BulletMove()
    {
        transform.position += direction * speed * Time.deltaTime;
        speed += 1f;
    }

    void BulletCollision()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 1);
        foreach (var item in targets)
        {
            // Se cambia item.tag por Compare.tag porque es m�s eficiente
            //Relacionado con el punto 14 del documento de correcciones
            if (item.CompareTag("Enemy"))
            {
                //Se destruye al enemigo al colisionar
                //Relacionado con el punto 16 del documento de correcciones
                Destroy(item.gameObject);
                //Una vez destruye al enemigo, la bala vuelve a la piscina
                //Relacionado con el punto 13 del documento de correcciones
                bulletPool.ReturnBullet(gameObject);
                return;
            }
        }
    }

    //Relacionado con el punto 15 del documento de correcciones
    public void SetDirection(Vector3 dir)
    {
        //Se inicializa la velocidad en Start para que no se acumule la velocidad en la bala
        //Relacionado con el punto 13 del documento de correcciones
        speed = 3f;
        direction = dir;
        //Relacionado con el punto 13 del documento de correcciones
        // Devuelve la bala al pool despu�s de cierto tiempo para que no se acumulen.
        Invoke("ReturnToPool", 3f);
    }

    //Funcii�n que devuelve las balas a la piscina
    //Relacionado con el punto 13 del documento de correcciones
    void ReturnToPool()
    {
        bulletPool.ReturnBullet(gameObject); 
    }

}
