using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Se eliminan directivas que no se usan.

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;

    private BulletPool bulletPool; // Referencia al BulletPool

    //Se asigna un valór máximo de salud que es constante.
    private const float MAX_HEALTH = 100f;
    private float health;

    // Variables para el movimiento
    // Relacionado con punto 30 del documento de correcciones.
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    private Vector3 velocity;
    private bool isGrounded;

    private CharacterController characterController;

    void Start()
    {
        // Se iguala la salud a salud máxima para empezar el juego
        health = MAX_HEALTH; 
        bulletPool = FindObjectOfType<BulletPool>();
        characterController = GetComponent<CharacterController>();

        UpdateHealthUI();
    }
    // Se extraen los métodos para cumplir el principio de responsabilidad única
    // y mejorar la legibilidad del código.
    void Update()
    {
        Shooting();
        PlayerAttack();
        PlayerCollisions();
        MovePlayer(); 
    }

    // Función de movimiento del jugador
    // Relacionado con punto 30 del documento de correcciones.
    private void MovePlayer()
    {
        // Comprobar si el jugador está en el suelo.
        isGrounded = characterController.isGrounded;

        // Mantiene al jugador en el suelo.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Obtener las entradas de movimiento horizontal (WASD o flechas)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Crear el vector de movimiento para el player
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Aplicar el movimiento 
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Controlador para el salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplica la gravedad
        velocity.y += gravity * Time.deltaTime;

        // Aplica el movimiento vertical (gravedad)
        characterController.Move(velocity * Time.deltaTime);
    }

    void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                // Se cambia item.tag por Compare.tag porque es más eficiente
                //Relacionado con el punto 25 del documento de correcciones
                if (item.CompareTag("Enemy"))
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }

    void PlayerCollisions()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 3f);
        foreach (var item in targets)
        {
            // Se cambia item.tag por Compare.tag porque es más eficiente
            //Relacionado con el punto 25 del documento de correcciones
            if (item.CompareTag("Heal"))
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.CompareTag("Finish"))
            {
                Win();
            }
            if (item.CompareTag("Enemy"))
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
            // Se cambia la referencia de la rotación del player por la del rifle para que la bala salga correctamente.
            // Punto 12 del documento de correcciones.
            buf.transform.rotation = rifleStart.rotation;
            // Se cambia la rotación del player por la rotación del rifle para la dirección de la bala.
            // Punto 11 del documento de correcciones.
            buf.GetComponent<Bullet>().SetDirection(rifleStart.forward);
            
        }
    }
    public void ChangeHealth(int hp)
    {
        // Se cambia 100 por la referencia de la vida máxima aceptada.
        // Relacionado con el punto 26 del documento de correcciones.
        health += hp;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        else if (health <= 0)
        {
            Lost();
        }
        UpdateHealthUI();
    }

    public void Win()
    {
        victoryScreen.SetActive(true);
        //Destruir PlayerLook puede generar conflictos, es mejor apagarlo.
        //Relacionado con el punto 24 del documento de revisión.
        DisablePlayerControl();
        UnlockCursor();
    }

    public void Lost()
    {
        gameOverScreen.SetActive(true);
        DisablePlayerControl();
        UnlockCursor();
    }

    private void UpdateHealthUI()
    {
        hpText.text = health.ToString();
    }

    private void DisablePlayerControl()
    {
        //Es mejor desactivar el componente player Look en lugar de eliminarlo ya que esto podría
        //generar conflictos más adelante si se llega a necesitar este componente.
        //Relacionado con el punto 24 del documento de revisión.
        var playerLook = GetComponent<PlayerLook>();
        if (playerLook != null)
        {
            playerLook.enabled = false;
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
