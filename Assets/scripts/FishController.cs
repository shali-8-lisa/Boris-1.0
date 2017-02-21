using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

	public float speed = 10;

	void Start ()
	{
		StartCoroutine (FishThrown ());
	}

	IEnumerator FishThrown ()
	{

		//Vector3 targetPos = Input.mousePosition;
		Vector3 targetPos = new Vector3 (5.0f, -1.2f, 8.0f);
		float distanceToTarget = Vector3.Distance (transform.position, targetPos);
		bool move = true;

		while (move) 
		{
			transform.LookAt (targetPos);
			float angle = Mathf.Min (1, Vector3.Distance (transform.position, targetPos) / distanceToTarget) * 45;
			transform.rotation = transform.rotation * Quaternion.Euler (Mathf.Clamp (-angle, -42, 42), 0, 0);
			float currentDist = Vector3.Distance (transform.position, targetPos);
			if (transform.position == targetPos)
				move = false;
			transform.Translate (Vector3.forward * Mathf.Min (speed * Time.deltaTime, currentDist));
			yield return null;
		}
	}
}
