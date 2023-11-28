using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using TMPro;

[RequireComponent(typeof(Collider)), UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class NarrativePlayer : UdonSharpBehaviour
{
    public GameObject container;

    public AudioSource audioSource;

    public GameObject textCanvas;

    public TMP_Text textComponent;


    bool isPlaying = false;

    float playbackTime = 0;


#if UNITY_EDITOR
    [Space, TextArea(10, 25)]
    public string text;

    [Space]
    public AudioClip clip;
#endif

    void Start()
    {
        StopAndHide();
        HideText();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerTriggerEnter(player);

        if (!player.isLocal)
            return;

        ShowAndResume();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerTriggerExit(player);

        if (!player.isLocal)
            return;

        StopAndHide();
    }

    void ShowAndResume()
    {
        container.SetActive(true);

        if (isPlaying)
            Play();
    }

    void StopAndHide()
    {
        Stop();

        if (isPlaying && playbackTime < Mathf.Epsilon)
            isPlaying = false;

        container.SetActive(false);
    }

    public void Play()
    {
        audioSource.time = playbackTime;
        audioSource.Play();

        isPlaying = true;
    }

    public void Replay()
    {
        playbackTime = 0;
        Play();
    }

    public void Pause()
    {
        Stop();
        
        isPlaying = false;
    }

    void Stop()
    {
        playbackTime = audioSource.time;
        audioSource.Stop();
    }

    public void ShowText()
    {
        textCanvas.SetActive(true);
    }

    public void HideText()
    {
        textCanvas.SetActive(false);
    }
}
