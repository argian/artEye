
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CameraPortal : UdonSharpBehaviour
{
    public CameraPortal LinkedPortal;
    public Camera ReferenceCamera;
    private Camera PortalCamera;
    private VRCPlayerApi LocalPlayer;

    private RenderTexture PortalTexture;
    public Material PortalMaterial;

    void Start()
    {
        LocalPlayer = Networking.LocalPlayer;
        PortalCamera = gameObject.GetComponentInChildren<Camera>();

        //renderTexture setter
        /*
        PortalTexture = new RenderTexture(ReferenceCamera.pixelHeight, ReferenceCamera.pixelWidth, 24);
        PortalCamera.targetTexture = PortalTexture;

        PortalMaterial.mainTexture = PortalTexture;
        */
        //PortalTexture.height = ReferenceCamera.pixelHeight;
        //PortalTexture.width = ReferenceCamera.pixelWidth;
    }

    private void Update()
    {

        //setting camera:
        //we have to set camera inside the portal relevant to player vs the OTHER portal
        //main camera pos and rot
        //wait, schouldn't I made this separate for both eyes?
        VRCPlayerApi.TrackingData playerHead = LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

        PortalCamera.transform.localPosition = LinkedPortal.transform.rotation * (playerHead.position - LinkedPortal.transform.position);
        PortalCamera.transform.localRotation = LinkedPortal.transform.rotation * playerHead.rotation;

        //PortalCamera.transform.localPosition = new Vector3(PortalCamera.transform.localPosition.x * transform.forward.x, PortalCamera.transform.localPosition.y * transform.forward.y, PortalCamera.transform.localPosition.z * transform.forward.z);



        //change camera frustum to match portal
        //Vector3 posToCamera = new Vector3(transform.position - PortalCamera.transform.position);
        int facing = System.Math.Sign(Vector3.Dot(transform.forward, (transform.position - PortalCamera.transform.position)));
        Vector3 cameraSpacePos = PortalCamera.worldToCameraMatrix.MultiplyPoint(transform.position);
        Vector3 cameraSpaceNormal = PortalCamera.worldToCameraMatrix.MultiplyVector(transform.forward) * facing;
        float cameraSpaceDist = -Vector3.Dot(cameraSpacePos, cameraSpaceNormal);
        Vector4 PortalClipPlane = new Vector4(cameraSpaceNormal.x, cameraSpaceNormal.y, cameraSpaceNormal.z, cameraSpaceDist * 0.9f);
        PortalCamera.projectionMatrix = PortalCamera.CalculateObliqueMatrix(PortalClipPlane);
        //PortalCamera.nearClipPlane = PortalCamera.nearClipPlane * 0.8f;
	}
}
