using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaySkyboxDatabase : ShaderPasser
{
	[SerializeField] private SkinnedMeshRenderer treeMesh;
	[SerializeField] private ClockAnimator[] clockAnimators;
	[SerializeField] private ClockCentralizer[] clockCentralizers;
	[SerializeField] private Transform[] markers;
	[SerializeField] private int currentMarker;
	[SerializeField] private float activationDistance = 0.05f;
	[SerializeField] private AnimationCurve lerpCurve;
	
	private VRCPlayerApi localPlayer;

	[SerializeField]
	private float YScale;

	[SerializeField]
	private float lerp;

	public void Reset()
	{
		currentMarker = 0;
		lerp = 0;
	}

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

		for (int i = 0; i < clockAnimators.Length; i++)
		{
			clockAnimators[i].reanimate(lerp);
		}

		float markerPart = 1f / (float)(markers.Length - 1);
		for (int i = 0; i < clockCentralizers.Length; i++)
		{
			clockCentralizers[i].threshold = markerPart * currentMarker + lerp * markerPart;
		}

		if (currentMarker % 2 > 0)
		{
			treeMesh.SetBlendShapeWeight(0, 100 - (markerPart * currentMarker + lerp * markerPart * 400));
		}
		else
		{
			treeMesh.SetBlendShapeWeight(0, markerPart * currentMarker + lerp * markerPart * 400);
		}

		Debug.Log(markerPart * currentMarker + lerp * markerPart * 100);
	}
}
