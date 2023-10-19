
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ThePersistenceOfMemory : UdonSharpBehaviour
{
    public Marker[] markers;
    [SerializeField] private float totalWeightedValue;
    [SerializeField] private Vector3 playerRootBone;
    [SerializeField] private GameObject targetObject;
    
    private Renderer targetRenderer;
    private VRCPlayerApi vrcPlayerApi;

    protected virtual void Start()
    {
        vrcPlayerApi = Networking.LocalPlayer;
        if (targetObject)
            targetRenderer = targetObject.GetComponent<Renderer>();
        else
            Debug.LogWarning("Target object not set!");
    }

    private void Update()
    {
        if (!vrcPlayerApi.isLocal)
            return;

        playerRootBone = vrcPlayerApi.GetPosition(); // FIXME
        
        totalWeightedValue = 0.0f;
        
        for (int i = 0; i < markers.Length; i++)
        {
            float distance = Vector3.Distance(markers[i].transform.position, playerRootBone);
            float weight = 1f / distance;
            float weightedValue = markers[i].weight * weight;
            
            totalWeightedValue += weightedValue;
        }
        
        if (!targetObject)
            return;
        
        targetRenderer.material.SetFloat("value", totalWeightedValue); // TODO set proper name
    }
}
