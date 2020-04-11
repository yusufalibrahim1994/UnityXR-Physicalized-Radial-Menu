using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectLockableGO : MonoBehaviour
{
	[System.NonSerialized] public bool correctGOinsideLockZone;
	public GameObject ItemLockable;

	//Detects when the SnapItem gameobject has entered the snap zone radius
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == ItemLockable.name)
		{
			correctGOinsideLockZone = true;
		}
	}

	//Detects when the SnapItem gameobject has left the snap zone radius
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == ItemLockable.name)
		{
			correctGOinsideLockZone = false;
		}
	}

}