using UnityEngine;

public class ImogenLive : MonoBehaviour
{
    [Header("Sistema de Vidas")]
    [SerializeField] private int vidas = 100;

    // Propiedad pública para leer las vidas
    public int VidasActuales => vidas;

    private Animator animator;
    private Talking talkingScript;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator
        talkingScript = GetComponent<Talking>(); // Obtiene el script Talking para interrumpir la animación de hablar
    }

    // Método público para recibir daño
    public void RecibirDaño(int cantidad)
    {
        vidas -= cantidad;
        animator.SetTrigger("IsDamage"); // Activa la animación de daño
        Debug.Log("Imogen ha recibido " + cantidad + " de daño. Vidas restantes: " + vidas);

        // Si el script de Talking está presente, interrumpimos la animación de hablar
        if (talkingScript != null)
        {
            talkingScript.RecibirDaño(); // Interrumpe la animación de hablar
        }

        if (vidas <= 0)
        {
            Debug.Log("Imogen ha muerto.");
            // Aquí puedes poner lógica de muerte/desactivación
            // gameObject.SetActive(false);
        }
    }
}