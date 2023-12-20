
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PersistenceOfMemoryLoader : Loadable
{
	[SerializeField] private RaySkyboxDatabase raySkyboxDatabase;
	
	public override void Load()
	{
		raySkyboxDatabase.Reset();
	}

	public override void Unload() {}
}
