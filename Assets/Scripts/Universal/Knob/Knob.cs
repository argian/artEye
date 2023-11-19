
using System;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public class Knob : UdonSharpBehaviour
{
	[SerializeField] private float[] angles;
	[SerializeField] private string[] values;

	[SerializeField] private float currentAngle;

	// TODO debug variables
	[SerializeField] private Text debugDisplay;

	public string GetValue()
	{
		return values[FindNearestAngle(currentAngle)];
	}
	
	private int FindNearestAngle(float target)
	{
		int nearestIndex = 0;
		float minDifference = Math.Abs(target - angles[0]);

		for (int i = 1; i < angles.Length; i++)
		{
			float difference = Math.Abs(target - angles[i]);

			if (difference < minDifference)
			{
				minDifference = difference;
				nearestIndex = i;
			}
		}

		return nearestIndex;
	}

	public void SetAngle(float angle)
	{
		currentAngle = angle;
	}
	
	// TODO only for debbuging
	public void Interact()
	{
		debugDisplay.text = GetValue();
	}
}
