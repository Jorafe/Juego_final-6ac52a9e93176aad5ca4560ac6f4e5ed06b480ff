using UnityEngine;

public class ImogenLive : MonoBehaviour
{
    [Header("Sistema de Vidas")]
    [SerializeField] private int vidas = 100;

    // Propiedad pública para leer las vidas
    public int VidasActuales => vidas;

    // Método público para recibir daño
    public void RecibirDaño(int cantidad)
    {
        vidas -= cantidad;
        Debug.Log("Imogen ha recibido " + cantidad + " de daño. Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            Debug.Log("Imogen ha muerto.");
            // Aquí puedes poner lógica de muerte/desactivación
            // gameObject.SetActive(false);
        }
    }
}