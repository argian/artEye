
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DimensionSorter : UdonSharpBehaviour
{
	public DimensionalLens[] portals;
	private VRCPlayerApi localPlayer;
	[SerializeField] private int discardQueue;
	[SerializeField] private int acceptQueue;

	//public int PortalQueue;
	public MeshRenderer[] dimensionMeshes;
	[SerializeField] private MeshRenderer room;

	private void Start()
	{
		localPlayer = Networking.LocalPlayer;
		dimensionMeshes = GetComponentsInChildren<MeshRenderer>(includeInactive: false);
	}

	private void Update()
	{
		if (portals.Length == 0)
			return;
		
		//get local player head data (yes, we have to get new one every frame)
		VRCPlayerApi.TrackingData playerHead = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

		//portal dist:
		float portalDist = Vector3.Distance(playerHead.position, portals[0].transform.position);
		for (int i = 0; i < dimensionMeshes.Length; ++i)
		{
			//check if object is between player and portal:
			if (Vector3.Distance(playerHead.position, dimensionMeshes[i].transform.position) < portalDist)
			{
				dimensionMeshes[i].material.renderQueue = discardQueue;
			}
			else
			{
				dimensionMeshes[i].material.renderQueue = acceptQueue;
			}
		}
	}

	public void SetDiscardQueue(int value)
	{
		discardQueue = value;
	}
	
	public void SetAcceptQueue(int value)
	{
		acceptQueue = value;
		room.material.renderQueue = acceptQueue;
	}
}
