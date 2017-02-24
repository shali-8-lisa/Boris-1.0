using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour {

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Boris")
			StartCoroutine (reboundPlayer(other));
	}

	IEnumerator reboundPlayer (Collider other)
	{
		Vector3 dir = transform.position - other.transform.position;
		float timeP = 0.0f;
		while (timeP < 1.0f) 
		{
			other.GetComponent<CharacterController>().Move(dir * (1.0f - timeP) * 0.1f);
			timeP += Time.deltaTime;
			yield return null;
		}
	}
}
