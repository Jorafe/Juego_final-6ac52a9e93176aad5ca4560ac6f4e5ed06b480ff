using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class ImogenLive : MonoBehaviour
{
    [Header("Sistema de Vidas")]
    [SerializeField] private int vidas = 100;
    [SerializeField] private UnityEngine.UI.Slider vidaSlider;
    

    public int VidasActuales => vidas;

    private Animator animator;
    private Talking talkingScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        talkingScript = GetComponent<Talking>();

        if (vidaSlider != null)
        {
            vidaSlider.maxValue = vidas;
            vidaSlider.value = vidas;
        }
    }

    public void RecibirDaño(int cantidad)
    {
        vidas -= cantidad;
        vidas = Mathf.Clamp(vidas, 0, vidas);

        SoundManagerMenu.Instance?.PlayEnemyDamageSFX();

        animator.SetTrigger("IsDamage");
        Debug.Log("Imogen ha recibido " + cantidad + " de daño. Vidas restantes: " + vidas);

        if (talkingScript != null)
        {
            talkingScript.RecibirDaño();
        }

        if (vidaSlider != null)
        {
            vidaSlider.value = vidas;
        }

        if (vidas <= 0)
        {
            Debug.Log("Imogen ha muerto.");
            StoryManager.Instance?.LoadNextStep();
        }
    }
}