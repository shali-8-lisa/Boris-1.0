using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

	public float fishSpeed = 10;
	public static Vector3 targetPos;
	public static bool isPenguin;

	private Vector3 landingPos;
	private bool isFed;

	void Start ()
	{
		landingPos = targetPos;
		StartCoroutine (FishThrown ());
		if (isPenguin)
			isFed = true;
		else
			isFed = false;
	}

	IEnumerator FishThrown ()
	{
		float distanceToTarget = Vector3.Distance (transform.position, landingPos);
		bool move = true;

		while (move) 
		{
			transform.LookAt (landingPos);
			float angle = Mathf.Min (1, Vector3.Distance (transform.position, landingPos) / distanceToTarget) * 45;
			transform.rotation = transform.rotation * Quaternion.Euler (Mathf.Clamp (-angle, -42, 42), 0, 0);
			float currentDist = Vector3.Distance (transform.position, landingPos);
			if (transform.position == landingPos)
				move = false;
			transform.Translate (Vector3.forward * Mathf.Min (fishSpeed * Time.deltaTime, currentDist));
			yield return null;
		}

		if (isFed) 
		{
			Destroy (gameObject);
			PenguinController.Hunger--;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Penguin")
		{
			Destroy (gameObject);
			PenguinController.Hunger--;
		}
	}
}

/*
 * Vector3 targetPos = new Vector3 (5.0f, -1.2f, 8.0f);
 * Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
*/
