
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DimensionSorter : UdonSharpBehaviour
{
	public Transform DimensionSpace;
	public Transform[] Portals;
	private VRCPlayerApi LocalPlayer;
	public int DiscardQueue;
	public int AcceptQueue;
	//public int PortalQueue;
	public MeshRenderer[] DimensionMeshes;

	private void Start()
	{
		LocalPlayer = Networking.LocalPlayer;
		//PortalQueue = Portals[0].GetComponent<MeshRenderer>().material.renderQueue;
		DimensionMeshes = DimensionSpace.GetComponentsInChildren<MeshRenderer>(includeInactive: false);
	}

	private void Update()
	{
		//get local player head data (yes, we have to get new one every frame)
		VRCPlayerApi.TrackingData playerHead = LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

		//portal dist:
		float portalDist = Vector3.Distance(playerHead.position, Portals[0].transform.position);
		for (int i = 0; i < DimensionMeshes.Length; ++i)
		{
			//check if object is between player and portal:
			if (Vector3.Distance(playerHead.position, DimensionMeshes[i].transform.position) < portalDist)
			{
				DimensionMeshes[i].material.renderQueue = DiscardQueue;
			}
			else
			{
				DimensionMeshes[i].material.renderQueue = AcceptQueue;
			}
		}
	}
}
