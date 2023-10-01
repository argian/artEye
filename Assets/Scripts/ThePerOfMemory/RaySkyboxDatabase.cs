using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaySkyboxDatabase : ShaderPasser
{
	public Camera ReferenceCamera;
	private VRCPlayerApi LocalPlayer;

	protected override void BakePropertyNames()
	{
		PropertyNames = new string[3];
		PropertyIDs = new int[3];

		PropertyNames[0] = "CameraPos";
		PropertyNames[1] = "CameraDir";
		PropertyNames[2] = "Fov";
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
		//MainMaterial.SetVector(PropertyIDs[1], playerHead.rotation.eulerAngles);
		MainMaterial.SetVector(PropertyIDs[1], playerHead.rotation * Vector3.forward);

		MainMaterial.SetFloat(PropertyIDs[2], ReferenceCamera.fieldOfView);
		//Debug.Log(playerHead.rotation );
	}
}
