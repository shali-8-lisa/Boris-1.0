using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacierController : MonoBehaviour {

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boris") {
			Debug.Log ("bbbbbbb");
			StartCoroutine (reboundPlayer (other));
		}
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
