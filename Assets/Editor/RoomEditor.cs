using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
	private Room room;

	private void OnEnable()
	{
		room = (Room)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Get loadable objects"))
			FindLoadableObjects();
	}

	private void FindLoadableObjects()
	{
		List<Loadable> loadableObjects = new List<Loadable>();
		foreach (Transform t in room.transform)
			loadableObjects.AddRange(t.GetComponentsInChildren<Loadable>().ToList());
		room.roomObjects = loadableObjects.ToArray();
	}
}