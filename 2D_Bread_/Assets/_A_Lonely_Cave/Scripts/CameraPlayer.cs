using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform target;
    public float offsetX = 2f;      // Cuánto se adelanta
    public float offsetY = 1f;
    public float smoothSpeed = 0.15f;

    private float currentOffsetX;

    void LateUpdate()
    {
        if (target == null) return;

        // Detecta hacia dónde mira el personaje
        float direction = Mathf.Sign(target.localScale.x);
        float targetOffsetX = offsetX * direction;

        // Suaviza el cambio de lado
        currentOffsetX = Mathf.Lerp(
            currentOffsetX,
            targetOffsetX,
            smoothSpeed
        );

        Vector3 targetPosition = new Vector3(
            target.position.x + currentOffsetX,
            target.position.y + offsetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed
        );
    }

}