
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Cooker : UdonSharpBehaviour
{
	[SerializeField] private bool canInside;
	[SerializeField] private Transform cookerOutput;
	[SerializeField] private GameObject canPrefab;
	private void OnTriggerEnter(Collider other)
	{
		if (canInside)
			return;
		
		Destroy(other.gameObject);
		canInside = true;
	}

	public void CreateCan()
	{
		CampbellCan campbellCan = Instantiate(canPrefab, cookerOutput.position, cookerOutput.rotation).GetComponent<CampbellCan>();
		// TODO
		// campbellCan.SetTexture();
	}
}
