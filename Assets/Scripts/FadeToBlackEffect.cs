using UnityEngine;

public class FadeToBlackEffect : MonoBehaviour
{
    public float fadeDuration = 3f; // Duración en segundos del fundido
    private SpriteRenderer spriteRenderer;
    private Color color;

    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Este script necesita estar en un GameObject con un SpriteRenderer.");
            return;
        }

        color = spriteRenderer.color;
        color.a = 0; // Comienza completamente transparente
        spriteRenderer.color = color;

        StartFade(); // Llama a StartFade automáticamente al inicio
    }


    void Update()
    {
        if (isFading)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration); // Valor entre 0 y 1

            // Incrementa la transparencia a medida que avanza el tiempo
            color.a = progress;
            spriteRenderer.color = color;

            // Detiene el efecto una vez que la transparencia llega a 1 (completamente negro)
            if (progress >= 1f)
            {
                isFading = false;
            }
        }
    }

    public void StartFade()
    {
        timer = 0f;
        isFading = true;
    }
}