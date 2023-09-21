using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using TMPro;

[RequireComponent(typeof(Collider))]
public class NarrativePlayer : UdonSharpBehaviour
{
    public GameObject container;

    public AudioSource audioSource;

    public TMP_Text textComponent;

    [Space]
    public bool playOnce;

    bool skipClip;

    float playbackTime;


#if UNITY_EDITOR
    [Space, TextArea(10, 25)]
    public string text;

    [Space]
    public AudioClip clip;
#endif

    void Start()
    {
        StopAndHide();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        base.OnPlayerTriggerEnter(player);

        if (!player.isLocal)
            return;

        ShowAndPlay();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerTriggerExit(player);

        if (!player.isLocal)
            return;

        StopAndHide();

        if (playOnce && playbackTime < Mathf.Epsilon)
            skipClip = true;
    }

    void ShowAndPlay()
    {
        container.SetActive(true);

        if (skipClip)
            return;

        audioSource.time = playbackTime;
        audioSource.Play();
    }

    void StopAndHide()
    {
        playbackTime = audioSource.time;
        audioSource.Stop();

        container.SetActive(false);
    }
}
