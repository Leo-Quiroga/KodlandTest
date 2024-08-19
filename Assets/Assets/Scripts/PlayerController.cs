using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    public BulletPool bulletPool; // Referencia al BulletPool

    //Se asigna un valór máximo de salud que es constante
    private const float MaxHealth = 100f;
    public float health;

    void Start()
    {
        // Se iguala la salud a salud máxima para empezar el juego
        health = MaxHealth; 
        ChangeHealth(0); //Inicia en 0 lo que llama al método Lost() al inicio del juego.
        bulletPool = FindObjectOfType<BulletPool>();
    }

    void Update()
    {
        Shooting();
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.tag == "Enemy")
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.tag == "Heal")
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.tag == "Finish")
            {
                Win();
            }
            if (item.tag == "Enemy")
            {
                Lost();
            }
        }
    }

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //No se instancia sino que se usa una bala de la piscina.
            GameObject buf = bulletPool.GetBullet();
            buf.transform.position = rifleStart.position;
            // Se cambia la rotación del player por la rotación del rifle para la dirección de la bala.
            // Punto 11 del documento de correcciones.
            buf.GetComponent<Bullet>().SetDirection(rifleStart.forward);
            // Se cambia la referencia de la rotación del player por la del rifle para que la bala salga correctamente.
            // Punto 12 del documento de correcciones.
            buf.transform.rotation = rifleStart.rotation;
        }
    }
    public void ChangeHealth(int hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        Victory.SetActive(true);
        Destroy(GetComponent<PlayerLook>());//Destruir PlayerLook puede generar conflictos, es mejor apagarlo.
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }
}
