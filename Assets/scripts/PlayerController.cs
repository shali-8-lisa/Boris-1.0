
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
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (transform.position, ray.direction * 100, Color.black);

		if (Physics.Raycast (ray, out hit)) 
		{
			if (hit.collider.tag == "Penguin") {
				if (Input.GetMouseButtonDown (0)) {
					FishController.targetPos = hit.point;
					FishController.isPenguin = true;
					Instantiate (fish, transform.position, Quaternion.identity);
				}
			} else if (hit.collider.tag == "Land") {

				if (Input.GetMouseButtonDown (0)) {
					FishController.targetPos = hit.point;
					FishController.isPenguin = false;
					Instantiate (fish, transform.position, Quaternion.identity);
				}
			}
		}
		

	}
}
