using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Button State")]
    public bool isActivated = false;

    [Header("Optional")]
    public Sprite activatedSprite;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo se activa una vez
        if (isActivated) return;

        if (collision.CompareTag("Player"))
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        isActivated = true;

    
        if (activatedSprite != null && sr != null)
        {
            sr.sprite = activatedSprite;
        }

        Debug.Log("Botón activado");
    }
}