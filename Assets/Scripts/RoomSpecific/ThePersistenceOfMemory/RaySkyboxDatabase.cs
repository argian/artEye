using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaySkyboxDatabase : ShaderPasser
{
	[SerializeField] private Camera referenceCamera;
	[SerializeField] private Transform[] markers;
	[SerializeField] private int currentMarker;
	[SerializeField] private float activationDistance = 0.05f;
	
	private VRCPlayerApi localPlayer;
	
	protected override void BakePropertyNames()
	{
		PropertyNames = new string[8];
		PropertyIDs = new int[8];

		PropertyNames[0] = "1";
		PropertyNames[1] = "2";
		PropertyNames[2] = "3";
		PropertyNames[3] = "4";
		PropertyNames[4] = "5";
		PropertyNames[5] = "6";
		PropertyNames[6] = "7";
		PropertyNames[7] = "8";
	}

	protected override void FakeStart()
    {
		localPlayer = Networking.LocalPlayer;
    }

	protected override void PassToRender()
	{
		if (currentMarker == markers.Length - 1)
			return;
		
		//camera pos and rot
		Vector3 playerHeadPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Origin).position;
		
		float lerp = 1f - (Vector3.Distance(markers[currentMarker + 1].position, playerHeadPosition) / Vector3.Distance(markers[currentMarker].position, markers[currentMarker + 1].position));

		if (lerp >= 1f - activationDistance)
		{
			lerp = 1f;
			currentMarker++;
		}

		MainMaterial.SetFloat(PropertyIDs[currentMarker], lerp);
	}
}
