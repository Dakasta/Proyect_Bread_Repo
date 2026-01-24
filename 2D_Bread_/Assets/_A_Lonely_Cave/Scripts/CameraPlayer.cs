using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script de Unity | 0 referencias
public class CameraPlayer : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.025f;
    public Vector3 desplazamiento;

    // Mensaje de Unity | 0 referencias
    private void LateUpdate()
    {
        Vector3 posicionDeseada = objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);
        transform.position = posicionSuavizada;
    }
}
