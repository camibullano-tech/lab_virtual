using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camere")]
    public Camera playerCam;      // Camera del personaggio
    public Camera screenCam;      // Camera dello schermo

    void Start()
    {
        SetActiveCam(playerCam); // All'inizio usa la camera del player
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetActiveCam(screenCam);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetActiveCam(playerCam);
    }

    void SetActiveCam(Camera active)
    {
        bool toPlayer = active == playerCam;

        playerCam.enabled = toPlayer;
        screenCam.enabled = !toPlayer;

        // Gestione AudioListener
        // var al1 = playerCam.GetComponent<AudioListener>();
        // var al2 = screenCam.GetComponent<AudioListener>();
        // if (al1) al1.enabled = toPlayer;
        // if (al2) al2.enabled = !toPlayer;
    }
}
