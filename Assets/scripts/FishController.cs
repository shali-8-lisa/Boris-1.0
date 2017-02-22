using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

	public float fishSpeed = 10;
	public static Vector3 targetPos;

	private Vector3 landingPos;

	void Start ()
	{
		landingPos = targetPos;
		StartCoroutine (FishThrown ());
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
	}
}

/*
 * Vector3 targetPos = new Vector3 (5.0f, -1.2f, 8.0f);
 * Vector3 targetPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
*/
