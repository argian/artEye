
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaySkyboxDatabase : ShaderPasser
{

	private VRCPlayerApi LocalPlayer;

	protected override void BakePropertyNames()
	{
		PropertyNames = new string[2];
		PropertyIDs = new int[2];

		PropertyNames[0] = "CameraPos";
		PropertyNames[1] = "CameraDir";
	}

	protected override void FakeStart()
    {
		LocalPlayer = Networking.LocalPlayer;
	}

	protected override void PassToRender()
	{
		//camera pos and rot
		VRCPlayerApi.TrackingData playerHead = LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

		MainMaterial.SetVector(PropertyIDs[0], playerHead.position);
		MainMaterial.SetVector(PropertyIDs[1], playerHead.rotation.eulerAngles);

		Debug.Log(playerHead.rotation.eulerAngles);
	}
}
