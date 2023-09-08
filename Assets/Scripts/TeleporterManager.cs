using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class TeleporterManager : UdonSharpBehaviour
{
	[SerializeField] private Loadable[] loadOnPlayerRespawn;
	[SerializeField] private Loadable[] unloadOnPlayerRespawn;

	public override void OnPlayerRespawn(VRCPlayerApi player)
	{
		base.OnPlayerRespawn(player);
		foreach (Loadable loadable in unloadOnPlayerRespawn)
		{
			loadable.Unload();
		}
		foreach (Loadable loadable in loadOnPlayerRespawn)
		{
			loadable.Load();
		}
	}
}
