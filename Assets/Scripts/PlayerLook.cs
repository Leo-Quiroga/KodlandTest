using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Ajusto sensibilidad de mouse para que no sea 0.
    // Punto 7 del documento de correcciones.
    [SerializeField] float mouseSense = 150f; 
    [SerializeField] private Transform player, playerArms;

    private float xAxisClamp = 0;

     void Start()
    {
        // Mover al start para que se ejecute una sóla vez.
        // Punto 8 del documento de correcciones.
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Cambio de Update a LateUpdate.
    // Punto 4 del documento de correcciones.
    // Se extraen los métodos para cumplir el principio de responsabilidad única
    // y mejorar la legibilidad del código.
    void LateUpdate()
    {
        MoveCamera();
    }

    // Sacar el controlador de la cámara a la función.
    // Punto 9 del documento de correcciones.
    void MoveCamera()
    {
        // Suavizar el movimiento con Time.deltaTime
        // Punto 3 del documento de correcciones.
        float rotateX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime; 
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        // Acumular xAxisClamp con rotateY en lugar de rotateX la cual guarda el movimiento horizontal y no el vertical que se necesita.
        // Punto 2 de documento de correcciones.
        xAxisClamp += rotateY;

        // Limitar la rotación en el eje X (rotación vertical)
        // Punto 6 del documento de correcciones
        xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f);

        // Rotar los brazos del Player en el eje X (rotación vertical)
        // Cambio de rotation.eulerAngles a Quaternion.Euler, punto 5 del documento de correcciones.
        Quaternion targetRotation = Quaternion.Euler(-xAxisClamp, 0f, 0f);
        playerArms.localRotation = targetRotation;

        // Rotar al jugador en el eje Y (rotación horizontal) de una forma más simplificada con una sóla línea de código
        player.Rotate(Vector3.up * rotateX);
    }
}

