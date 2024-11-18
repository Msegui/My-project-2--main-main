using UnityEngine;
using UnityEngine.SceneManagement;

public class Pilar : MonoBehaviour
{
    public Sprite[] estadosPilar; // Arreglo para almacenar los sprites del pilar
    private SpriteRenderer spriteRenderer;
    private int indiceEstado = 0; // Índice que representa el estado actual del pilar
    private GeneradorEnemigos generadorEnemigos;

    private Vector3 ultimaPosicionMouse; // Última posición del mouse
    private bool cursorSobrePilar = false;
    private bool pilarDestruido = false; // Controla si el pilar está destruido
    public float tiempoReparacion = 0.5f; // Intervalo de tiempo entre cada reparación
    private float contadorReparacion = 0f; // Contador de tiempo para la reparación

    private bool enemigosGenerados = false; // Controla si ya se han generado enemigos al reparar

    // Sonido que se reproduce cuando un enemigo colisiona con el pilar
    public AudioClip sonidoRoboCristal;
    private AudioSource audioSource;
    // Sonnido que suena cuando estas en el pilar
    public AudioClip sonidoMusicaAmbiente;
    private AudioSource audioSource2;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        indiceEstado = estadosPilar.Length - 1;
        spriteRenderer.sprite = estadosPilar[indiceEstado];

        generadorEnemigos = FindObjectOfType<GeneradorEnemigos>();

        if (estadosPilar.Length == 0)
        {
            Debug.LogError("No se han asignado sprites al arreglo de estados del pilar.");
        }

        ultimaPosicionMouse = Input.mousePosition;

        // Configura el audio
        audioSource  = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoRoboCristal;
        audioSource.playOnAwake = false; // Para que el sonido solo se reproduzca en la colisión
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoMusicaAmbiente;
        audioSource2.playOnAwake = false; // Para que el sonido solo se reproduzca cuando se esta en el pilar
    }

    void Update()
    {
        if (pilarDestruido) return;

        Vector3 posicionActualMouse = Input.mousePosition;

        if (cursorSobrePilar && posicionActualMouse != ultimaPosicionMouse)
        {
            contadorReparacion += Time.deltaTime;
            if (contadorReparacion >= tiempoReparacion)
            {
                RetrocederEstadoPilar();
                contadorReparacion = 0f;
            }
        }
        else
        {
            contadorReparacion = 0f;
        }

        ultimaPosicionMouse = posicionActualMouse;
    }

    private void OnMouseEnter()
    {
        cursorSobrePilar = true;
    }

    private void OnMouseExit()
    {
        cursorSobrePilar = false;
    }

    public void RetrocederEstadoPilar()
    {
        if (indiceEstado > 0)
        {
            indiceEstado--;
            spriteRenderer.sprite = estadosPilar[indiceEstado];
            
            // Si el pilar está completamente reparado, activa el generador de enemigos solo una vez
            if (indiceEstado == 0 && generadorEnemigos != null && !enemigosGenerados)
            {
                generadorEnemigos.GenerarEnemigos();
                enemigosGenerados = true; // Marcar que los enemigos han sido generados
            }
        }
    }

    public void AvanzarEstadoPilar()
    {
        if (indiceEstado < estadosPilar.Length - 1)
        {
            indiceEstado++;
            spriteRenderer.sprite = estadosPilar[indiceEstado];
        }
        else
        {
            Debug.Log("El pilar ya ha perdido todas sus piedras preciosas.");
            
            if (generadorEnemigos != null)
            {
                generadorEnemigos.EliminarEnemigos();
            }

            pilarDestruido = true;

            SceneManager.LoadScene("FinDelJuego");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            AvanzarEstadoPilar();
            
            // Reproduce el sonido de colisión
            if (audioSource != null && sonidoRoboCristal != null)
            {
                audioSource.Play();
            }
        }
        If(new Vector3(-2.452202,0.13,-10))
        {
       AudioSource.PlayClipAtPoint(sonidoMusicaAmbiente,new Vector3(-2.452202,0.13,-10), 1);
        }
    }
}