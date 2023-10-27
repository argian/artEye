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
		PropertyNames = new string[2];
		PropertyIDs = new int[2];

		PropertyNames[0] = "HeadPos";
		PropertyNames[1] = "Spacing";
		/*
		PropertyNames[1] = "CameraDir";
		PropertyNames[2] = "CameraRot";
		PropertyNames[3] = "AspectRatio";
		PropertyNames[4] = "Fov";
		*/
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
		MainMaterial.SetVector(PropertyIDs[1], LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
		Debug.Log(LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Origin).position);
		/*
		MainMaterial.SetVector(PropertyIDs[1], playerHead.rotation * Vector3.forward);
		MainMaterial.SetVector(PropertyIDs[2], playerHead.rotation.eulerAngles);
		MainMaterial.SetFloat(PropertyIDs[3], ReferenceCamera.aspect);

		//make it more optymized in future
		MainMaterial.SetVector(PropertyIDs[4], new Vector4(Camera.VerticalToHorizontalFieldOfView(ReferenceCamera.fieldOfView, ReferenceCamera.aspect), ReferenceCamera.fieldOfView, 0, 0));
		//Debug.Log(ReferenceCamera.fieldOfView);
		*/
	}
}
