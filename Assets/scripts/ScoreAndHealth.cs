using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndHealth : MonoBehaviour {

	public Text penguinHungerText;
	public Text BorisSpeedText;

	void Start ()
	{
		SetCountText ();
	}

	void Update ()
	{
		SetCountText ();
	}

	void SetCountText ()
	{
		penguinHungerText.text = "Penguin Hunger: " + PenguinController.Hunger;
		BorisSpeedText.text = "Boris speed: " + PlayerController.BorisSpeed;
	}

}
