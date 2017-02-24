using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject Penguin;

	void Start ()
	{
		StartCoroutine (penguinGenerator ());
	}

	IEnumerator penguinGenerator ()
	{
		int amount = 5;
		for (int i = 0; i < amount; i++) 
		{
			Vector3 position = new Vector3 (Random.Range (-50.0f, 50.0f), 0.0f, Random.Range (-50.0f, 50.0f));
			Instantiate (Penguin, position, Quaternion.identity);

			yield return new WaitForEndOfFrame();
		}
	}
}
