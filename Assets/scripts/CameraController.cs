using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float distanceAway;
	public float distanceUp;
	public float smooth;

	private Vector3 targetPosition;

	Transform follow;

	void Start()
	{
		follow = player.transform;
	}

	void LateUpdate () 
	{
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		transform.LookAt (follow);
	}
		
}
