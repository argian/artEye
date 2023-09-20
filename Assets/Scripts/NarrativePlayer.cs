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

#if UNITY_EDITOR
    [Space, TextArea(1, 25)]
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

        ShowAndPlay();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        base.OnPlayerTriggerExit(player);

        StopAndHide();
    }

    void ShowAndPlay()
    {
        container.SetActive(true);
        audioSource.Play();
    }

    void StopAndHide()
    {
        audioSource.Stop();
        container.SetActive(false);
    }
}
