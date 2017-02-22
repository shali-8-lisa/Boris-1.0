using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinController : MonoBehaviour {

	//Chase Boris!

	//Fed
	public static int Hunger;

	void Start ()
	{
		Hunger = Random.Range (90, 120);
	}

	void Update ()
	{
		Debug.Log ("Hunger: " + Hunger);
	}
}
