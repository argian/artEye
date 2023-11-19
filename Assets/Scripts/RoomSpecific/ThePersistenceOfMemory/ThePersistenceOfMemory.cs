using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class ThePersistenceOfMemory : UdonSharpBehaviour
{
    public Marker[] markers;
    [SerializeField] private float totalWeightedValue;
    [SerializeField] private Vector3 playerRootBone;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private string shaderPropertyName;
    
    private VRCPlayerApi vrcPlayerApi;

    protected virtual void Start()
    {
        vrcPlayerApi = Networking.LocalPlayer;
        if (!targetRenderer)
            Debug.LogWarning("Target object not set!");
    }

    private void Update()
    {
        if (!vrcPlayerApi.isLocal)
            return;

        playerRootBone = vrcPlayerApi.GetPosition(); // FIXME
        
        totalWeightedValue = 0.0f;

        float totalDistance = 0f;
        float sumOfNormalizedDistances = 0f;
        float[] normalizedDistances = new float[markers.Length];

        for (int i = 0; i < markers.Length; i++)
        {
            float distance = Vector3.Distance(markers[i].transform.position, playerRootBone);
            totalDistance += distance;
            normalizedDistances[i] = distance / totalDistance;
            sumOfNormalizedDistances += normalizedDistances[i];
        }

        for (int i = 0; i < markers.Length; i++)
        {
            totalWeightedValue += markers[i].weight * (sumOfNormalizedDistances - 2 * normalizedDistances[i]);
        }

        if (!targetRenderer)
            return;
        
        targetRenderer.material.SetFloat(shaderPropertyName, totalWeightedValue);
    }
}
