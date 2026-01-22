using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Player player;   // 👉 arrástralo desde el Inspector

    [Header("UI")]
    public Image rellenoBarraVida;

    public float vidaMax;

    void Start()
    {
        // Guardamos la vida máxima al empezar
        vidaMax = player.Vida;
    }

    void Update()
    {
        // Normalizamos la vida (0 a 1)
        rellenoBarraVida.fillAmount = player.Vida / vidaMax;
    }
}
