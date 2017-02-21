
//This is Boris!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float speedR;
	public GameObject fish;

	void Start ()
	{

	}

	void Update ()
	{
		//Run - tilt
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		transform.Translate (moveHorizontal * speed * Time.deltaTime, 0.0f, moveVertical * speed * Time.deltaTime);
		transform.Rotate (Vector3.up * moveHorizontal * speedR * Time.deltaTime);

		//Throw fish
		if (Input.GetMouseButtonDown (0))
			Instantiate (fish, transform.position, Quaternion.identity);
	}


}
