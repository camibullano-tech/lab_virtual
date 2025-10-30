using UnityEngine;
using UnityEngine.Video;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class OneShotVideoStarter : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;     // quello che scrive sulla RenderTexture
    public float startDelay = 0.5f;
    public string playerTag = "Player";

    [Header("Cameras")]
    public Camera playerCam;
    public Camera videoCam;

    private bool started = false;       // true dopo la prima partenza
    private Coroutine startCo;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void Awake()
    {
        if (videoPlayer)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.waitForFirstFrame = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        SetActiveCamera(videoCam);
        if (!started)
        {
            if (startCo != null) StopCoroutine(startCo);
            startCo = StartCoroutine(StartVideoOnce());
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        SetActiveCamera(playerCam);        // torna alla camera del player, ma NON fermare il video
    }

    IEnumerator StartVideoOnce()
    {
        started = true;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        if (!videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared) yield return null;
        }

        videoPlayer.time = 0;   // parti da inizio
        videoPlayer.Play();
    }

    void SetActiveCamera(Camera active)
    {
        if (!playerCam || !videoCam) return;

        bool isPlayer = (active == playerCam);
        playerCam.enabled = isPlayer;
        videoCam.enabled = !isPlayer;      
    }
}
