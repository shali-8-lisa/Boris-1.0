
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

		//Throw fish - to where clicked
		if (Input.GetMouseButtonDown (0))
			Instantiate (fish, transform.position, Quaternion.identity);

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (transform.position, ray.direction * 100, Color.black);

		if (Physics.Raycast (ray, out hit)) 
		{
			//FishController.targetPos = hit.point;


			if (hit.collider.tag == "Glacier") 
			{
				Debug.Log ("Hit");
				FishController.targetPos = hit.point;
			} 
			else
				Debug.Log ("No Hit");
		}
		

	}
}
