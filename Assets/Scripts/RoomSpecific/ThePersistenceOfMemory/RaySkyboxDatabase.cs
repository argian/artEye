using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaySkyboxDatabase : ShaderPasser
{
	[SerializeField] private Transform[] markers;
	[SerializeField] private int currentMarker;
	[SerializeField] private float activationDistance = 0.05f;
	[SerializeField] private AnimationCurve lerpCurve;
	
	private VRCPlayerApi localPlayer;

	[SerializeField]
	private float YScale;

	[SerializeField]
	private float lerp;


	protected override void BakePropertyNames()
	{
		PropertyNames = new string[8];
		PropertyIDs = new int[8];

		PropertyNames[0] = "Spacing1";
		PropertyNames[1] = "Spacing2";
		PropertyNames[2] = "Spacing3";
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

		float distanceToNextMarker = Vector3.Distance(markers[currentMarker + 1].position, playerHeadPosition);

		lerp = 1f - distanceToNextMarker / Vector3.Distance(markers[currentMarker].position, markers[currentMarker + 1].position);

		if (distanceToNextMarker <= activationDistance)
		{
			lerp = 1f;
			currentMarker++;
		}

		lerp = lerp < 0 ? 0 : lerp;
		lerp = lerpCurve.Evaluate(lerp);
		MainMaterial.SetVector(PropertyIDs[currentMarker], new Vector4(lerp, YScale, lerp, lerp));
	}
}
