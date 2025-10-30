using UnityEngine;
using UnityEngine.Video;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(VideoPlayer))]
public class VideoTrigger : MonoBehaviour
{
    [Header("Camere")]
    public Camera playerCam;       // la camera del personaggio
    public Camera videoCam;        // la camera dello schermo

    [Header("Delay prima dell'avvio del video")]
    public float startDelay = 2f;  // secondi di attesa

    private VideoPlayer videoPlayer;
    private Coroutine playCoroutine;

    void Start()
    {
        // Recupera automaticamente il componente VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();

        // Impostazioni di sicurezza
        videoPlayer.playOnAwake = false;
        videoPlayer.Stop();

        // All'avvio, mostra la camera del player
        SetActiveCamera(playerCam);

        // Assicurati che il BoxCollider sia trigger
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Passa subito alla camera video
            SetActiveCamera(videoCam);

            // Avvia la coroutine che aspetta 2s prima di far partire il video
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);

            playCoroutine = StartCoroutine(StartVideoWithDelay());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Se esce prima del delay, ferma la coroutine
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);

            // Torna alla camera del player
            SetActiveCamera(playerCam);

            // Metti in pausa il video
            videoPlayer.Pause();
        }
    }

    // Coroutine per avviare il video con ritardo
    IEnumerator StartVideoWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    // Funzione per attivare/disattivare le camere e gestire gli AudioListener
    void SetActiveCamera(Camera activeCam)
    {
        bool isPlayer = (activeCam == playerCam);

        playerCam.enabled = isPlayer;
        videoCam.enabled = !isPlayer;

        // Gestione AudioListener (Unity non ne vuole due attivi insieme)
        var al1 = playerCam.GetComponent<AudioListener>();
        var al2 = videoCam.GetComponent<AudioListener>();
        if (al1) al1.enabled = isPlayer;
        if (al2) al2.enabled = !isPlayer;
    }
}
